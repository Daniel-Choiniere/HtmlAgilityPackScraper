using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace HtmlAgilityPackScraper
{
    public class GetData
    {
        public static List<HtmlNode> GetHtml()
        {
            const string url = "http://www.coinmarketcap.com";
            
            HtmlWeb web = new HtmlWeb();  
            HtmlDocument document = web.Load(url);

            List<HtmlNode> nodes = document.DocumentNode.SelectNodes("//td").ToList();
            
            return nodes;
        }
    }
}