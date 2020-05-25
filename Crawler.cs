using HtmlAgilityPack;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WikiScraper.models;

namespace WikiScraper
{
    class Crawler
    {

        private ScraperAgentGUI scraper;
        //public List<Link> getLinksAfterCrawling;

        public Crawler(ScraperAgentGUI scraper)
        {
            this.scraper = scraper;
        }

        public void Start(CancellationToken token, Link link, decimal depth)
        {

            UriBuilder ub = new UriBuilder(link.URL);

            HtmlWeb hweb = new HtmlWeb();
            hweb.UserAgent = "uldahl";
            HtmlDocument doc = hweb.Load(ub.Uri.ToString());

            foreach (HtmlNode linkHere in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                if (scraper.iteration < (int)depth)
                {
                    HtmlAttribute att = linkHere.Attributes["href"];
                    if ((att.Value.Contains("http://") || att.Value.Contains("https://")) && att.Value.Contains("wiki")) //evt lav kun dansk wiki
                    {
                        this.scraper.links.Add(new Link { URL = att.Value, Depth = 1, visited = false });
                        scraper.iteration++;
                    }
                }

            }

            var exp = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);

            Regex r = new Regex(@"\s+");
            var sentences = exp.Replace(",\r\n", ", ").Split(new String[]
            {
                    "\r\n"
            }, StringSplitOptions.RemoveEmptyEntries);
            string text = string.Join("\r\n", sentences.Select(s => r.Replace(s, " ").Trim()));
            //Console.WriteLine(text);
            scraper.allText = scraper.allText + text.ToLower();
            link.visited=true;
            /*
            var dict = this.frequencies(text);
            foreach (var item in dict)
            {
                Console.WriteLine(item.Key);
                Console.WriteLine(item.Value);
            }*/
            //this.getLinksAfterCrawling = links;
            //return dict;
        }



        private Dictionary<string, int> frequencies(string text)
        {
            Dictionary<string, int> count =
                text.Split(' ')
                    .GroupBy(s => s)
                    .ToDictionary(g => g.Key, g => g.Count());
            var items = from item in count orderby item.Value descending select item;
            return items.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

    }
}