using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                //start the agent here
                HtmlWeb hweb = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = hweb.Load(ub.Uri.ToString());

                //ArrayList words = new ArrayList<String>();
                var text = doc.DocumentNode.InnerText;
                Console.WriteLine(text);
                cont = false;
                //cancellation finder pt aldrig sted, så hver crawler looper
            }

        }

    }
}
