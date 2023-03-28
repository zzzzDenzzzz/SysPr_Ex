namespace Task_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void BtnStart_Click(object sender, EventArgs e)
        {
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
                if (SearchWords(file, "абракадабра1", "абракадабра2", "абракадабра3"))
                {
                    txtResultSearch.Text += file + Environment.NewLine;
                    source.Add(file);
                }
                progressBar.Value++;
            }

            CopyFiles(source, progressBar, labelProgressBar);
        }

        static void CopyFiles(List<string> source, ProgressBar progressBar, Label label)
        {
            try
            {
                if (Directory.Exists(Directory.GetCurrentDirectory() + "\\CopyFolder\\"))
                {
                    Directory.Delete(Directory.GetCurrentDirectory() + "\\CopyFolder\\", true);
                }
                var newDirectory = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\CopyFolder\\");
                if (source.Count > 0)
                {
                    progressBar.Maximum = source.Count;
                    progressBar.Value = 0;
                    label.Text = "CopyFiles";
                    for (int i = 0; i < source.Count; i++)
                    {
                        File.Copy(source[i], newDirectory + Path.GetFileName(source[i]), false);
                        progressBar.Value++;
                    }
                    while (progressBar.Value != progressBar.Maximum)
                    {
                        progressBar.Value++;
                    }
                }
            }
            catch { }
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