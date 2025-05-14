using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using File = System.IO.File;

namespace RomashkaParser
{
    public class Exchange
    {
        private static decimal _processed = .0m;
        private static double _position = 0;
        private static double _total = 0;
        public static string _resultsStrings = @"Time;Side;Open Price;Take Profit;Stop Loss;PNL;Volume;Bars;Balance;SL TP" + Environment.NewLine;
        private static List<Kline> _dates = new List<Kline>();

        public static void SaveDates(DateTime starDateTime, DateTime endDateTime, string symbol, KlineInterval interval)
        {
            var resp = new List<IBinanceKline>();

            var restClient = new BinanceRestClient();

            var maxLenght = (int)interval * 500; // seconds

            var span = endDateTime - starDateTime;

            var circle = (int)span.TotalSeconds / maxLenght;

            for (var i = 0; i <= circle; i++)
            {
                var result = restClient.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol, interval, starDateTime, endDateTime).Result;

                if (result.Success)
                {
                    var data = result.Data.ToList();
                    resp.AddRange(data);
                    starDateTime = starDateTime.AddSeconds(maxLenght);

                    var strJson = JsonConvert.SerializeObject(resp);
                    File.WriteAllText(Environment.CurrentDirectory + @"\dates.json", strJson);
                }
            }
        }
        public static List<Signal> SortedSignals(List<Signal> signals)
        {
            var sortedSigs = signals.OrderBy(x => x.Time).ToList();
            return sortedSigs;
        }
        private static async Task<List<Kline>> GetDates(string pathResults)
        {
            return await Task.Run(() =>
            {
                // Здесь ваша логика получения данных
                List<Kline> klines = new List<Kline>();

                var path = Environment.CurrentDirectory + @"\dates.json";
                if (File.Exists(path))
                {
                    var jsonString = File.ReadAllText(path);
                    klines = JsonConvert.DeserializeObject<List<Kline>>(jsonString) ?? new List<Kline>();
                }
                return klines;
            });
        }
        private static void CloseSignal(Signal signal, string message, string path, decimal volume, int count, decimal riskRatio, decimal risk, decimal openPrice, decimal takeProfit, decimal stopLoss)
        {
            if (message.Contains("Take Profit"))
            {
                _total += (double)(risk * riskRatio);
                //File.AppendAllText(path, @$"{signal.Time};{signal.Side};{openPrice};{takeProfit};{stopLoss};{risk * riskRatio:0.00};{volume};{count};{_total};Take Profit" + Environment.NewLine);
                _resultsStrings +=
                    @$"{signal.Time};{signal.Side};{openPrice};{takeProfit};{stopLoss};{risk * riskRatio:0.00};{volume};{count};{_total};Take Profit" +
                    Environment.NewLine;
            }
            else
            {
                _total -= (double)risk;
                //File.AppendAllText(path, @$"{signal.Time};{signal.Side};{openPrice};{takeProfit};{stopLoss};{-risk:0.00};{volume};{count};{_total};Stop Loss" + Environment.NewLine);
                _resultsStrings +=
                    @$"{signal.Time};{signal.Side};{openPrice};{takeProfit};{stopLoss};{-risk:0.00};{volume};{count};{_total};Stop Loss" +
                    Environment.NewLine;
            }
        }
        public static async Task Processing(List<Signal> signalList, int candlesCount, decimal offsetMinimal, decimal riskRatio, int risk, ProgressBar bar, double startBalance, bool longs = true, bool shorts = true)
        {
            var pathResults = Environment.CurrentDirectory + @$"\results_{DateTime.Now:T}.csv".Replace(":", "_");

            _total = startBalance;
            _position = _total / 100;
            _dates = await GetDates(pathResults);
            
            var currentSignals = new List<Signal>();

            if (!longs) currentSignals = signalList.Where(x => x.Side != "LONG").ToList();
            if (!shorts) currentSignals = signalList.Where(x => x.Side != "SHORT").ToList();
            
            await Task.Run(() =>
            {
                
                var stepSignal = 100.0m / signalList.Count;

                foreach (var signal in currentSignals)
                {
                    if (signal is { Price: < 8999, Name: "BTCUSDT" }) continue;
                    if (signal is { Price: > 8999, Name: "ETHUSDT" }) continue;

                    var stopLossPrice = 0m;
                    var openPrice = signal.Price;
                    var side = signal.Side;
                    var time = signal.Time;
                    var stopLoss = CalculateStopLoss(openPrice, candlesCount, offsetMinimal, side, time);

                    if (stopLoss < 0) continue;

                    var takeProfitPrice = CalculateTakeProfit(openPrice, stopLoss, riskRatio, side);
                    
                    switch (side)
                    {
                        case "LONG": stopLossPrice = openPrice - stopLoss; break;
                        case "SHORT": stopLossPrice = openPrice + stopLoss; break;
                    }

                    var volume = risk / (Math.Abs(stopLossPrice - openPrice) / openPrice);

                    SearchClose(signal, bar, time, openPrice, stopLossPrice, takeProfitPrice, side, pathResults, volume,
                        riskRatio, risk, stepSignal);
                    //var isOpen = true;
                    //var count = 0;
                   
                    //while (isOpen)
                    //{
                    //    if (count > 10000)
                    //    {
                    //        bar.Invoke((MethodInvoker)delegate
                    //        {
                    //            bar.Value = 100;
                    //            bar.Text = "Completed!";
                    //        });
                    //        MessageBox.Show("Completed!");
                    //        return;
                    //    }

                    //    var checkBarTime = time.AddMinutes(5 * count);
                    //    var response = CheckSignal(checkBarTime, openPrice, stopLossPrice, takeProfitPrice, side, count);
                    //    isOpen = response.isOpen;
                    //    if (!isOpen)
                    //    {

                    //        CloseSignal(signal, response.message, pathResults, volume, count, riskRatio, risk, openPrice, takeProfitPrice, stopLossPrice);
                    //        break;
                    //    }

                    //    count++;
                    //}


                }
            });
        } 


       

