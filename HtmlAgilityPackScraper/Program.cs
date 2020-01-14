using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.Data.SqlClient;

namespace HtmlAgilityPackScraper
{
    internal static class Program
     {
         static void Main(string[] args)
         {
             List<HtmlNode> cryptoData = GetData.GetHtml();

             ParseAndInject.InjectData(cryptoData);
         }
     }
}
