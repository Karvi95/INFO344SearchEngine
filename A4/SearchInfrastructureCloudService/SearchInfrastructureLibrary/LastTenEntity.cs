using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SearchInfrastructureLibrary
{
    public class LastTenEntity : TableEntity
    {
        public string lastTen { get; set; }

        public LastTenEntity(string toInsert)
        {
            this.PartitionKey = "LastTen";
            this.RowKey = "0";

            this.lastTen = toInsert;
        }

        public LastTenEntity() { }

    }
}
