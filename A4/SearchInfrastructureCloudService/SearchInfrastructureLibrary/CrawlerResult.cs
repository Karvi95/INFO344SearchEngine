using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchInfrastructureLibrary
{
    public class CrawlerResult
    {
        public string Item1;
        public int Item2;
        public string Item3;
        public string Item4;

        public CrawlerResult() { }

        public CrawlerResult(Tuple<string, int, string, string> t)
        {
            Item1 = t.Item1;
            Item2 = t.Item2;
            Item3 = t.Item3;
            Item4 = t.Item4;
        }
    }
}
