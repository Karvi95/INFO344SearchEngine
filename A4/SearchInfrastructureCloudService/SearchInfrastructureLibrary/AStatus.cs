using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchInfrastructureLibrary
{
    public class AStatus : TableEntity
    {
        public string status { get; set; }

        public AStatus(int crawlerID, string status)
        {
            this.PartitionKey = "" + crawlerID;
            this.RowKey = "0";

            this.status = status;
        }

        public AStatus() { }
    }
}
