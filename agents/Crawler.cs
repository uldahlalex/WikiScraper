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
    /// <summary>
    /// The crawler represents each "autonomous agent" in the system
    /// The main goal of each crawler is to fetch data to the manager from a given URL
    /// </summary>
    class Crawler
    {

        private ScraperAgentManager manager;

        public Crawler(ScraperAgentManager manager)
        {
            this.manager = manager;
        }

        public void helloWorld()
        {
            Console.WriteLine("helloworldhowueflj");
        }

        public void Start(CancellationToken token, Link link, decimal articleLimit)
        {
            
            UriBuilder ub = new UriBuilder(link.URL);

            HtmlWeb hweb = new HtmlWeb();
            hweb.UserAgent = "uldahlalex";
            HtmlDocument doc = hweb.Load(ub.Uri.ToString());

            foreach (HtmlNode linkHere in doc.DocumentNode.SelectNodes("//a[@href]") )
            {
                if (manager.getNumberOfArticles() < (int)articleLimit)
                {
                    HtmlAttribute att = linkHere.Attributes["href"];
                    if ((att.Value.Contains("http://") || att.Value.Contains("https://")) && att.Value.Contains("en.wikikedia")) 
                    {
                        this.manager.addLink(new Link { URL = att.Value, visited = false });
                        manager.incrementArticles();
                    }
                    else if (att.Value.Contains("/wiki/")) //interne wiki links
                    {
                        this.manager.addLink(new Link { URL = "https://en.wikipedia.org"+att.Value, visited = false });
                        manager.incrementArticles();
                    }
                }
            }

            //DeEntitize replaces known entities by characters
            var exp = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);

            //Regularexpressions are used for tasks such as eliminating excess whitespace and special characters
            var expFiltered = Regex.Replace(exp, @"[^\w\d\s]", "");
            Regex space = new Regex(@"\s+");
            var sentences = exp.Replace(",\r\n", ", ").Split(new String[]
            {
                    "\r\n"
            }, StringSplitOptions.RemoveEmptyEntries);
            string text = string.Join("\r\n", sentences.Select(s => space.Replace(s, " ").Trim()));
            
            //All words should be lower case, such that two identical words dont register as different
            text = text.ToLower();

            manager.addText(text);
            
            link.visited=true;

        }




    }
}