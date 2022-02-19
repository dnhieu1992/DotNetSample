using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFAsynchronusExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DemoMethod demoMethod = new DemoMethod();
        CancellationTokenSource source = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Normal_Excution_Click(object sender, RoutedEventArgs e)
        {
            txtResults.Text = "";
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var results = demoMethod.DownloadWebsite();
            stopWatch.Stop();
            PrintData(results.ToList());
            txtResults.Text += $"Time consume: {stopWatch.ElapsedMilliseconds}";
        }

        private async void Async_Excute_Click(object sender, RoutedEventArgs e)
        {
            txtResults.Text = "";
            try
            {
                var progress = new Progress<ProgressReportData>();
                progress.ProgressChanged += ReportProgress;

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await demoMethod.DownloadWebsiteAsync(progress, source.Token);
                stopWatch.Stop(); ;
                txtResults.Text += $"Time consume: {stopWatch.ElapsedMilliseconds}";
            }
            catch (Exception ex)
            {
                txtResults.Text += $"The request was cancelled.{Environment.NewLine}";
            }
        }

        private async void Async_Parallel_Excute_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<ProgressReportData>();
            progress.ProgressChanged += ReportProgress;
            txtResults.Text = "";
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            //var result = await demoMethod.DownloadWebsiteParallelAsync(progress);
            await demoMethod.DownloadWebsiteParallelAsyncV2(progress);
            stopWatch.Stop();
            //PrintData(result);
            txtResults.Text += $"Time consume: {stopWatch.ElapsedMilliseconds}";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            source.Cancel();
        }

        private void PrintData(IEnumerable<WebDataResult> webDataResults)
        {
            txtResults.Text = "";
            foreach (var item in webDataResults.ToList())
            {
                txtResults.Text += $"{item.Url}: {item.Length} {Environment.NewLine}";
            }
        }

        private void ReportProgress(object sender, ProgressReportData e)
        {
            progressBar.Value = e.PercentageCompleted;
            PrintData(e.WebDataResult);
        }
    }
}
