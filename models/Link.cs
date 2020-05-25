using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiScraper.models
{
    public class Link
    {
        public String URL { get; set; }
        public int Depth { get; set; }
        public bool visited { get; set; }
    }
}
