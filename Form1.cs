using AngleSharp.Html.Parser;
using AngleSharp;
using System.Configuration;
using System.Formats.Tar;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;

namespace RomashkaParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) => { }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = @"C:\Users\Poma\Downloads\Telegram Desktop\ChatExport_2025-04-29";
            var allFiles = Directory.GetFiles(path).ToList();
            var builder = new StringBuilder();
            int counter = 0;

            var allMess = new List<string>();

            foreach (var file in allFiles)
            {
                counter++;

                var html = File.ReadAllText(file);

                // Создание парсера
                var context = BrowsingContext.New(AngleSharp.Configuration.Default);
                var parser = context.GetService<IHtmlParser>();
                var document = parser.ParseDocument(html);

                // Поиск элементов
                var items = document.QuerySelectorAll(".body").ToList();

                foreach (var item in items)
                {
                    var parse = item.InnerHtml
                        .Replace("  ", "")
                        .Replace(@"""","'")
                        .Replace("\n", "");

                    var match = Regex.Match(parse, ".*title='(.*'>[0-9].*<br>[A-z]*_5.*[0-9]*\\.[0-9][0-9])").Groups[1].Value;
                    
                    if (!string.IsNullOrEmpty(match))
                    {
                        allMess.Add(match);
                    }
                }

                var fd = allMess;
            }
        }
    }
}
