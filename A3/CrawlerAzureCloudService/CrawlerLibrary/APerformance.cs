using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLibrary
{
    public class APerformance : TableEntity
    {
        public string performance { get; set; }
    
        public APerformance(int counterId, string performance)
        {
            this.PartitionKey = "" + counterId;
            this.RowKey = "" + 0;

            this.performance = performance;
        }

        public APerformance() { }


    }
}
