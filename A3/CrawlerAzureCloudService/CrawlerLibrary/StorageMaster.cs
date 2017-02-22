using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace CrawlerLibrary
{
    public class StorageMaster
    {
        public static string _StartMessage = "Start";
        public static readonly string _StopMessage = "Stop";
        public static readonly string _ErrorMessage = "ERROR FOUND";
        public static readonly string _StatusIdling = "Idling";
        public static readonly string _StatusLoading = "Loading";
        public static readonly string _StatusCrawling = "Crawling";
        public static readonly int _CPUCounter = 1;
        public static readonly int _RAMCounter = 2;
        public static readonly string _CNNRobotsTXT = "http://www.cnn.com/robots.txt";
        public static readonly string _BleacherReportRobotsTXT = "http://bleacherreport.com/robots.txt";

        private static CloudStorageAccount StorageAccount; // My Credentials

        private static CloudQueue directivesQueue; // Queue to direct worker role
        private static CloudQueue xmlsQueue; // Queue With Sitemaps
        private static CloudQueue urlsQueue; // Queue with unprocessed urls (not yet indexed)

        private static CloudTable lastTenTable; // Table containing list of lastTen urls
        private static CloudTable errorsTable; // Table containing list of error urls
        private static CloudTable performancesTable; // Table containing worker role performance information
        private static CloudTable statusesTable; // Table with current status of crawler(s)
        private static CloudTable urlsTable; // Table with processed urls (indexed page titles)


        public StorageMaster(string connectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = StorageAccount.CreateCloudQueueClient();
            CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();


            directivesQueue = queueClient.GetQueueReference("queue-of-directives");
            directivesQueue.CreateIfNotExists();

            xmlsQueue = queueClient.GetQueueReference("queue-of-xmls");
            xmlsQueue.CreateIfNotExists();

            urlsQueue = queueClient.GetQueueReference("queue-of-urls");
            urlsQueue.CreateIfNotExists();



            lastTenTable = tableClient.GetTableReference("tableoflastten");
            lastTenTable.CreateIfNotExists();

            errorsTable = tableClient.GetTableReference("tableoferrors");
            errorsTable.CreateIfNotExists();

            performancesTable = tableClient.GetTableReference("tableofperformances");
            performancesTable.CreateIfNotExists();

            statusesTable = tableClient.GetTableReference("tableofstatuses");
            statusesTable.CreateIfNotExists();

            urlsTable = tableClient.GetTableReference("tableofurls");
            urlsTable.CreateIfNotExists();


            TableQuery<AStatus> query = new TableQuery<AStatus>();
            if (statusesTable.ExecuteQuery(query).Count() == 0)
            {
                SetStatus(StorageMaster._StatusIdling);
            }

        }

        public CloudQueue GetXMLsQueue()
        {
            return xmlsQueue;
        }

        public CloudQueue GetDirectivesQueue()
        {
            return directivesQueue;
        }

        public CloudQueue GetUrlsQueue()
        {
            return urlsQueue;
        }

        public CloudTable GetLastTenTable()
        {
            return lastTenTable;
        }


        public CloudTable GetErrorsTable()
        {
            return errorsTable;
        }

        public CloudTable GetPerformancesTable()
        {
            return performancesTable;
        }

        public CloudTable GetUrlsTable()
        {
            return urlsTable;
        }

        public string GetStatus()
        {
            TableQuery<AStatus> query = new TableQuery<AStatus>().Take(1);

            string result = "";

            foreach (AStatus aStatus in statusesTable.ExecuteQuery(query))
            {
                result = aStatus.status;
            }

            return result;
        }

        public void SetStatus(string newStatus)
        {
            AStatus newStatusItem = new AStatus(0, newStatus);
            TableOperation operation = TableOperation.InsertOrReplace(newStatusItem);
            statusesTable.Execute(operation);
        }

        public void clearAll()
        {
            directivesQueue.Clear();
            urlsQueue.Clear();
            xmlsQueue.Clear();

            lastTenTable.DeleteIfExists();
            performancesTable.DeleteIfExists();
            statusesTable.DeleteIfExists();
            urlsTable.DeleteIfExists();
            errorsTable.DeleteIfExists();
        }

        public int GetIndexSize()
        {
            TableQuery<WebPage> query = new TableQuery<WebPage>();

            List<WebPage> pageList = new List<WebPage>();

            foreach (WebPage page in urlsTable.ExecuteQuery(query))
            {
                pageList.Add(page);
            }

            if (pageList.Count == 0)
            {
                return 1;
            }
            else
            {
                return (pageList.Count + 1);
            }
        }

        public int GetTotalCrawledUrls()
        {
            int indexNum = GetIndexSize() - 1;
            TableQuery<AnError> query = new TableQuery<AnError>();
            int errorNum = errorsTable.ExecuteQuery(query).Count();

            return indexNum + errorNum;
        }

        public int GetQueueSize(CloudQueue queue)
        {
            queue.FetchAttributes();
            return (int)queue.ApproximateMessageCount;
        }

        public List<string> GetRecentUrls()
        {
            List<string> result = new List<string>();
            TableQuery<LastTenEntity> currentTenQuery = new TableQuery<LastTenEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LastTen"));

            string recentTen = lastTenTable.ExecuteQuery(currentTenQuery).First().lastTen;

            recentTen = recentTen.Trim(new Char[] { '|' });
            string[] recentTenArray = recentTen.Split('|');

            for (int i = 0; i < recentTenArray.Length; i++)
            {
                result.Add(recentTenArray[i]);
            }

            return result;
        }

        public Dictionary<string, string> GetErrors()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            TableQuery<AnError> query = new TableQuery<AnError>().Take(10);

            foreach (AnError error in errorsTable.ExecuteQuery(query))
            {
                result.Add(error.URL, error.Error);
            }

            return result;
        }

        public string GetPerformanceCounter(int counterId)
        {
            TableQuery<APerformance> query = new TableQuery<APerformance>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "" + counterId));

            string result = "";

            foreach (APerformance performanceUnit in performancesTable.ExecuteQuery(query))
            {
                result = performanceUnit.performance;
            }

            return result;
        }
    }
}