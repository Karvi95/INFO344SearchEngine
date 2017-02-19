using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerLibrary
{
    public class WebPage : TableEntity
    {
        public string title { get; set; }

        public string URL { get; set; }

        public string date { get; set; }

        public int index { get; set; }

        public WebPage(string title, string URL, int index)
        {
            this.PartitionKey = new MD5Hash(URL).hashed;
            this.RowKey = new MD5Hash(title).hashed;

            this.title = title;
            this.URL = URL;
            this.date = DateTime.Now.ToString();
            this.index = index;
        }

        public WebPage() { }
    }
}