        private static async Task SearchClose(Signal signal, ProgressBar bar, DateTime time, decimal openPrice, decimal stopLossPrice, decimal takeProfitPrice, string side, string pathResults, decimal volume, decimal riskRatio, decimal risk, decimal stepSignal)
        {
            await Task.Run(() =>
            {
                var isOpen = true;
                var count = 0;

                while (isOpen)
                {
                    if (count > 10000) return;
                    
                    var checkBarTime = time.AddMinutes(5 * count);
                    var response = CheckSignal(checkBarTime, openPrice, stopLossPrice, takeProfitPrice, side, count);
                    isOpen = response.isOpen;
                    if (!isOpen)
                    {

                        CloseSignal(signal, response.message, pathResults, volume, count, riskRatio, risk, openPrice, takeProfitPrice, stopLossPrice);
                        break;
                    }

                    count++;
                }


                _processed += stepSignal;
                bar.Invoke((MethodInvoker)delegate { bar.Value = (int)_processed; });
            });
        }

        private static (bool isOpen, string message) CheckSignal(DateTime checkBarTime, decimal openPrice, decimal stopLossPrice, decimal takeProfitPrice, string side, int count)
        {
            
            var min = checkBarTime.Minute;

            if (min > 5 && min < 7)
            {
                var add = 10 - min;
                checkBarTime = checkBarTime.AddMinutes(add);
            }
            else if (min < 5 && min > 0)
            {
                checkBarTime = checkBarTime.AddMinutes(-min);
            }

            var bar = _dates.FirstOrDefault(x => x.OpenTime.Year == checkBarTime.Year
                                                 && x.OpenTime.Month == checkBarTime.Month
                                                 && x.OpenTime.Day == checkBarTime.Day 
                                                 && x.OpenTime.Hour == checkBarTime.Hour
                                                 && x.OpenTime.Minute == checkBarTime.Minute);

            if (bar != null)
            {
                switch (side)
                {
                    case "LONG":
                        if (bar.ClosePrice >= (double)takeProfitPrice)
                        {
                            if (count == 0)
                            {
                                var stop = "stop";
                            }
                            return (false, $"Take Profit {takeProfitPrice - openPrice}");
                        }
                        if (bar.ClosePrice <= (double)stopLossPrice)
                        {
                            if (count == 0)
                            {
                                var stop = "stop";
                            }
                            return (false, $"Stop Loss {stopLossPrice - openPrice}");
                        }
                        
                        break;


                    case "SHORT":
                        if (bar.ClosePrice <= (double)takeProfitPrice) return (false, $"Take Profit {openPrice - takeProfitPrice}");
                        if (bar.ClosePrice >= (double)stopLossPrice) return (false, $"Stop Loss {openPrice - stopLossPrice}"); 
                        if (count == 0)
                        {
                            var stop = "stop";
                        }
                        break;
                }
                return (true, "Not Found");
            }

            return (true, "Not Found");
        }

