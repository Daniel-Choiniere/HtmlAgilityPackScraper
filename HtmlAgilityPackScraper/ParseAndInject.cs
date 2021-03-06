using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.Data.SqlClient;

namespace HtmlAgilityPackScraper
{
    public class ParseAndInject
    {
        private static List<List<T>> ChunkIt<T>(List<T> nodes, int size)
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
         
         public static void InjectData(List<HtmlNode> nodes)
         {
             string connection =
                @"Server=tcp:finance-scraper.database.windows.net,1433;Initial Catalog=CoinMarketCap;Persist Security Info=False;User ID=Dan;Password=iLOVEcareerdevs1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

             using SqlConnection dbConnection = new SqlConnection(connection);
             dbConnection.Open();
                
             var chunked = ChunkIt(nodes, 9);

             foreach (var data in chunked)
             {
                 Crypto cryptoData = new Crypto
                 {
                     currency = data[1].InnerText,
                     marketCap = data[2].InnerText,
                     price = data[3].InnerText,
                     volume = data[4].InnerText,
                     circulatingSupply = data[5].InnerText,
                     priceChange = data[6].InnerText
                 };

                 SqlCommand insertCommand = new SqlCommand(
                     "INSERT into dbo.CryptoData (DateTimeScraped, Currency, MarketCap, Price, Volume, circulatingSupply, PriceChange) VALUES (@dateTime, @currency, @marketCap, @price, @volume, @circulatingSupply, @priceChange)",
                     dbConnection);
                 insertCommand.Parameters.AddWithValue("@dateTime", DateTime.Now);
                 insertCommand.Parameters.AddWithValue("@currency", cryptoData.currency);
                 insertCommand.Parameters.AddWithValue("@marketCap", cryptoData.marketCap);
                 insertCommand.Parameters.AddWithValue("@price", cryptoData.price);
                 insertCommand.Parameters.AddWithValue("@volume", cryptoData.volume);
                 insertCommand.Parameters.AddWithValue("@circulatingSupply", cryptoData.circulatingSupply);
                 insertCommand.Parameters.AddWithValue("@priceChange", cryptoData.priceChange);
                        
                 insertCommand.ExecuteNonQuery();
             }
                    
             Console.WriteLine("Data Collection Successful");
             dbConnection.Close();
         }
    }
}