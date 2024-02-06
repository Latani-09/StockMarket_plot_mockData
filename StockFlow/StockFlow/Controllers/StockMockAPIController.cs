using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockFlow.Data;

namespace StockFlow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockMockAPIController : ControllerBase
    {
 
        List<stockData> data_last = new List<stockData>(); 

        public StockMockAPIController( )
        {
  
        }


        [HttpGet("close-price")]
        public async Task<ActionResult<List<stockData>>> Get([FromQuery] string company)
        {
            // Create a Random object
            Random random = new Random();
            // Specify the range (minValue, maxValue)
            int minValue = 940;
            int maxValue = 980;
            DateTime now = DateTime.Now;


            List<stockData> data_last_10min = new List<stockData>();
            List<stockData> data_last_2min = new List<stockData>();
            if (data_last.Count > 2)
            {
                data_last_2min = data_last.Skip(Math.Max(0, data_last.Count - 1)).ToList();
            }
            else
            {


                // Subtract one minute
                DateTime oneMinuteBefore = now.AddMinutes(-1);
                var data_old = new stockData
                {
                    Time = oneMinuteBefore,
                    Date = oneMinuteBefore,
                    Close = random.Next(minValue, maxValue + 1)
                };
                data_last_2min.Add(data_old);
                data_last.Add(data_old);


            }

            var data = new stockData
            {
                Time = now,
                Date = now,
                Close = random.Next(minValue, maxValue + 1)
            };
            data_last_2min.Add(data);
            data_last.Add(data);





            //Console.Write(Count);
            //Console.Write("stockdata", dataObject.ToString());

            var result = RetrieveDataWithinLast10MinutesAsync(company);

            if (result != null)
            {
                Console.WriteLine("Data within the last 1 minutes:");
                data_last_2min = result;
                Console.WriteLine(result[0].Close);
            }
            else
            {
                Console.WriteLine("No data available within the last 10 minutes.");
            }

            Console.WriteLine(data_last_2min);
            return data_last_2min;

            static List<stockData?> RetrieveDataWithinLast10MinutesAsync(string company)
            {
                string jsonFilePath = $"stock_details/{company}.json";
                string DataJson = System.IO.File.ReadAllText(jsonFilePath);
                
                List<stockData> dataObjects = JsonConvert.DeserializeObject<List<stockData>>(DataJson);

                List<stockData> data_last_2min = new List<stockData>();
                var i = 0;
                while (i < dataObjects.Count)
                {
                   // Console.Write("stockdata" + dataObjects[i].Close);
                    DateTime timestamp = DateTime.Parse($"{dataObjects[i].Date.ToShortDateString()} {dataObjects[i].Time.TimeOfDay}");
                   // Console.WriteLine("timestamp" + timestamp);
                    TimeSpan timeDifference = DateTime.Now - timestamp;
                  //  Console.WriteLine("time difference " + timeDifference.ToString());

                    if (timeDifference <= TimeSpan.FromMinutes(1) & timeDifference >= TimeSpan.FromMinutes(0))
                    {

                        var stock_last = new stockData
                        {
                            Time = DateTime.Parse($"{dataObjects[i].Time}"),
                            Date = DateTime.Parse($"{dataObjects[i].Date}"),
                            Close = dataObjects[i].Close
                            // Add other relevant data you want to retrieve
                        };
                        data_last_2min.Add(stock_last);
                    }
                    i += 1;
                }
                return data_last_2min;
            }
        }

        [HttpGet("trend")]
        public ActionResult<List<stockData>> GetTrend()
        {
            // Assuming you have a list of company symbols
            List<string> companySymbols = new List<string> { "ABC", "XYZ", "J123", "K456", "M789" };

            // Create a Random object
            Random random = new Random();
            int minValue = 940;
            int maxValue = 980;

            List<stockData> trendData = new List<stockData>();

            foreach (var symbol in companySymbols)
            {
                var company = new stockData
                {
                    Symbol = symbol,
                    Close = random.Next(minValue, maxValue + 1)
                };

                trendData.Add(company);
            }

            return trendData;
        }




    }
}



