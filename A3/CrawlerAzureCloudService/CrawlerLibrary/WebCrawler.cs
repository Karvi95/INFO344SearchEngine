﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;

namespace CrawlerLibrary
{
    public class WebCrawler
    {
        private readonly DateTime earliestDate;
        private HashSet<string> disalloweds;
        private HashSet<string> visited;
        private List<string> lastten;

        public WebCrawler()
        {
            earliestDate = new DateTime(2017, 2, 10);
            disalloweds = new HashSet<string>();
            visited = new HashSet<string>();
            lastten = new List<string>();
        }

        public void Initialize(StorageMaster myStorageMaster, string robotsDotTXT)
        {
            myStorageMaster.SetStatus(StorageMaster._StatusLoading);

            // Reads cnn.com/robot.txt to add disalloweds to list

            WebResponse response;
            WebRequest request = WebRequest.Create(robotsDotTXT);
            response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().ToLower().Trim();

                    if (line.StartsWith("disallow: "))
                    {
                        string header = robotsDotTXT.Substring(0, robotsDotTXT.Length - 11);
                        string disallowed = line.Substring(10);
                        disalloweds.Add(header + disallowed);
                    }
                    else if (!disalloweds.Contains(line))
                    {
                        string[] elements = line.Split(new char[] { ' ' });
                        Debug.WriteLine(elements);
                        for (int i = 0; i < elements.Length; i++)
                        {
                            if (robotsDotTXT == StorageMaster._CNNRobotsTXT && elements[i].EndsWith(".xml"))
                            {
                                myStorageMaster.GetXMLsQueue().AddMessage(new CloudQueueMessage(elements[i]));

                                //xmlsList.Add(elements[i]);
                            }
                            else if (robotsDotTXT == StorageMaster._BleacherReportRobotsTXT && elements[i].EndsWith("nba.xml"))
                            {
                                myStorageMaster.GetXMLsQueue().AddMessage(new CloudQueueMessage(elements[i]));
                            }
                        }
                    }
                }
            }

            //myStorageMaster.GetXMLsQueue();
            //List<string> xmlsList = new List<string>();

