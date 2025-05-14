using AngleSharp.Html.Parser;
using AngleSharp;
using System.Configuration;
using System.Formats.Tar;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Binance.Net.Enums;
using Newtonsoft.Json;

namespace RomashkaParser
{
    public partial class Form1 : Form
    {
        private static List<Signal> signalList = new List<Signal>();
        private static bool _longs = true;
        private static bool _shorts = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = $"Количество сигналов: {signalList.Count}";
            checkBoxLongs.Checked = _longs;
            checkBoxShorts.Checked = _shorts;
            checkBoxShorts.Enabled = false;
        }
        public DateTime ParseCustomDateTime(string dateString)
        {
            // Удаляем лишнюю часть с временной зоной и повторным временем
            int utcIndex = dateString.IndexOf("UTC");
            if (utcIndex >= 0)
            {
                dateString = dateString.Substring(0, utcIndex).Trim();
            }

            // Формат для парсинга
            string format = "dd.MM.yyyy HH:mm:ss";

            // Парсим дату с учетом инвариантной культуры
            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            throw new FormatException($"Не удалось распознать дату из строки: {dateString}");
        }

        public decimal ParsePrice(string dateString)
        {
            if (decimal.TryParse(dateString.Replace(".", ","), out var result))
            {
                return result;
            }

            return -1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var path = @"C:\Users\Poma\Downloads\Telegram Desktop\ChatExport_2025-04-29";
            var allFiles = Directory.GetFiles(path).ToList();
            var builder = new StringBuilder();
            int counter = 0;

            var allMess = new List<Signal>();

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
                        .Replace(@"""", "'")
                        .Replace("\n", "");

                    var matches = Regex.Matches(parse, ".*title='(.*'>[0-9].*)<br>([A-z]*)_5.*([0-9][0-9][0-9][0-9]\\.[0-9][0-9])");

                    foreach (Match match in matches)
                    {
                        var sig = new Signal
                        {
                            Name = "ETHUSDT",
                            Time = ParseCustomDateTime(Regex.Match(match.Groups[1].Value, "(.*[0-9][0-9]:[0-9][0-9])").Groups[1].Value.Replace("'>", " ")),
                            Side = match.Groups[2].Value,
                            Price = ParsePrice(match.Groups[3].Value)
                        };

                        allMess.Add(sig);

                    }
                }
            }

            var jsonString = JsonConvert.SerializeObject(allMess);
            File.WriteAllText(Environment.CurrentDirectory + "/eth_sigs_5.json", jsonString);

        }
        private void buttonDates_Click(object sender, EventArgs e)
        {
            var dateStart = signalList.MinBy(x => x.Time)?.Time;
            var dateEnd = signalList.MaxBy(x => x.Time)?.Time;

            if (dateStart != null && dateEnd != null)
            {
                //Exchange.SaveDates((DateTime)dateStart, (DateTime)dateEnd, signalList[0].Name, KlineInterval.FiveMinutes);    
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {


        }
        private void buttonBtcSigs_Click(object sender, EventArgs e)
        {
            var path = Environment.CurrentDirectory + "/btc_sigs_5.json";
            if (File.Exists(path))
            {
                var strings = File.ReadAllText(path);
                var sigs = JsonConvert.DeserializeObject<List<Signal>>(strings);

                if (sigs != null)
                {

                    var sortedSignals = Exchange.SortedSignals(sigs);
                    signalList = sortedSignals;
                    var jsonString = JsonConvert.SerializeObject(sortedSignals);
                    File.WriteAllText(Environment.CurrentDirectory + "/btc_sigs_5_sorted.json", jsonString);
                    label1.Text = $"Количество сигналов: {signalList.Count}";
                    buttonBtcSigs.BackColor = Color.ForestGreen;
                    buttonEthSigs.BackColor = Color.Gray;
                }
            }
        }
        private void buttonEthSigs_Click(object sender, EventArgs e)
        {
            var path = Environment.CurrentDirectory + "/eth_sigs_5.json";
            if (File.Exists(path))
            {
                var strings = File.ReadAllText(path);
                signalList = JsonConvert.DeserializeObject<List<Signal>>(strings);
                label1.Text = $"Количество сигналов: {signalList.Count}";
                buttonEthSigs.BackColor = Color.ForestGreen;
                buttonBtcSigs.BackColor = Color.Gray;
            }
        }

        //  


        private async void buttonStart_Click(object sender, EventArgs e)
        {
            var candlesCount = 0;
            var offsetMinimal = .0m;
            var riskRatio = .0m;
            var risk = 0;

            if (!string.IsNullOrEmpty(textBoxCandels.Text)) if (int.TryParse(textBoxCandels.Text, out var countCandelsResult)) candlesCount = countCandelsResult; else return;
            if (!string.IsNullOrEmpty(textBoxOffsetMinimal.Text)) if (decimal.TryParse(textBoxOffsetMinimal.Text.Replace(".", ","), out var offsetMinimalResult)) offsetMinimal = offsetMinimalResult; else return;
            if (!string.IsNullOrEmpty(textBoxRR.Text)) if (decimal.TryParse(textBoxRR.Text.Replace(".", ","), out var rrResult)) riskRatio = rrResult; else return;
            if (!string.IsNullOrEmpty(textBoxRisk.Text)) if (int.TryParse(textBoxRisk.Text, out var takeResult)) risk = takeResult; else return;

            await Exchange.ProcessingParallel(signalList, candlesCount, offsetMinimal, riskRatio, risk, progressBar1, 100000, _longs, _shorts);
        }

        private void textBoxCandels_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxLongs_CheckedChanged(object sender, EventArgs e)
        {
            _longs = checkBoxLongs.Checked;
        }

        private void checkBoxShorts_CheckedChanged(object sender, EventArgs e)
        {
            _shorts = checkBoxShorts.Checked;
        }

        private void textBoxOffsetMinimal_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class Signal
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string Side { get; set; }
        public decimal Price { get; set; }

    }
}
