using Microsoft.WindowsAzure.Storage.Table;
using System.Security.Cryptography;
using System.Text;

namespace SearchInfrastructureLibrary
{
    public class InvertedIndex : TableEntity
    {
        public string URL { get; set; }

        public string pageTitle { get; set; }
        public string date { get; set; }

        public InvertedIndex(string word, string URL, string pageTitle, string date)
        {
            this.PartitionKey = new MD5Hash(word).hashed;

            this.RowKey = new MD5Hash(URL).hashed;
        
            this.URL = URL;
            this.pageTitle = pageTitle;
            this.date = date;
        }

        public InvertedIndex() { }
    }
}