        private static decimal CalculateStopLoss(decimal openPrice, int candlesCount, decimal offsetMinimal, string side, DateTime time)
        {
            var response = 0m;

            switch(side)
            {
                case "LONG": 
                    var minPrice = GetMinimalPriceForCandles(time, candlesCount);
                    if (minPrice > 0)
                    {
                        response = Math.Abs(openPrice - minPrice) + minPrice * offsetMinimal;
                        //        200 == 90000 - 89900 + 100
                        break;
                    }
                    return -1;
                case "SHORT":
                    var maxPrice = GetMaximumPriceForCandles(time, candlesCount); 
                    if (maxPrice > 0)
                    {
                        response = Math.Abs(openPrice - maxPrice) + maxPrice * offsetMinimal;
                        //        200 == 90000 - 90100 + 100
                        break;
                    }
                    return -1;
            }

            return response;
        }

        private static decimal GetMaximumPriceForCandles(DateTime time, int candlesCount)
        {
            var minTime = time.AddMinutes(candlesCount * -5);
            var response = .0m;

            var listCandles = _dates
                .Where(x => x.OpenTime > minTime && x.OpenTime < time)
                .ToList();

            var max = listCandles.MaxBy(x => x.ClosePrice)?.ClosePrice;

            if (max != null) return (decimal)max;
            
            return response;
        }

        private static decimal GetMinimalPriceForCandles(DateTime time, int candlesCount)
        {
            var minTime = time.AddMinutes(candlesCount * -5);
            var response = .0m;

            var listCandles = _dates
                .Where(x => x.OpenTime > minTime && x.OpenTime < time)
                .ToList();

            var max = listCandles.MinBy(x => x.ClosePrice)?.ClosePrice;

            if (max != null) return (decimal)max;

            return response;
        }

        public static decimal CalculateTakeProfit(decimal openPrice, decimal stopLoss, decimal riskRatio, string side)
        {
            var response = .0m;

            switch (side)
            {
                case "LONG":
                    response = (openPrice - (openPrice - stopLoss)) * riskRatio + openPrice;
                    break;

                case "SHORT":
                    response = (openPrice - (openPrice + stopLoss)) * riskRatio + openPrice;
                    break;
            }

            return response;
        }



        public static async Task ProcessingParallel(List<Signal> signalList, int candlesCount, decimal offsetMinimal,
            decimal riskRatio, int risk, ProgressBar bar, int startBalance, bool longs, bool shorts)
        {
            var pathResults = Environment.CurrentDirectory + @$"\results_{DateTime.Now:T}.csv".Replace(":", "_");

            _total = startBalance;
            _position = _total / 100;
            _dates = await GetDates(pathResults);

            // Фильтрация сигналов
            var currentSignals = signalList.ToList(); // Копируем список
            if (!longs) currentSignals = currentSignals.Where(x => x.Side != "LONG").ToList();
            if (!shorts) currentSignals = currentSignals.Where(x => x.Side != "SHORT").ToList();

            var stepSignal = 100.0m / currentSignals.Count;
            var maxDegreeOfParallelism = Environment.ProcessorCount * 2; // Оптимальное число потоков
            var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);

