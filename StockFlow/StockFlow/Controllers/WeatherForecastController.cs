using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockFlow.Data;
using System.Security.Cryptography;

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
        List<stockData> data_last = new List<stockData>();
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<List<stockData>>> Get()
        {
            // Create a Random object
            Random random = new Random();
            // Specify the range (minValue, maxValue)
            int minValue = 940;
            int maxValue = 980;
            DateTime now = DateTime.Now;
            string jsonFilePath = "StockMarket2.json";
            string DataJson = System.IO.File.ReadAllText(jsonFilePath);
            List<stockData> dataObjects = JsonConvert.DeserializeObject<List<stockData>>(DataJson);
            List<stockData> data_last_10min=new List<stockData> ();
            List<stockData> data_last_2min = new List<stockData>();
            if (data_last.Count >2)
            {
                data_last_2min = data_last.Skip(Math.Max(0, data_last.Count - 1)).ToList();
            }
            else
            {
            

                // Subtract one minute
                DateTime oneMinuteBefore = now.AddMinutes(-1);
                var data_old = new stockData
                {
                    Time= oneMinuteBefore,
                    Date = oneMinuteBefore,
                    Close = random.Next(minValue, maxValue + 1)
                };
                data_last_2min.Add(data_old);
                data_last.Add(data_old);


            }

            var data = new stockData
            {
                Time= now,
                Date = now,
                Close = random.Next(minValue, maxValue + 1)
            };
            data_last_2min.Add(data);
            data_last.Add(data); 

            

            
            var Count = 0;
            foreach (var dataObject in dataObjects)
                {
                Count += 1;
                //Console.Write(Count);
                //Console.Write("stockdata", dataObject.ToString());

                var result = await RetrieveDataWithinLast10MinutesAsync(dataObject);

                    if (result != null)
                    {
                        Console.WriteLine("Data within the last 1 minutes:");
                    data_last_10min.Add(result);
                        Console.WriteLine(result.Close);
                    }
                    else
                    {
                        Console.WriteLine("No data available within the last 10 minutes.");
                    }
                }
            Console.WriteLine(data_last_2min);
            return data_last_2min;
        }
            static async Task<stockData?> RetrieveDataWithinLast10MinutesAsync(stockData dataObject)
            {
            Console.Write("stockdata"+dataObject.Close);
                DateTime timestamp = DateTime.Parse($"{dataObject.Date.ToShortDateString()} {dataObject.Time.TimeOfDay}");
            Console.WriteLine("timestamp"+timestamp);
            TimeSpan timeDifference = DateTime.Now - timestamp;
            Console.WriteLine("time difference "+ timeDifference.ToString());

            if (timeDifference <= TimeSpan.FromMinutes(1) & timeDifference >= TimeSpan.FromMinutes(0) )
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



