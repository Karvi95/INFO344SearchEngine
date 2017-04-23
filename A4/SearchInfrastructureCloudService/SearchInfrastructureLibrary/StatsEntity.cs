using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchInfrastructureLibrary
{
    public class StatsEntity : TableEntity
    {

        public string statusString;
        public string performanceString;
        public string totalString;
        public string qsizeString;
        public string isizeString;
        public string lasttenString;
        public string errorString;

        public StatsEntity(string statusString, string performanceString, string totalString, string qsizeString, string isizeString, string lasttenString, string errorString)
        {
            this.PartitionKey = "CurrentStatus";
            this.RowKey = "0";

            this.statusString = statusString;
            this.performanceString = performanceString;
            this.totalString = totalString;
            this.qsizeString = qsizeString;
            this.isizeString = isizeString;
            this.lasttenString = lasttenString;
            this.errorString = errorString;
        }
    }
}
