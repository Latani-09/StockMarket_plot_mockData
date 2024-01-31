using Microsoft.AspNetCore.Identity;
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
                var filePath = "StockMarket.json";
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
                                entry[header[i]] = line[i];
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

    }
}

