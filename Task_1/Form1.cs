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
                arrayFiles[i] = GetDirectoryFiles(drives[i], "*.tttt", SearchOption.AllDirectories);
            }

            IEnumerable<string>? files = JoinMany(arrayFiles);

            if (!files.Any())
            {
                MessageBox.Show("Файлы не найдены!");
                Environment.Exit(-1);
            }

            if (files != null)
            {
                progressBar.Maximum = files.Count();
                foreach (var file in files)
                {
                    if (SearchWords(file, "word"))
                    {
                        txtResultSearch.Text += file + Environment.NewLine;
                    }
                    progressBar.Value++;
                }
            }
        }

        static string[] GetDrives()
        {
            string[] drives = Directory.GetLogicalDrives();
            if (drives.Length == 0)
            {
                MessageBox.Show("Директории не найдены!");
                Environment.Exit(-1);
            }
            return drives;
        }

        static IEnumerable<T> JoinMany<T>(params IEnumerable<T>[] array)
        {
            var final = array.Where(x => x != null).SelectMany(x => x);
            return final ?? Enumerable.Empty<T>();
        }

        static bool SearchWords(string file, string word)
        {
            foreach (var line in File.ReadLines(file))
            {
                if (line.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
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