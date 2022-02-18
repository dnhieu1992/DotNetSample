using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WPFAsynchronusExample
{
    public class ProgressReportData
    {
        public string Url { get; set; }
        public int Percentage { get; set; }
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
        private readonly WebClient client = new WebClient();

        public IEnumerable<string> DownloadWebsite()
        {
            List<string> results = new List<string>();
            foreach (var address in Address)
            {
                var stringResult = $"{address} : {client.DownloadString(address)?.Length} {Environment.NewLine}";
                results.Add(stringResult);
            }

            return results;
        }

        public async Task<IEnumerable<string>> DownloadWebsiteAsync(IProgress<ProgressReportData> progress, CancellationToken cancellationToken)
        {
            var total = Address.Count() > 0 ? Address.Count : 1;
            List<string> results = new List<string>();
            foreach (var address in Address)
            {
                var stringResult = await client.DownloadStringTaskAsync(address);
                cancellationToken.ThrowIfCancellationRequested();
                results.Add($"{address} : {stringResult?.Length} {Environment.NewLine}");
                progress.Report(new ProgressReportData() { Url = address, Percentage = (results.Count * 100)/total});
            }

            return results;
        }

        public async Task<IEnumerable<WebDataResult>> DownloadWebsiteParallelAsync(IProgress<int> progress)
        {
            List<Task<WebDataResult>> WebDataResults = new List<Task<WebDataResult>>();
            foreach (var address in Address)
            {
                WebDataResults.Add(Task.Run(() => GetStringOfWebsite(address)));
            }
            var result = await Task.WhenAll(WebDataResults);
            return result;
        }

        private WebDataResult GetStringOfWebsite(string address)
        {
            WebClient webClient = new WebClient();
            var result = webClient.DownloadString(address);
            return new WebDataResult() { Url = address, Length = result.Length };
        }
    }
}