            while (myStorageMaster.GetXMLsQueue().ApproximateMessageCount != 0 /*xmlsList.Count != 0*/)
            {
                XmlDocument xDoc = new XmlDocument();

                CloudQueueMessage unloadXML = myStorageMaster.GetXMLsQueue().GetMessage();
                if (unloadXML != null)
                {
                    string urlAsString = unloadXML.AsString;

                    try
                    {
                        xDoc.Load(urlAsString);
                        //xDoc.Load(xmlsList[0]);
                        string xml = xDoc.InnerXml;

                        using (XmlReader xreader = XmlReader.Create(new StringReader(xml)))
                        {
                            while (xreader.ReadToFollowing("loc"))
                            {
                                string content = xreader.ReadElementContentAsString();
                                DateTime urlDate;
                                bool validDateCheck = false;

                                if (robotsDotTXT == StorageMaster._CNNRobotsTXT && xreader.ReadToFollowing("lastmod"))
                                {
                                    urlDate = DateTime.Parse(xreader.ReadElementContentAsString());
                                    validDateCheck = (urlDate == null || urlDate.AddMonths(2).CompareTo(DateTime.Now) >= 0) ? true : false;

                                    if (validDateCheck)
                                    {
                                        if (content.EndsWith(".xml"))
                                        {
                                            myStorageMaster.GetXMLsQueue().AddMessage(new CloudQueueMessage(content));
                                            //xmlsList.Add(content);
                                            Debug.WriteLine("Loaded " + content);
                                        }
                                        else if (content.Contains("cnn.com"))
                                        {
                                            if (!visited.Contains(content))
                                            {
                                                visited.Add(content);
                                                myStorageMaster.GetUrlsQueue().AddMessage(new CloudQueueMessage(content));
                                                Debug.WriteLine("Loaded " + content);
                                            }
                                        }
                                    }
                                }
                                else if (content.Contains("bleacherreport"))
                                {
                                    if (!visited.Contains(content))
                                    {
                                        visited.Add(content);
                                        myStorageMaster.GetUrlsQueue().AddMessage(new CloudQueueMessage(content));
                                        Debug.WriteLine("Loaded " + content);
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine(content + " did not pass date check");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        HtmlWeb htmlWeb = new HtmlWeb();
                        Debug.WriteLine("An error occured at: " + urlAsString);
                        AnError error = new AnError(urlAsString, ex.Message);
                        TableOperation errorOperation = TableOperation.Insert(error);
                        myStorageMaster.GetErrorsTable().Execute(errorOperation);
                    }
                    myStorageMaster.GetXMLsQueue().DeleteMessage(unloadXML);
                    //xmlsList.RemoveAt(0);
                } else
                {
                    break;
                }

            }
            Debug.WriteLine("Crawler Initialized. Waiting to Crawl.");
            myStorageMaster.SetStatus(StorageMaster._StatusIdling);
        }

        public void crawl(StorageMaster myStorageMaster, string aURL)
        {
            myStorageMaster.SetStatus(StorageMaster._StatusCrawling);
            if (!disalloweds.Contains(aURL))
            {
                try
                {
                    HtmlDocument htmlPage = new HtmlWeb().Load(aURL);

                    var title = htmlPage.DocumentNode.SelectSingleNode("//head/title");
                    if (title != null)
                    {
                        string pageTitle = title.InnerText;

                        if (pageTitle.Equals("Error"))
                        {
                            HtmlWeb htmlWeb = new HtmlWeb();
                            Debug.WriteLine("An error occured at: " + aURL);
                            AnError error = new AnError(aURL, htmlWeb.StatusCode.ToString());
                            TableOperation errorOperation = TableOperation.Insert(error);
                            myStorageMaster.GetErrorsTable().Execute(errorOperation);
                        }
                        else
                        {

                            int index = myStorageMaster.GetIndexSize();

                            // Removes suffixes from title
                            pageTitle = pageTitle.Split(new string[] { " - CNN.com" }, StringSplitOptions.None)[0];
                            pageTitle = pageTitle.Split(new string[] { " | Bleacher Report" }, StringSplitOptions.None)[0];
                            pageTitle = pageTitle.Split(new string[] { " - CNNPolitics.com" }, StringSplitOptions.None)[0];
                            pageTitle = pageTitle.Split(new string[] { " - CNN Video" }, StringSplitOptions.None)[0];
                            pageTitle = pageTitle.Split(new string[] { " - Special Reports from CNN.com" }, StringSplitOptions.None)[0];

                            try
                            {

                                if (lastten.Count < 10)
                                {
                                    lastten.Add(aURL);
                                } else
                                {
                                    lastten.RemoveAt(0);
                                    lastten.Add(aURL);
                                }

                                string toInsert = string.Join("|", lastten.ToArray());

                                LastTenEntity currentTen = new LastTenEntity(toInsert);
                                TableOperation currentTenOperation = TableOperation.InsertOrReplace(currentTen);
                                myStorageMaster.GetLastTenTable().Execute(currentTenOperation);


                                WebPage newWebpage = new WebPage(pageTitle, aURL.ToLower()/*convertedURL*/, index);
                                TableOperation urlOperation = TableOperation.Insert(newWebpage);
                                myStorageMaster.GetUrlsTable().Execute(urlOperation);
                            }
                            catch (Exception ex)
                            {
                                HtmlWeb htmlWeb = new HtmlWeb();
                                Debug.WriteLine("An error occured at: " + aURL);
                                AnError error = new AnError(aURL, ex.Message);
                                TableOperation errorOperation = TableOperation.Insert(error);
                                myStorageMaster.GetErrorsTable().Execute(errorOperation);
                            }
                            try
                            {
                                foreach (HtmlNode node in htmlPage.DocumentNode.SelectNodes("//a").ToArray())
                                {
                                    string href = node.GetAttributeValue("href", string.Empty.Trim());

                                    if (!string.IsNullOrEmpty(href))
                                    {
                                        try
                                        {
                                            // get subdomain here and path logic here
                                            Uri theURI = new Uri(new Uri(aURL), href);

                                            string potentialURL = theURI.ToString();
                                            if (potentialURL.StartsWith("www"))
                                            {
                                                potentialURL = potentialURL.Replace("www.", "");
                                            }

                                            if (!potentialURL.StartsWith("http://"))
                                            {
                                                potentialURL = "http://" + potentialURL;
                                            }

                                            if (!visited.Contains(potentialURL))
                                            {
                                                // Href is of cnn or bleacherreport domain
                                                if (potentialURL.Contains("cnn.com") || potentialURL.Contains("bleacherreport.com"))
                                                {
                                                    if (!disalloweds.Contains(potentialURL))
                                                    {
                                                        visited.Add(potentialURL);
                                                        CloudQueueMessage hrefMessage = new CloudQueueMessage(potentialURL);
                                                        myStorageMaster.GetUrlsQueue().AddMessage(hrefMessage);
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            HtmlWeb htmlWeb = new HtmlWeb();
                                            Debug.WriteLine("An error occured at: " + aURL);
                                            AnError error = new AnError(aURL, ex.Message);
                                            TableOperation errorOperation = TableOperation.Insert(error);
                                            myStorageMaster.GetErrorsTable().Execute(errorOperation);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                HtmlWeb htmlWeb = new HtmlWeb();
                                Debug.WriteLine("An error occured at: " + aURL);
                                AnError error = new AnError(aURL, ex.Message);
                                TableOperation errorOperation = TableOperation.Insert(error);
                                myStorageMaster.GetErrorsTable().Execute(errorOperation);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    HtmlWeb htmlWeb = new HtmlWeb();
                    Debug.WriteLine("An error occured at: " + aURL);
                    AnError error = new AnError(aURL, ex.Message);
                    TableOperation errorOperation = TableOperation.Insert(error);
                    myStorageMaster.GetErrorsTable().Execute(errorOperation);
                }
            }
            myStorageMaster.SetStatus(StorageMaster._StatusIdling);
        }
    }
}