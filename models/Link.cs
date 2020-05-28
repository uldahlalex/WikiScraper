using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiScraper.models
{
    /// <summary>
    /// Each href fetched from a website will be stored as a Link object
    /// </summary>
    public class Link
    {
        public String URL { get; set; }
        public int Depth { get; set; }
        public bool visited { get; set; }
    }
}
