using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace HtmlAgilityPackScraper
{
  class Program
    {
        static void Main(string[] args)
        {
            WebDataScrap();
        }
 
        public static void WebDataScrap()
        {
            try
            {
                const string url = "http://www.coinmarketcap.com";
                
                
                // var web = new HtmlWeb();
                // var doc = web.Load(url);
                //
                // //Get the content from a file
                // //var path = "countries.html";
                // //var doc = new HtmlDocument();
                // //doc.Load(path);
                //
                // //Filter the content
                // doc.DocumentNode.Descendants()
                //     
                //                 .Where(n => n.Name == "script")
                //                 .ToList()
                //                 .ForEach(n => n.Remove());
                //
                // const string classValue = "border1";
                // var nodes = doc.DocumentNode.SelectNodes($"//*[@class='{classValue}']") ?? Enumerable.Empty<HtmlNode>();
                //
                // //Write the desired content to a file
                // using (var file = new StreamWriter("test.txt"))
                // {
                //     foreach (var node in nodes)
                //     {
                //         //Get the country name
                //         var splittedWords = Regex.Split(node.InnerText, "\n");
                //         var words = splittedWords
                //             .Where(x => !x.Contains("&nbsp;") && !string.IsNullOrEmpty(x.Trim()))
                //             .ToList();
                //
                //         if (words.Count() != 4) continue;
                //
                //         var countryName = words[0].Trim();
                //         var countryCode = words[2].Trim();
                //         var result = $"{countryName};{countryCode}";
                //
                //         file.WriteLine(result);
                //         Console.WriteLine(result);
                //     }
                // }
                //
                // Console.WriteLine("\r\nPlease press a key...");
                // Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured:\r\n{ex.Message}");
            }
        }
    }
}
