using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerLibrary
{
    public class AnError : TableEntity
    {
        public string URL { get; set; }

        public string Error { get; set; }

        public AnError(string URL, string Error)
        {
            this.PartitionKey = Guid.NewGuid().ToString();
            this.RowKey = Guid.NewGuid().ToString();

            this.URL = URL;
            this.Error = Error;
        }

        public AnError() { }
    }
}