using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerLibrary
{
    public class ADuplicate : TableEntity
    {
        public ADuplicate(int count)
        {
            this.PartitionKey = "" + 0;
            this.RowKey = "" + 0;

            this.Count = count;
        }

        public ADuplicate() { }

        public int Count { get; set; }
    }
}