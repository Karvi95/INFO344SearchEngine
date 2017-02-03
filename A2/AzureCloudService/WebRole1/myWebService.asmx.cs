using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for myWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class myWebService : System.Web.Services.WebService
    {

        private static Trie myTrie = new Trie();
        private string myPath = System.Web.HttpContext.Current.Server.MapPath(@"/output.txt");


        [WebMethod]
        public string downloadBlob()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("querysuggestioncontainer");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("output.txt");

            // Use DownloadToFile and appropriate error handling
            try
            {
                blockBlob.DownloadToFile(myPath, System.IO.FileMode.Create);
            }
            catch (Exception e)
            {
                return "Some problems: " + e.Message;
            }
            return "Downloaded Data";
        }

        [WebMethod]
        public string buildTrie()
        {
            int InsertionCounter = 0;
            PerformanceCounter MemCounter = new PerformanceCounter("Memory", "Available MBytes");

            using (StreamReader reader = new StreamReader(myPath))
            {
                while (!reader.EndOfStream)
                {
                    if (InsertionCounter % 1000 == 0 && MemCounter.NextValue() < 20)
                    {
                        break;
                    }

                    string line = reader.ReadLine().Trim().ToLower();
                    myTrie.insert(line);
                    InsertionCounter++;
                }
            }
            return "Trie was Built.";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string searchTrie(string queryValue)
        {
            if (myTrie == null)
            {
                myTrie = new Trie();
                this.buildTrie();
            }

            List<string> tenItems = myTrie.getPrefix(queryValue.ToLower().Trim().Replace(' ', '_'));

            // Output processing to replace underscores with spaces and other stuff. 
            if (!tenItems.Any())
            {
                return "No results found.";
            }
            else
            {
                for (int i = 0; i < tenItems.Count; i++)
                {
                    string s = tenItems[0];
                    tenItems.Remove(s);
                    s = s.Replace('_', ' ');
                    tenItems.Add(s);
                }
            }
            return new JavaScriptSerializer().Serialize(tenItems);
        }
    }
}
