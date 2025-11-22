using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace FileHashTool
{
    public partial class Form1 : Form
    {
        // 已加入的檔案路徑（保持加入順序）
        private readonly List<string> _filePaths = new();
        // 用來避免重複加入檔案
        private readonly HashSet<string> _filePathSet = new(StringComparer.OrdinalIgnoreCase);
        // 檔案路徑 -> MD5 快取，避免重複計算
        private readonly ConcurrentDictionary<string, string> _hashCache = new(StringComparer.OrdinalIgnoreCase);
        private readonly object _syncRoot = new();
        private string _currentFilter = string.Empty;
        // 控制並行計算的取消權杖，確保移除時可以中止舊的運算
        private readonly object _computeLock = new();
        private CancellationTokenSource? _computeCts;

        public Form1()
        {
            InitializeComponent();
        }

        private async void SelectFilesButton_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "選擇檔案"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await AddFilesAsync(dialog.FileNames);
            }
        }

        private void FileListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private async void FileListBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is string[] droppedFiles && droppedFiles.Length > 0)
            {
                await AddFilesAsync(droppedFiles);
            }
        }

        private async Task AddFilesAsync(IEnumerable<string> files)
        {
            var added = false;

            foreach (var file in files)
            {
                if (!File.Exists(file))
                {
                    continue;
                }

                if (_filePathSet.Add(file))
                {
                    lock (_syncRoot)
                    {
                        _filePaths.Add(file);
                    }
                    added = true;
                }
            }

            if (added)
            {
                RefreshFileList();
                await UpdateOutputAsync();
            }
        }

        private void RefreshFileList()
        {
            var filtered = GetFilteredPaths();
            fileListBox.BeginUpdate();
            fileListBox.Items.Clear();
            fileListBox.Items.AddRange(filtered.Select(p => new FileEntry(p)).Cast<object>().ToArray());
            fileListBox.EndUpdate();
        }

        private async Task UpdateOutputAsync(bool computeMissingOnly = true)
        {
            CancellationTokenSource? previousCts;
            lock (_computeLock)
            {
                previousCts = _computeCts;
                _computeCts = null;
            }
            previousCts?.Cancel();
            previousCts?.Dispose();

            var snapshot = GetFilteredPaths();

            if (snapshot.Count == 0)
            {
                outputTextBox.Text = string.Empty;
                UpdateProgressUI(0, 0);
                return;
            }

            // 只計算還沒有快取的檔案，避免重複運算
            var targets = computeMissingOnly
                ? snapshot.Where(p => !_hashCache.ContainsKey(p)).ToList()
                : snapshot.ToList();

            if (targets.Count == 0)
            {
                outputTextBox.Text = BuildOutputText(snapshot);
                UpdateProgressUI(0, 0);
                return;
            }

            var total = targets.Count;
            UpdateProgressUI(0, total);

            CancellationToken token;
            CancellationTokenSource cts;
            lock (_computeLock)
            {
                _computeCts?.Cancel();
                _computeCts?.Dispose();
                _computeCts = new CancellationTokenSource();
                cts = _computeCts;
                token = cts.Token;
            }

            try
            {
                var builder = await Task.Run(() =>
                {
                    var processed = 0;
                    var options = new ParallelOptions
                    {
                        MaxDegreeOfParallelism = Math.Max(Environment.ProcessorCount, 1),
                        CancellationToken = token
                    };

                    // 並行計算 MD5，避免大量檔案時 UI 卡住
                    Parallel.ForEach(targets, options, filePath =>
                    {
                        token.ThrowIfCancellationRequested();
                        var fileName = Path.GetFileName(filePath);
                        var hash = CalculateMd5(filePath);

                        _hashCache[filePath] = hash;
                        var current = Interlocked.Increment(ref processed);
                        ReportProgress(current, total);
                    });

                    return BuildOutputText(snapshot);
                }, token).ConfigureAwait(true);

                if (token.IsCancellationRequested)
                {
                    return;
                }

                outputTextBox.Text = builder;
                UpdateProgressUI(total, total);
            }
            catch (OperationCanceledException)
            {
                // Swallow cancellations triggered by new updates (e.g., removals while hashing).
                UpdateProgressUI(0, 0);
            }
            finally
            {
                lock (_computeLock)
                {
                    if (_computeCts == cts)
                    {
                        _computeCts = null;
                    }
                }
                cts.Dispose();
            }
        }

        private static string CalculateMd5(string filePath)
        {
            try
            {
                using var stream = File.OpenRead(filePath);
                var hash = MD5.HashData(stream);
                return Convert.ToHexString(hash);
            }
            catch (Exception ex)
            {
                return $"N/A (錯誤: {ex.Message})";
            }
        }

        private async void RemoveFilesButton_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItems.Count == 0)
            {
                return;
            }

            var selectedEntries = fileListBox.SelectedItems
                .Cast<FileEntry>()
                .Select(e => e.Path)
                .ToList();

            lock (_syncRoot)
            {
                foreach (var path in selectedEntries)
                {
                    if (_filePathSet.Remove(path))
                    {
                        var index = _filePaths.IndexOf(path);
                        if (index >= 0)
                        {
                            _filePaths.RemoveAt(index);
                        }
                        _hashCache.TryRemove(path, out _);
                    }
                }
            }

            RefreshFileList();
            await UpdateOutputAsync(computeMissingOnly: true);
        }

        private void ReportProgress(int processed, int total)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int, int>(ReportProgress), processed, total);
                return;
            }

            UpdateProgressUI(processed, total);
        }

        private void FileListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                for (var i = 0; i < fileListBox.Items.Count; i++)
                {
                    fileListBox.SetSelected(i, true);
                }

                e.SuppressKeyPress = true;
            }
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            _currentFilter = searchTextBox.Text ?? string.Empty;
            RefreshFileList();
            _ = UpdateOutputAsync();
        }

        private void CopyAllButton_Click(object sender, EventArgs e)
        {
            var content = outputTextBox.Text;
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            try
            {
                Clipboard.SetText(content);
                progressLabel.Text = "已複製到剪貼簿";
            }
            catch
            {
                MessageBox.Show("複製時發生錯誤，請再試一次。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string BuildOutputText(IEnumerable<string> orderedPaths)
        {
            var builder = new StringBuilder();

            foreach (var filePath in orderedPaths)
            {
                var fileName = Path.GetFileName(filePath);
                // 從快取取出 MD5，未完成計算時顯示計算中
                var hash = _hashCache.TryGetValue(filePath, out var cached)
                    ? cached
                    : "計算中...";

                builder.AppendLine($"檔案: {fileName}");
                builder.AppendLine($"MD5: {hash}");
                builder.AppendLine();
            }

            return builder.ToString().TrimEnd();
        }

        private List<string> GetFilteredPaths()
        {
            List<string> snapshot;
            lock (_syncRoot)
            {
                snapshot = _filePaths.ToList();
            }

            if (string.IsNullOrWhiteSpace(_currentFilter))
            {
                return snapshot;
            }

            // 依搜尋關鍵字篩選（忽略大小寫，只比對檔名）
            var term = _currentFilter.Trim();
            return snapshot
                .Where(p => Path.GetFileName(p).Contains(term, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private sealed record FileEntry(string Path)
        {
            public override string ToString() => System.IO.Path.GetFileName(Path);
        }

        private void UpdateProgressUI(int processed, int total)
        {
            if (total <= 0)
            {
                progressBar.Visible = false;
                progressLabel.Text = "待命";
                return;
            }

            progressBar.Visible = true;
            progressBar.Maximum = total;
            progressBar.Value = Math.Min(processed, total);
            progressLabel.Text = processed >= total
                ? "完成"
                : $"處理中... {processed}/{total}";
        }
    }
}
