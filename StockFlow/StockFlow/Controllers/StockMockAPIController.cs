using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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
            DateTime now = DateTime.Now;
            List<stockData> data_last_10min = new List<stockData>();


            var result = RetrieveDataWithinLast10MinutesAsync(company);

            if ((result != null)&(result.Count>0))
            {
                Console.WriteLine("Data within the last 10 minutes:");
                data_last_10min = result;
                Console.WriteLine(result[0].Close);

                return data_last_10min;
            }
            else
            {
                Console.WriteLine("No data available within the last 10 minutes.");
                return data_last_10min;
            }

            Console.WriteLine(data_last_10min);
            

            static List<stockData?> RetrieveDataWithinLast10MinutesAsync(string companySelected)
            {
                List<stockData> data_last_10min = new List<stockData>();

                    string jsonFilePath = $"stock_details/{companySelected}.json";
                    string DataJson = System.IO.File.ReadAllText(jsonFilePath);

                    List<stockData> dataObject = JsonConvert.DeserializeObject<List<stockData>>(DataJson);


                    var i = 0;
                    while (i < dataObject.Count)
                    {
                        // Console.Write("stockdata" + dataObjects[i].Close);
                        DateTime timestamp = DateTime.Parse($"{dataObject[i].Date.ToShortDateString()} {dataObject[i].Time.TimeOfDay}");
                        // Console.WriteLine("timestamp" + timestamp);
                        TimeSpan timeDifference = (DateTime.Now).ToLocalTime() - timestamp;
                        //  Console.WriteLine("time difference " + timeDifference.ToString());

                        if (timeDifference <= TimeSpan.FromMinutes(10) & timeDifference >= TimeSpan.FromMinutes(0))
                        {

                            var stock_last = new stockData
                            {
                                Symbol = companySelected,
                                Time = DateTime.Parse($"{dataObject[i].Time.ToLocalTime()}"),
                                Date = DateTime.Parse($"{dataObject[i].Date}"),
                                Close = dataObject[i].Close
                                // Add other relevant data you want to retrieve
                            };
                            data_last_10min.Add(stock_last);
                        }
                        i += 1;
                    }
                
                return data_last_10min;
            }
            return Ok(data_last_10min);

        }

        [HttpGet("trend")]
        public ActionResult<List<stockData>> GetTrend()
        {

            DateTime now = DateTime.Now;
            List<stockData> data_last_1min = new List<stockData>();


            var result = RetrieveDataWithinLastMinuteAsync();

            if( (result != null) & (result.Count > 0) )
            {
                Console.WriteLine("Data within the lastminute:");
                data_last_1min = result;
                Console.WriteLine(result[0].Close);
            }
            else
            {
                Console.WriteLine("No data available within the last minute.");
            }

            Console.WriteLine(data_last_1min);
            return data_last_1min;

            static List<stockData?> RetrieveDataWithinLastMinuteAsync()
            {
                List<stockData> data_last_min = new List<stockData>();
                List<string> companySymbols = new List<string> { "ABS", "ABX", "ACW", "XYZ" };
                foreach (var companysymbol in companySymbols)
                {
                    string jsonFilePath = $"stock_details/{companysymbol}.json";
                    string DataJson = System.IO.File.ReadAllText(jsonFilePath);

                    
                    
                    
                    List<stockData> dataObject = JsonConvert.DeserializeObject<List<stockData>>(DataJson);


                    var i = 0;
                    while (i < dataObject.Count)
                    {
                        // Console.Write("stockdata" + dataObjects[i].Close);
                        DateTime timestamp = DateTime.Parse($"{dataObject[i].Date.ToShortDateString()} {dataObject[i].Time.TimeOfDay}");
                        // Console.WriteLine("timestamp" + timestamp);
                        TimeSpan timeDifference = (DateTime.Now)- timestamp;
                        //  Console.WriteLine("time difference " + timeDifference.ToString());

                        if (timeDifference <= TimeSpan.FromMinutes(1) & timeDifference >= TimeSpan.FromMinutes(0))
                        {

                            var stock_last = new stockData
                            {
                                Symbol = companysymbol,
                                Time = DateTime.Parse($"{dataObject[i].Time}"),
                                Date = DateTime.Parse($"{dataObject[i].Date}"),
                                Close = dataObject[i].Close
                                // Add other relevant data you want to retrieve
                            };
                            data_last_min.Add(stock_last);
                        }
                        i += 1;
                    }
                }
                return data_last_min;
            }
            return Ok(data_last_1min);
            // Assuming you have a list of company symbols
           
        }




    }
}


/*
 
        [HttpGet("random-price")]
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
 */
