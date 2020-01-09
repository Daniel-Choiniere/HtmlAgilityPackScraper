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
                
                    var chunked = ChunkIt(nodes, 9);

                    for (var i = 0; i < chunked.Count; i++)
                    {
                        string currency = chunked[i][1].InnerText;
                        string marketCap = chunked[i][2].InnerText;
                        string price = chunked[i][3].InnerText;
                        string volume = chunked[i][4].InnerText;
                        string circulatingSupply = chunked[i][5].InnerText;
                        string priceChange = chunked[i][6].InnerText;
                        
                        // Console.WriteLine(chunked[i][1].InnerText);
                        // Console.WriteLine(chunked[i][2].InnerText);
                        // Console.WriteLine(chunked[i][3].InnerText);
                        // Console.WriteLine(chunked[i][4].InnerText);
                        // Console.WriteLine(chunked[i][5].InnerText);
                        // Console.WriteLine(chunked[i][6].InnerText);
                        
                        SqlCommand insertCommand = new SqlCommand(
                            "INSERT into dbo.CryptoData (DateTimeScraped, Currency, MarketCap, Price, Volume, circulatingSupply, PriceChange) VALUES (@dateTime, @currency, @marketCap, @price, @volume, @circulatingSupply, @priceChange)",
                            dbConnection);
                        insertCommand.Parameters.AddWithValue("@dateTime", DateTime.Now);
                        insertCommand.Parameters.AddWithValue("@currency", currency);
                        insertCommand.Parameters.AddWithValue("@marketCap", marketCap);
                        insertCommand.Parameters.AddWithValue("@price", price);
                        insertCommand.Parameters.AddWithValue("@volume", volume);
                        insertCommand.Parameters.AddWithValue("@circulatingSupply", circulatingSupply);
                        insertCommand.Parameters.AddWithValue("@priceChange", priceChange);
                        
                        insertCommand.ExecuteNonQuery();
                    }
                    
                    Console.WriteLine("Data Collection Successful");
                    dbConnection.Close();
            }
         }
     }
}
