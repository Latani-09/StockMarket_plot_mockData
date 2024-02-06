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
                RequestUri = new Uri("https://eodhd.com/api/real-time/AAPL.US?api_token=65c1d2d0497201.35321167&fmt=json"),
                
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                Console.WriteLine(body);

                var stockData = JsonConvert.DeserializeObject<stock>(body);

                // Access values from the deserialized object
                string symbol = stockData.code;
         

                int timestamp = stockData.timestamp;
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(timestamp/ 1000d)).ToLocalTime();
                Console.WriteLine(dt);
                Console.WriteLine($"Symbol: {symbol}");
      
                Console.WriteLine($"Timestamp:{timestamp}");
                Console.WriteLine($"Timestamp formatted:{dt}");
                var date = DateTimeOffset.FromUnixTimeSeconds((long)timestamp).DateTime;
                Console.WriteLine(date);


            }
            /* var client1 = new HttpClient();
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

            */

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
    

} 


