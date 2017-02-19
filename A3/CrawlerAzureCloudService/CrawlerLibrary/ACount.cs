using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLibrary
{
    public class ACount : TableEntity
    {
        public int count { get; set; }
        public ACount(int count, string URL)
        {
            this.PartitionKey = URL;
            this.RowKey = "count";

            this.count = count;
            
        }

        public ACount() { }

    }
}
