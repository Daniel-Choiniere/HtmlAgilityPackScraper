using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.Data.SqlClient;

namespace HtmlAgilityPackScraper
{
     class Program
     {
         static void Main(string[] args)
         {
             GetHtml();
         }
         
         private static void GetHtml()
         {
             const string url = "http://www.coinmarketcap.com";
            
             HtmlWeb web = new HtmlWeb();  
             HtmlDocument document = web.Load(url);

             List<HtmlNode> nodes = document.DocumentNode.SelectNodes("//td").ToList();
             
             // foreach (var node in nodes)
             // {
             //     Console.WriteLine(node.InnerText);
             // }
             var chunked = ChunkIt(nodes, 5);

             for (var i = 0; i < chunked.Count; i++)
             {
                 Console.WriteLine(chunked[i][i].InnerText);
             }
             
             // foreach (var chunk in chunked)
             // {
             //     foreach (var subChunk in chunk)
             //     {
             //         Console.WriteLine(subChunk.InnerText);
             //     }
             // }
             
             InjectData(nodes);
         }
         
         
         public static List<List<T>> ChunkIt<T>(List<T> nodes, int size)
         {
             var chunks = new List<List<T>>();
             var count = 0;
             var temp = new List<T>();

             foreach (var element in nodes)
             {
                 if (count++ == size)
                 {
                     chunks.Add(temp);
                     temp = new List<T>();
                     count = 1;
                 }
                 temp.Add(element);
             }

             chunks.Add(temp);  
             return chunks;
         }
         
         
         private static void InjectData(List<HtmlNode> nodes)
         {
             string connection =
                @"Server=tcp:finance-scraper.database.windows.net,1433;Initial Catalog=CoinMarketCap;Persist Security Info=False;User ID=Dan;Password=iLOVEcareerdevs1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();
                
                foreach (HtmlNode node in nodes)
                {
                    // Console.WriteLine(node.InnerText);
                    
                    // string[] splitData = node.InnerText.Split('$');
                    //
                    // string currency = Regex.Replace(splitData[0], @"[\d-]", string.Empty);
                    // string marketCap = "$" + splitData[1];
                    // string price = "$" + splitData[2];
                    // string volume = "0";    
                    //
                    // string[] splitFurtherData = splitData[3].Split(" ");
                    //
                    // string[] splitVolume = splitFurtherData[0].Split(",");
                    // foreach (string numberSet in splitVolume)
                    // {
                    //     if (numberSet.Length > 3)
                    //     {
                    //         string volumeLastNumbers = numberSet.Substring(0, 3);
                    //         string circulatingSupplyFirstNumbers = numberSet.Substring(3);
                    //         
                    //         // Console.WriteLine("LastNumber: " +  volumeLastNumbers);
                    //
                    //         volume = splitVolume[0] + "," + splitVolume[1] + "," + splitVolume[2] + "," +
                    //                         volumeLastNumbers;
                    //         
                    //         Console.WriteLine(volume);
                    //         // Console.WriteLine("Circulating Supply: " + circulatingSupplyFirstNumbers);
                    //     }
                    //
                    //     volume = "5";
                    // }
                    //
                    // string priceChange = Regex.Replace(splitFurtherData[1], "[^0-9.%]", "");
                    
                    // SqlCommand insertCommand = new SqlCommand(
                    //     "INSERT into dbo.CryptoData (DateTimeScraped, Currency, MarketCap, Price, Volume, PriceChange) VALUES (@dateTime, @currency, @marketCap, @price, @volume, @priceChange)",
                    //     dbConnection);
                    // insertCommand.Parameters.AddWithValue("@dateTime", DateTime.Now);
                    // insertCommand.Parameters.AddWithValue("@currency", currency);
                    // insertCommand.Parameters.AddWithValue("@marketCap", marketCap);
                    // insertCommand.Parameters.AddWithValue("@price", price);
                    // insertCommand.Parameters.AddWithValue("@volume", volume);
                    // insertCommand.Parameters.AddWithValue("@priceChange", priceChange);
                    //
                    // insertCommand.ExecuteNonQuery();
                }
                
                Console.WriteLine("Data Collection Successful");
                dbConnection.Close();
            }
         }
     }
}
