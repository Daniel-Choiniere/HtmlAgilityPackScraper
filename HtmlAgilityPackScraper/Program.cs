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

             List<HtmlNode> nodes = document.DocumentNode.SelectNodes("//tr").ToList();
             nodes.RemoveRange(0, 3);

             InjectData(nodes);
         }

         private static void InjectData(List<HtmlNode> nodes)
         {
             string connection =
                @"Server=tcp:finance-scraper.database.windows.net,1433;Initial Catalog=CoinMarketCap;Persist Security Info=False;User ID=Dan;Password=iLOVEcareerdevs1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                foreach (HtmlNode item in nodes)
                {
                    string[] splitData = item.InnerText.Split('$');

                    string currency = Regex.Replace(splitData[0], @"[\d-]", string.Empty);
                    string marketCap = "$" + splitData[1];
                    string price = "$" + splitData[2];
                 
                    string[] splitFurtherData = splitData[3].Split(" ");
                    string volume = "$" + splitFurtherData[0];
                 
                    string priceChange = Regex.Replace(splitFurtherData[1], "[^0-9.%]", "");
                    
                    SqlCommand insertCommand = new SqlCommand(
                        "INSERT into dbo.CryptoData (DateTimeScraped, Currency, MarketCap, Price, Volume, PriceChange) VALUES (@dateTime, @currency, @marketCap, @price, @volume, @priceChange)",
                        dbConnection);
                    insertCommand.Parameters.AddWithValue("@dateTime", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@currency", currency);
                    insertCommand.Parameters.AddWithValue("@marketCap", marketCap);
                    insertCommand.Parameters.AddWithValue("@price", price);
                    insertCommand.Parameters.AddWithValue("@volume", volume);
                    insertCommand.Parameters.AddWithValue("@priceChange", priceChange);

                    insertCommand.ExecuteNonQuery();
                }
                
                Console.WriteLine("Data Collection Successful");
                dbConnection.Close();
            }
         }
     }
}
