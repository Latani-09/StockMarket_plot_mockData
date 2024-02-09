using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Newtonsoft.Json;
using SharpCompress.Common;

namespace StockFlow.Data
{
    public class SeedData
    {
        public static async Task Seed()
        {
            var csvFilePath = "E:\\Users\\Dell\\LT.csv";
            int intervalInSeconds = 60; // Set your desired interval in seconds


            // Read data from CSV
            var csvData = ReadCsv(csvFilePath);
            var filePath = "ABS.json";
            // Convert each row to JSON and send
            foreach (var row in csvData)
            {
                string json = ConvertRowToJson(row);
                // Replace the following line with your logic to send the JSON object
                Console.WriteLine($"Sending JSON: {json}");
                File.AppendAllText(filePath, json + Environment.NewLine);

            }

            // Wait for the specified interval before the next iteration


            static List<Dictionary<string, string>> ReadCsv(string filePath)
            {
                var csvData = new List<Dictionary<string, string>>();

                using (var reader = new StreamReader(filePath))
                {
                    // Assuming the CSV has a header
                    var header = reader.ReadLine()?.Split(',');

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine()?.Split(',');

                        if (line != null)
                        {
                            var entry = new Dictionary<string, string>();

                            for (int i = 0; i < header.Length; i++)
                            {

                                if (header[i] == "Date")
                                {
                                    entry[header[i]] = " 6 - Feb - 24";
                                }
                                else { entry[header[i]] = line[i]; }
                            }

                            csvData.Add(entry);
                        }
                    }
                }

                return csvData;

            }
            static string ConvertRowToJson(dynamic row)
            {
                // Implement your logic to convert the CSV row to a JSON object
                // Example using Newtonsoft.Json:
                return Newtonsoft.Json.JsonConvert.SerializeObject(row);
            }
        }

        public static async Task Updatetimestamp()
        {

            List<stockData> data_last_min = new List<stockData>();
            List<string> companySymbols = new List<string> { "ABS", "ABX", "ACW", "XPP","IBN" };
            foreach (var companysymbol in companySymbols)
            {
                string jsonFilePath = $"stock_details/{companysymbol}.json";
                string DataJson = System.IO.File.ReadAllText(jsonFilePath);




                List<stockData> dataObject = JsonConvert.DeserializeObject<List<stockData>>(DataJson);
                var i = 0;
                // Get current date and time
                DateTime DateAndTime = DateTime.Now;

                DateTime date = DateAndTime.AddMinutes(-10);

                while (i < dataObject.Count)
                {

                    dataObject[i].Date = date.Date;
                    dataObject[i].Time = date.ToLocalTime();
                    i += 1;
                    Console.WriteLine(i);
                    date = date.AddMinutes(1);
                }
                Console.WriteLine(dataObject[0].Date);
                // Serialize the updated list back to JSON
                string updatedJson = JsonConvert.SerializeObject(dataObject, Formatting.Indented);

                // Write the updated JSON back to the file
                File.WriteAllText(jsonFilePath, updatedJson);
            }
        }
    }
}

