using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WPFAsynchronusExample
{
    public class ProgressReportData
    {
        public List<WebDataResult> WebDataResult { get; set; } = new List<WebDataResult>();
        public int PercentageCompleted { get; set; } = 0;
    }
    public class WebDataResult
    {
        public string Url { get; set; }
        public int Length { get; set; }
    }
    public class DemoMethod
    {
        private readonly List<string> Address = new List<string> {
            "https://stackoverflow.com/",
            "https://www.c-sharpcorner.com/",
            "https://vnexpress.net/",
            "https://dantri.com.vn/",
            "https://docs.microsoft.com/en-us/dotnet/api/system.net.webclient?view=net-6.0",
            "https://docs.microsoft.com/en-us/dotnet/api/system.net.webclient.-ctor?view=net-6.0",
            "https://docs.microsoft.com/en-us/dotnet/api/?view=net-6.0",
            "https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0",
            "https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0",
            "https://docs.microsoft.com/en-us/aspnet/core/fundamentals/choose-aspnet-framework?view=aspnetcore-6.0",
            "https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.run?view=net-6.0",
            "https://www.pluralsight.com/guides/using-task-run-async-await",
            "https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.run?view=net-6.0",
            "https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.runsynchronously?view=net-6.0",
            "https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.waitasync?view=net-6.0",
            "https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.whenany?view=net-6.0",
            "https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1?view=net-6.0"
        };

        public IEnumerable<WebDataResult> DownloadWebsite()
        {
            List<WebDataResult> results = new List<WebDataResult>();
            foreach (var address in Address)
            {
                results.Add(RunDownloadString(address));
            }

            return results;
        }

        public async Task DownloadWebsiteAsync(IProgress<ProgressReportData> progress, CancellationToken cancellationToken)
        {
            List<WebDataResult> results = new List<WebDataResult>();

            foreach (var address in Address)
            {
                var stringResult = await RunDownloadStringAsync(address);
                cancellationToken.ThrowIfCancellationRequested();
                results.Add(new WebDataResult() { Url = address, Length = stringResult.Length });
                progress.Report(new ProgressReportData() { WebDataResult = results, PercentageCompleted = (results.Count * 100) / Address.Count });
            }
        }

        public async Task<IEnumerable<WebDataResult>> DownloadWebsiteParallelAsync(IProgress<ProgressReportData> progress)
        {
            List<Task<WebDataResult>> WebDataResults = new List<Task<WebDataResult>>();
            foreach (var address in Address)
            {
                WebDataResults.Add(Task.Run(() => RunDownloadStringAsync(address)));
            }
            var result = await Task.WhenAll(WebDataResults);
            return result;
        }

        public async Task DownloadWebsiteParallelAsyncV2(IProgress<ProgressReportData> progress)
        {
            List<WebDataResult> WebDataResults = new List<WebDataResult>();
            await Task.Run(() =>
            {
                Parallel.ForEach<string>(Address, address =>
                {
                    var result = RunDownloadString(address);
                    WebDataResults.Add(new WebDataResult() { Url = address, Length = result.Length });
                    progress.Report(new ProgressReportData() { WebDataResult = WebDataResults, PercentageCompleted = (WebDataResults.Count * 100) / Address.Count });
                });
            });
        }

        private WebDataResult RunDownloadString(string address)
        {
            using (HttpClient client = new HttpClient())
            {
                var result = new WebDataResult();
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.GetAsync(address).Result;
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    var stringResult = response.Content.ReadAsStringAsync().Result;
                    result.Url = address;
                    result.Length = stringResult.Length;
                }

                return result;
            }
        }
        private async Task<WebDataResult> RunDownloadStringAsync(string address)
        {
            using (HttpClient client = new HttpClient())
            {
                var result = new WebDataResult();
                client.DefaultRequestHeaders.Accept.Clear();
                var response = await client.GetAsync(address);
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    result.Url = address;
                    result.Length = stringResult.Length;
                }

                return result;
            }
        }
    }
}
