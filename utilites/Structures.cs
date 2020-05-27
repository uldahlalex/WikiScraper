using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiScraper.utilites
{
    /*
     * Class structures contains utilitarian methods and variables utilized by other components
     */
    public class Structures
    {
        public Dictionary<string, int> frequencies(string text)
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
