using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Newtonsoft.Json;
using File = System.IO.File;

namespace RomashkaParser
{
    public class Exchange
    {
        private static int clsd = 0;
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

        public static void Processing(List<Signal> signalList, int candlesCount, decimal offsetMinimal, decimal riskRatio, int takeProfit)
        {
            var pathResults = Environment.CurrentDirectory + @$"\results_{DateTime.Now:T}.csv".Replace(":", "_");
            File.WriteAllText(pathResults, @"Time;Side;PNL;Bars;SL TP" + Environment.NewLine);
            var path = Environment.CurrentDirectory + @"\dates.json";
            if (File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                _dates = JsonConvert.DeserializeObject<List<Kline>>(jsonString) ?? new List<Kline>();
            }

            foreach (var signal in signalList)
            {
                if (signal.Price < 8999) continue;

                if (clsd == 2682)
                {
                    var dd = new DateTime();
                }
                var openPrice = signal.Price;
                var side = signal.Side;
                var time = signal.Time;
                var stopLoss = CalculateStopLoss(openPrice, candlesCount, offsetMinimal, side, time);

                if (stopLoss < 0 ) continue;

                var takeProfitPrice = CalculateTakeProfit(openPrice, stopLoss, riskRatio, side);
                var stopLossPrice = 0m;
                switch (side)
                {
                    case "LONG": stopLossPrice = openPrice - stopLoss; break;
                    case "SHORT": stopLossPrice = openPrice + stopLoss; break;
                }

                var isOpen = true;
                var count = 0;
                while (isOpen)
                {
                    var checkBarTime = time.AddMinutes(5 * count);
                    var response = CheckSignal(checkBarTime,openPrice, stopLossPrice, takeProfitPrice, side);
                    isOpen = response.isOpen;
                    if (!isOpen)
                    {
                        CloseSignal(signal, response.message, pathResults, count);
                        break;
                    }
                    count++;
                }
            }
        }

        private static void CloseSignal(Signal signal, string message, string path, int count)
        {
            if (message.Contains("Take Profit"))
            {
                var pnl = message.Replace("Take Profit ", "");
                File.AppendAllText(path, @$"{signal.Time};{signal.Side};{pnl:0.00};{count};Take Profit" + Environment.NewLine);
            }
            else
            {
                var pnl = message.Replace("Stop Loss ", "");
                File.AppendAllText(path, @$"{signal.Time};{signal.Side};{pnl:0.00};{count};Stop Loss" + Environment.NewLine);
            }

            clsd++;
        }

        private static (bool isOpen, string message) CheckSignal(DateTime checkBarTime, decimal openPrice, decimal stopLossPrice, decimal takeProfitPrice, string side)
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
                                                 && x.OpenTime.Day == checkBarTime.Day 
                                                 && x.OpenTime.Hour == checkBarTime.Hour
                                                 && x.OpenTime.Minute == checkBarTime.Minute);

            if (bar != null)
            {
                switch (side)
                {
                    case "LONG":
                        if (bar.ClosePrice >= (double)takeProfitPrice) return (false, $"Take Profit {takeProfitPrice - openPrice}");
                        if (bar.ClosePrice <= (double)stopLossPrice) return (false, $"Stop Loss {stopLossPrice - openPrice}");
                        break;


                    case "SHORT":
                        if (bar.ClosePrice <= (double)takeProfitPrice) return (false, $"Take Profit {openPrice - takeProfitPrice}");
                        if (bar.ClosePrice >= (double)stopLossPrice) return (false, $"Stop Loss {openPrice - stopLossPrice}");
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
                        response = Math.Abs(openPrice - minPrice) + offsetMinimal;
                        //        200 == 90000 - 89900 + 100
                        break;
                    }
                    return -1;
                case "SHORT":
                    var maxPrice = GetMaximumPriceForCandles(time, candlesCount); 
                    if (maxPrice > 0)
                    {
                        response = Math.Abs(openPrice - maxPrice) + offsetMinimal;
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
