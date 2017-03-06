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
using SearchInfrastructureLibrary;

namespace SearchInfrastructureWebRole
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
        private static Dictionary<string, List<string>> cache;

        public admin()
        {
            myStorageMaster = new StorageMaster(ConfigurationManager.AppSettings["StorageConnectionString"]);
        }


        [WebMethod]
        public string startCrawling()
        {
            // Puge what was crawled
            clearIndex();

            // Start Crawling
            myStorageMaster.GetDirectivesQueue().AddMessage(new CloudQueueMessage(StorageMaster._StartMessage));

            return "Started.";
        }

        [WebMethod]
        public string stopCrawling()
        {
            // Stop Crawling
            myStorageMaster.GetDirectivesQueue().AddMessage(new CloudQueueMessage(StorageMaster._StopMessage));

            return "No longer crawling in my crawl.";
        }

        [WebMethod]
        public string clearIndex()
        {
            // Stop Crawling
            myStorageMaster.GetDirectivesQueue().AddMessage(new CloudQueueMessage(StorageMaster._StopMessage));

            // Clear all storage
            myStorageMaster.clearAll();

            // Pause Thread
            Thread.Sleep(40000);

            return "Purged all Storage.";
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getStatus()
        {
            List<string> results = new List<string>();

            // State of worker role
            results.Add(myStorageMaster.GetStatus());

            return new JavaScriptSerializer().Serialize(results);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getPerformance()
        {
            List<string> results = new List<string>();

            // CPU utilization %
            results.Add(myStorageMaster.GetPerformanceCounter(StorageMaster._CPUCounter));

            // RAM available
            results.Add(myStorageMaster.GetPerformanceCounter(StorageMaster._RAMCounter));

            return new JavaScriptSerializer().Serialize(results);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getTotal()
        {

            List<string> results = new List<string>();

            // # URL's crawled
            results.Add("" + myStorageMaster.GetTotalCrawledUrls());

            return new JavaScriptSerializer().Serialize(results);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string sizeQ()
        {

            List<string> results = new List<string>();
            // Size of queue (number of URL's to be crawled)
            results.Add(myStorageMaster.GetQueueSize(myStorageMaster.GetUrlsQueue()).ToString());
            return new JavaScriptSerializer().Serialize(results);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string sizeI()
        {

            List<string> results = new List<string>();
            // Size of index (table storage with crawled data)
            results.Add(myStorageMaster.GetIndexSize().ToString());
            return new JavaScriptSerializer().Serialize(results);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string lastTen()
        {

            List<string> results = new List<string>();
            // Last 10 URL's crawled
            string sumString = "";

            foreach (string s in myStorageMaster.GetRecentUrls())
            {
                sumString += s + " ";
            }
            results.Add(sumString);
            return new JavaScriptSerializer().Serialize(results);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getErrors()
        {

            List<string> results = new List<string>();
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
            return new JavaScriptSerializer().Serialize(results);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string retrieveTitle(string URL)
        {
            URL = URL.ToLower();

            string resultTitle = "No result found";

            // Because get rid of index.html
            if (URL.EndsWith("/index.html"))
            {
                URL = URL.Substring(0, URL.Length - 11);
            }

            string searcher = new MD5Hash(URL).hashed;

            TableQuery<WebPage> titleQuery = new TableQuery<WebPage>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, searcher)
            );

            CloudTable urlsTable = myStorageMaster.GetUrlsTable();

            var searchList = urlsTable.ExecuteQuery(titleQuery).ToList();

            if (urlsTable.Exists())
            {
                if (searchList.Count > 0)
                {
                    resultTitle = searchList[0].pageTitle;
                }

            }

            return new JavaScriptSerializer().Serialize(resultTitle);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string search(string input)
        {

            List<string> results = new List<string>();
            if (cache == null)
            {
                cache = new Dictionary<string, List<string>>();
            }

            if (cache.Count < 100)
            {
                if (cache.ContainsKey(input))
                {
                    results = cache[input];
                }
                else
                {
                    results = this.tableSearch(input);
                }
            }
            else
            {
                cache = new Dictionary<string, List<string>>();
                results = this.tableSearch(input);
            }
            return new JavaScriptSerializer().Serialize(results);
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> tableSearch(string searcher)
        {
            List<InvertedIndex> results = new List<InvertedIndex>();

            string[] keywords = searcher.ToLower().Split(' ');

            foreach (string word in keywords)
            {
                // Remove punctuation
                string processedWord = Convert.ToBase64String(Encoding.UTF8.GetBytes(new string(word.ToCharArray().Where(c => !char.IsPunctuation(c)).ToArray()).ToLower()));

                if (processedWord.Length > 0)
                {
     
                    string check = new MD5Hash(processedWord).hashed;

                    TableQuery<InvertedIndex> query = new TableQuery<InvertedIndex>()
                       .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, check)
                   );

                    var tempList = myStorageMaster.GetSearchQueryTable().ExecuteQuery(query).ToList();

                    if (tempList.Count > 0)
                    {
                        for (int i = 0; i < tempList.Count; i++)
                        {
                            results.Add(tempList[i]);
                        }
                    }

                }
            }

            return rankSuggestions(searcher, results);

        }

        private List<string> rankSuggestions(string searcher, List<InvertedIndex> results)
        {
            var query = results
                .GroupBy(x => x.RowKey)
                .Select(x => new Tuple<string, int, string, string, string>(x.Key, x.ToList().Count(), x.First().pageTitle, x.First().URL, x.First().date))
                .OrderByDescending(x => x.Item2)
                .ThenByDescending(x => x.Item5)
                .Take(10)
                .Select(x => x.Item3 + "|" + x.Item4)
                .ToList();

            cache[searcher] = new List<string>(query);

            return query;

        }
    }
}