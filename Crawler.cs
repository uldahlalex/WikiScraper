using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WikiScraper
{
    class Crawler
    {
        private Boolean cont;

        public void Start(CancellationToken token, String searchTerm)
        {
            cont = true;

            while (cont)
            {
                UriBuilder ub = new UriBuilder(searchTerm);


                HtmlWeb hweb = new HtmlWeb();
                HtmlDocument doc = hweb.Load(ub.Uri.ToString());

                var exp = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);
                Regex r = new Regex(@"\s+");
                var sentences = exp.Replace(",\r\n", ", ").Split(new String[]
                {
                    "\r\n"
                }, StringSplitOptions.RemoveEmptyEntries);
                string text = string.Join("\r\n", sentences.Select(s => r.Replace(s, " ").Trim()));
                Console.WriteLine(text);

                //Brug dictionary til at lagre alle ordene - mest effektive runtime, og det er mest hensigtsmæssigt
                //at bruge en datastruktur som har unikke "keys" og values der stiger efter frekvens.
                //Dette burde egentlig også bruges i datagen projektet til at lave frekvenslisten

                //cancellation of while loop
                cont = false;
                var dict = this.frequencies(text);
                foreach (var item in dict)
                {
                    Console.WriteLine(item.Key);
                    Console.WriteLine(item.Value);
                }
                
            }
        }

        private Dictionary<string, int> frequencies(string text)
        {
            Dictionary<string, int> count =
                text.Split(' ')
                    .GroupBy(s => s)
                    .ToDictionary(g => g.Key, g => g.Count());
            return count;
        }
    }
}