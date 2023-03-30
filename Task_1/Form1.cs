namespace Task_1
{
    public partial class Form1 : Form
    {
        readonly string[] words = { "абракадабра1", "абракадабра2", "абракадабра3" };

        public Form1()
        {
            InitializeComponent();
        }

        void BtnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            progressBar.Maximum = 0;
            progressBar.Value = 0;
            labelProgressBar.Text = "Search .txt files and forbidden words";

            string[] drives = GetDrives();

            IEnumerable<string>[] arrayFiles = new IEnumerable<string>[drives.Length];

            for (int i = 0; i < drives.Length; i++)
            {
                arrayFiles[i] = GetDirectoryFiles(drives[i], "*.txt", SearchOption.AllDirectories);
            }

            IEnumerable<string>? files = JoinMany(arrayFiles);

            if (!files.Any())
            {
                MessageBox.Show("Файлы не найдены!");
                Environment.Exit(-1);
            }

            progressBar.Maximum = files.Count();
            List<string> source = new();
            foreach (var file in files)
            {
                if (SearchWords(file, words))
                {
                    source.Add(file);
                }
                progressBar.Value++;
            }

            CopyFilesAndReplaceWord(source, progressBar, labelProgressBar, words);

            btnStart.Enabled = true;
        }

        static async void ReplaceWords(string path, params string[] words)
        {
            string text = await File.ReadAllTextAsync(path);
            foreach (var word in words)
            {
                text = text.Replace(word, "*******");
            }
            await File.WriteAllTextAsync(path, text);
        }

        void CopyFilesAndReplaceWord(List<string> source, ProgressBar progressBar, Label label, params string[] words)
        {
            try
            {
                if (Directory.Exists(Directory.GetCurrentDirectory() + "\\CopyFolder\\"))
                {
                    Directory.Delete(Directory.GetCurrentDirectory() + "\\CopyFolder\\", true);
                }
            }
            catch { }

            var newDirectory = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\CopyFolder\\");

            if (source.Count > 0)
            {
                progressBar.Maximum = source.Count;
                progressBar.Value = 0;
                label.Text = "CopyFiles";
                for (int i = 0; i < source.Count; i++)
                {
                    try
                    {
                        File.Copy(source[i], newDirectory + Path.GetFileName(source[i]));
                        ReplaceWords(newDirectory + Path.GetFileName(source[i]), words);
                        new Thread(new ThreadStart(
                            () =>
                            txtResultSearch.Text += newDirectory + Path.GetFileName(source[i]) + Environment.NewLine))
                            .Start();
                    }
                    catch {}
                    progressBar.Value++;
                }
            }
        }

        static string[] GetDrives()
        {
            List<string> drives = new();
            DriveInfo[] infoDrives = DriveInfo.GetDrives();

            foreach (var item in infoDrives)
            {
                if (item.IsReady)
                {
                    drives.Add(item.Name);
                }
            }
            return drives.ToArray();
        }

        static IEnumerable<T> JoinMany<T>(params IEnumerable<T>[] array)
        {
            var final = array.Where(x => x != null).SelectMany(x => x);
            return final ?? Enumerable.Empty<T>();
        }

        static bool SearchWords(string file, params string[] words)
        {
            try
            {
                foreach (var line in File.ReadLines(file))
                {
                    foreach (var word in words)
                    {
                        if (line.Contains(word, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            catch { }

            return false;
        }

        static IEnumerable<string> GetDirectoryFiles(string rootPath, string patternMatch, SearchOption searchOption)
        {
            foreach (string file in Directory.EnumerateFiles(rootPath, patternMatch))
            {
                yield return file;
            }

            if (searchOption == SearchOption.AllDirectories)
            {
                IEnumerator<string> enumarator = Directory.EnumerateDirectories(rootPath, string.Empty, searchOption).GetEnumerator();
                while (true)
                {
                    bool skip = true;
                    try
                    {
                        if (!enumarator.MoveNext())
                            break;
                        skip = false;
                    }
                    catch { }

                    if (skip)
                        continue;

                    foreach (string file in Directory.EnumerateFiles(enumarator.Current, patternMatch))
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}