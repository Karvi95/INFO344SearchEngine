﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Microsoft.Azure; // Namespace for CloudConfigurationManager
//using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
//using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types

namespace WebRole1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("C:\\Users\\ReppuVanWinkle\\Downloads\\output.txt"))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                Trie myTrie = new Trie();
                while ((line = sr.ReadLine()) != null)
                {
                    myTrie.insert(line);
                    
                }

                List<string> output = myTrie.getPrefix("Ac");
                foreach (string s in output)
                {
                    System.Console.WriteLine(s);
                }
                System.Console.ReadLine();
                //string outputCat = myTrie.getPrefix("Cat");
                //System.Console.WriteLine(outputCat);
            }
        }
    }
}
