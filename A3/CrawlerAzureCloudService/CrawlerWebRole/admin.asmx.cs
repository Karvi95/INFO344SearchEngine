using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Text;
using CrawlerLibrary;


namespace CrawlerWebRole
{
    /// <summary>
    /// Summary description for admin
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class admin : System.Web.Services.WebService
    {

        private static StorageMaster myStorageMaster;
        //public static readonly string _CNNRobotsTXT = "http://www.cnn.com/robots.txt";
        //public static readonly string _BleacherReportRobotsTXT = "http://bleacherreport.com/robots.txt";

        private bool readyState;


        public admin()
        {
            myStorageMaster = new StorageMaster(ConfigurationManager.AppSettings["StorageConnectionString"]);
        }


        [WebMethod]
        public string startCrawling()
        {
            
            clearIndex();
            myStorageMaster.GetDirectivesQueue().AddMessage(new CloudQueueMessage(StorageMaster._StartMessage));
             
            return "Started.";
        }

        [WebMethod]
        public string stopCrawling()
        {
            myStorageMaster.GetDirectivesQueue().AddMessage(new CloudQueueMessage(StorageMaster._StopMessage));
    
            return "No longer crawling in my crawl.";
        }

        [WebMethod]
        public string clearIndex()
        {
            myStorageMaster.GetDirectivesQueue().AddMessage(new CloudQueueMessage(StorageMaster._StopMessage));

            myStorageMaster.clearAll();

            // Pause Thread
            Thread.Sleep(40000);

            return "Purged all Storage.";
        }


        [WebMethod]
        public List<string> Report()
        {
            List<string> results = new List<string>();

            // State of worker role
            results.Add(myStorageMaster.GetStatus());

            // CPU utilization %
            results.Add(myStorageMaster.GetPerformanceCounter(StorageMaster._CPUCounter));

            // RAM available
            results.Add(myStorageMaster.GetPerformanceCounter(StorageMaster._RAMCounter));

            // # URL's crawled
            results.Add("" + myStorageMaster.GetTotalCrawledUrls());

            // Size of queue (number of URL's to be crawled)
            results.Add(myStorageMaster.GetQueueSize(myStorageMaster.GetUrlsQueue()).ToString());

            // Size of index (table storage with crawled data)
            results.Add(myStorageMaster.GetIndexSize().ToString());

            // Last 10 URL's crawled
            string sumString = "";

            foreach (string s in myStorageMaster.GetRecentUrls())
            {
                sumString += s + " ";
            }
            results.Add(sumString);


            // Any error URL's
            Dictionary<string, string> errorDict = myStorageMaster.GetErrors();

            string errorURLString = string.Join(" ", errorDict.Keys.ToList());

            string errorMessagesString = "";
            foreach (KeyValuePair<string, string> e in errorDict)
            {
                errorMessagesString += errorDict[e.Key] + "||";
            }
            results.Add(errorMessagesString);

            results.Add(errorURLString);

            return results;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string retrieveTitle(string URL)
        {
            URL = URL.ToLower();

            string resultTitle = "No result found";

            // Because 
            if (URL.EndsWith("/index.html"))
            {
                URL = URL.Substring(0, URL.Length - 11);
            }

            string searcher = new MD5Hash(URL).hashed;

            TableQuery<WebPage> titleQuery = new TableQuery<WebPage>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, searcher)
            );

            CloudTable urlsTable = myStorageMaster.GetUrlsTable();

            var searchList = urlsTable.ExecuteQuery(titleQuery).ToList();

            if (urlsTable.Exists())
            {
                if (searchList.Count > 0)
                {
                    resultTitle = searchList[0].title;
                }

            }

            return new JavaScriptSerializer().Serialize(resultTitle);
            
        }
    }
}
