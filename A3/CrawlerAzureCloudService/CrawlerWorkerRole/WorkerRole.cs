using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using CrawlerLibrary;
using HtmlAgilityPack;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CrawlerWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private static StorageMaster myStorageMaster;
        private static WebCrawler myCrawler;

        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;

        private int wait;
        private static int performanceUpdate = 5;

        public override void Run()
        {

            Trace.TraceInformation("CrawlerWorkerRole is running");
            myStorageMaster = new StorageMaster(ConfigurationManager.AppSettings["StorageConnectionString"]);
            myCrawler = new WebCrawler();

            //myStorageMaster.SetStatus(StorageMaster._StatusLoading);

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            wait = 0;
            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("CrawlerWorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("CrawlerWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("CrawlerWorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                // Store performance information every 500ms
                if (wait == 0)
                {
                    string CPU = cpuCounter.NextValue().ToString();
                    string RAM = ramCounter.NextValue().ToString();

                    APerformance cpuPerformance = new APerformance(StorageMaster._CPUCounter, CPU);
                    TableOperation cpuOperation = TableOperation.InsertOrReplace(cpuPerformance);
                    myStorageMaster.GetPerformancesTable().Execute(cpuOperation);

                    APerformance ramPerformance = new APerformance(StorageMaster._RAMCounter, RAM);
                    TableOperation ramOperation = TableOperation.InsertOrReplace(ramPerformance);
                    myStorageMaster.GetPerformancesTable().Execute(ramOperation);
                }
                else if (wait == performanceUpdate)
                {
                    wait = 0;
                }
                else
                {
                    wait++;
                }

                // Read from command queue every 50ms
                CloudQueueMessage directiveMessage = myStorageMaster.GetDirectivesQueue().GetMessage();

                // Non-empty message
                if (directiveMessage != null)
                {
                    // Start message
                    if (directiveMessage.AsString.Equals(StorageMaster._StartMessage))
                    {
                        // Crawler is idling, start crawl
                        if (myStorageMaster.GetStatus() == StorageMaster._StatusIdling)
                        {
                            myCrawler.Initialize(myStorageMaster, StorageMaster._CNNRobotsTXT);
                            myCrawler.Initialize(myStorageMaster, StorageMaster._BleacherReportRobotsTXT);
                        }

                        // Always remove from queue
                        myStorageMaster.GetDirectivesQueue().DeleteMessage(directiveMessage);
                    }
                    // Stop message
                    else if (directiveMessage.AsString.Equals(StorageMaster._StopMessage))
                    {
                        // Crawler is still loading, keep stop message pending
                        if (myStorageMaster.GetStatus() != StorageMaster._StatusLoading)
                        {
                            // Crawler is currently crawling
                            // Stop all crawling and clear url queue
                            myStorageMaster.GetUrlsQueue().Clear();
                            myStorageMaster.SetStatus(StorageMaster._StatusIdling);

                            myStorageMaster.GetDirectivesQueue().DeleteMessage(directiveMessage);
                        }
                    }
                }
                // No pending command, crawler is not loading and url queue is not empty
                // Take one url and crawl
                else if (myStorageMaster.GetQueueSize(myStorageMaster.GetUrlsQueue()) != 0)
                {
                    CloudQueueMessage URL = myStorageMaster.GetUrlsQueue().GetMessage();
                    myStorageMaster.GetUrlsQueue().DeleteMessage(URL);
                    myCrawler.crawl(myStorageMaster, URL.AsString);
                }

                await Task.Delay(500);
            }
        }
    }
}