            var tasks = currentSignals.Select(async signal =>
            {
                // Пропускаем не нужные сигналы
                if (signal is { Price: < 8999, Name: "BTCUSDT" }) return;
                if (signal is { Price: > 8999, Name: "ETHUSDT" }) return;

                await semaphore.WaitAsync();
                try
                {
                    var stopLossPrice = 0m;
                    var openPrice = signal.Price;
                    var side = signal.Side;
                    var time = signal.Time;
                    var stopLoss = CalculateStopLoss(openPrice, candlesCount, offsetMinimal, side, time);

                    if (stopLoss < 0) return;

                    var takeProfitPrice = CalculateTakeProfit(openPrice, stopLoss, riskRatio, side);

                    switch (side)
                    {
                        case "LONG":
                            stopLossPrice = openPrice - stopLoss;
                            break;
                        case "SHORT":
                            stopLossPrice = openPrice + stopLoss;
                            break;
                    }

                    var volume = risk / (Math.Abs(stopLossPrice - openPrice) / openPrice);

                    await SearchClose(signal, bar, time, openPrice, stopLossPrice,
                                    takeProfitPrice, side, pathResults, volume,
                                    riskRatio, risk, stepSignal);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
            MessageBox.Show("Completed!");
        }

        //public static async Task ProcessingParallel(List<Signal> signalList, int candlesCount, decimal offsetMinimal,
        //    decimal riskRatio, int risk, ProgressBar bar, int startBalance, bool longs, bool shorts)
        //{
        //    await Task.Run(() =>
        //    {
        //        var pathResults = Environment.CurrentDirectory + @$"\results_{DateTime.Now:T}.csv".Replace(":", "_");

        //        _total = startBalance;
        //        _position = _total / 100;
        //        _dates = GetDates(pathResults).Result;
        //        var currentSignals = new List<Signal>();


        //        if (!longs) currentSignals = signalList.Where(x => x.Side != "LONG").ToList();
        //        if (!shorts) currentSignals = signalList.Where(x => x.Side != "SHORT").ToList();

        //        var stepSignal = 100.0m / currentSignals.Count;

        //        Parallel.ForEach(currentSignals, signal =>
        //        {
        //            if (signal is { Price: < 8999, Name: "BTCUSDT" }) return;
        //            if (signal is { Price: > 8999, Name: "ETHUSDT" }) return;

        //            var stopLossPrice = 0m;
        //            var openPrice = signal.Price;
        //            var side = signal.Side;
        //            var time = signal.Time;
        //            var stopLoss = CalculateStopLoss(openPrice, candlesCount, offsetMinimal, side, time);

        //            if (stopLoss < 0) return;

        //            var takeProfitPrice = CalculateTakeProfit(openPrice, stopLoss, riskRatio, side);

        //            switch (side)
        //            {
        //                case "LONG": stopLossPrice = openPrice - stopLoss; break;
        //                case "SHORT": stopLossPrice = openPrice + stopLoss; break;
        //            }

        //            var volume = risk / (Math.Abs(stopLossPrice - openPrice) / openPrice);

        //            SearchClose(signal, bar, time, openPrice, stopLossPrice, takeProfitPrice, side, pathResults, volume,
        //                riskRatio, risk, stepSignal);
        //        });
        //        MessageBox.Show("Completed!");
        //    });

        //}
    }

    public class Kline
    {
        public double Volume { get; set; }
        public double QuoteVolume { get; set; }
        public double TakerBuyBaseVolume { get; set; }
        public double TakerBuyQuoteVolume { get; set; }
        public DateTime OpenTime { get; set; }
        public double OpenPrice { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double ClosePrice { get; set; }
        public DateTime CloseTime { get; set; }
        public int TradeCount { get; set; }
    }
}
