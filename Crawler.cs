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
                var text = string.Join("\r\n", sentences.Select(s => r.Replace(s, " ").Trim()));
                Console.WriteLine(text);


                //cancellation of while loop
                cont = false;

            }

        }

    }
}
