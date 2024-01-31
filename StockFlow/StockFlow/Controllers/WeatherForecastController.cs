using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;


using Newtonsoft.Json;
using System.Formats.Asn1;
using CsvHelper;
using System.Security.Cryptography.Xml;
using StockFlow.Data;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace StockFlow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
         };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]


        [HttpGet]
        public async Task<ActionResult<List<stockData>>> Get()
        {
            string jsonFilePath = "StockMarket.json";
            string DataJson = System.IO.File.ReadAllText(jsonFilePath);
            List<stockData> dataObjects = JsonConvert.DeserializeObject<List<stockData>>(DataJson);
            List<stockData> data_last_10min=new List<stockData> ();
            var Count = 0;
            foreach (var dataObject in dataObjects)
                {
                Count += 1;
                Console.Write(Count);
                Console.Write("stockdata", dataObject.ToString());

                var result = await RetrieveDataWithinLast10MinutesAsync(dataObject);

                    if (result != null)
                    {
                        Console.WriteLine("Data within the last 10 minutes:");
                    data_last_10min.Add(result);
                        Console.WriteLine(result.Close);
                    }
                    else
                    {
                        Console.WriteLine("No data available within the last 10 minutes.");
                    }
                }
            return data_last_10min;
        }
            static async Task<stockData?> RetrieveDataWithinLast10MinutesAsync(stockData dataObject)
            {
            Console.Write("stockdata"+dataObject.Close);
                DateTime timestamp = DateTime.Parse($"{dataObject.Date.ToShortDateString()} {dataObject.Time.TimeOfDay}");
            Console.WriteLine("timestamp"+timestamp);
            TimeSpan timeDifference = DateTime.Now - timestamp;
            Console.WriteLine("time difference "+ timeDifference.ToString());

            if (timeDifference <= TimeSpan.FromMinutes(10))
                {
                    return new stockData
                    {
                        Time = DateTime.Parse($"{dataObject.Time}"),
                        Date = DateTime.Parse($"{dataObject.Date}"),
                        Close = dataObject.Close
                        // Add other relevant data you want to retrieve
                    };
                }
                else
                {
                    return null;
                }
            }




   
        }
    }



