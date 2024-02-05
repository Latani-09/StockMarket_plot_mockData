using Microsoft.AspNetCore.Mvc;
using StockFlow.Data;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using MongoDB.Bson;

namespace StockFlow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<stockData>> Get()
        {


            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://apidojo-yahoo-finance-v1.p.rapidapi.com/stock/v3/get-chart?interval=1mo&symbol=AMRN&range=5y&region=US&includePrePost=false&useYfid=true&includeAdjustedClose=true&events=capitalGain%2Cdiv%2Csplit"),
                Headers =
    {
        { "X-RapidAPI-Key", "4a39c0e2d8mshffadcf17bdcebf7p10c3a7jsnf34ec18cbd02" },
        { "X-RapidAPI-Host", "apidojo-yahoo-finance-v1.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                Console.WriteLine(body);

                var chartData = JsonConvert.DeserializeObject<ChartData>(body);

                // Access values from the deserialized object
                string symbol = chartData.Chart.Result[0].Meta.Symbol;
                double regularMarketPrice = chartData.Chart.Result[0].Meta.RegularMarketPrice;
                List<int> timestamp = chartData.Chart.Result[0].Timestamp;
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(timestamp[0] / 1000d)).ToLocalTime();
                Console.WriteLine(dt);
                Console.WriteLine($"Symbol: {symbol}");
                Console.WriteLine($"Regular Market Price: {regularMarketPrice}");
                Console.WriteLine($"Timestamp:{timestamp}");
                Console.WriteLine($"Timestamp formatted:{dt}");
                var date = DateTimeOffset.FromUnixTimeSeconds((long)timestamp[0]).DateTime;
                Console.WriteLine(date);
                var result = GetTimestamps(timestamp);

                foreach (var time in timestamp)
                {
                    Console.WriteLine(timestamp);
                }

            }


            var client1 = new HttpClient();
            var request1 = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://query2.finance.yahoo.com/v7/finance/quote?symbols=TSLA&fields=regularMarketPreviousClose&region=US&lang=en-US"),
                Headers = { { "User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36" } }

            };
            using (var response = await client1.SendAsync(request1))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                Console.WriteLine(body);
            }



                return Ok();

        }

        // Deserialize JSON string into .NET object

    
    public static DateTime[] GetTimestamps(List<int> timestamps)
    {
            // Parse the JSON string

            // Convert timestamps to DateTime
            var result = new DateTime[timestamps.Count];
        for (int i = 0; i < timestamps.Count; i++)
        {
            result[i] = DateTimeOffset.FromUnixTimeSeconds((long)timestamps[i]).DateTime;
                Console.WriteLine(i + ": " + result[i]);
        }

        return result;
    }
    }

    // Define classes to represent the structure of the JSON
    public class ChartData
    {
        public Chart Chart { get; set; }
    }

    public class Chart
    {
        public List<Result> Result { get; set; }
    }

    public class Result
    {
        public Meta Meta { get; set; }
        public List<int> Timestamp { get; set; }
    }

    public class Meta
    {
        public string Symbol { get; set; }
        public double RegularMarketPrice { get; set; }
        // Add other properties as needed
    }
} 


