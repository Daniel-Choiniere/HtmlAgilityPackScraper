using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace HtmlAgilityPackScraper
{
     class Program
     {
         static void Main(string[] args)
         {
             GetHtmlAsync();
             Console.WriteLine("Hit any key to exit");
             Console.ReadLine();

         }

         private static async void GetHtmlAsync()
         {
             const string url = "http://www.coinmarketcap.com";
            
             HtmlWeb web = new HtmlWeb();  
             HtmlDocument document = web.Load(url);

             HtmlNode[] nodes = document.DocumentNode.SelectNodes("//tr").ToArray();  
             foreach (HtmlNode item in nodes)
             {
                 var splitData = item.InnerText.Split('$');
                 
                 var allAlphaSymbol = Regex.Replace(splitData[0], @"[\d-]", string.Empty);
                 
                 Console.WriteLine(allAlphaSymbol);
                 
                 foreach (var word in splitData)
                    Console.WriteLine(word);
             }
         }
     }
}
