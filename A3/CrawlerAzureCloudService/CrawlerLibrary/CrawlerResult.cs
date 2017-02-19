using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLibrary
{
    public class CrawlerResult
    {
        public string Item1;
        public int Item2;
        public string Item3;
        public string Item4;

        public CrawlerResult(Tuple<string, int, string, string> input)
        {
            Item1 = input.Item1;
            Item2 = input.Item2;
            Item3 = input.Item3;
            Item4 = input.Item4;
        }

        public CrawlerResult() { }
    }
}
