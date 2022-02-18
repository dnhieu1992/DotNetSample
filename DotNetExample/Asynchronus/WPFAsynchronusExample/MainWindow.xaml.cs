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
            PrintData(results);
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
                var results = await demoMethod.DownloadWebsiteAsync(progress, source.Token);
                stopWatch.Stop();
                PrintData(results);
                txtResults.Text += $"Time consume: {stopWatch.ElapsedMilliseconds}";
            }
            catch(Exception ex)
            {
                txtResults.Text += $"The request was cancelled.{Environment.NewLine}";
            }
        }

        private async void Async_Parallel_Excute_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            var progress = new Progress<int>(x => progressBar.Value = x);
            txtResults.Text = "";
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var results = await demoMethod.DownloadWebsiteParallelAsync(progress);
            stopWatch.Stop();
            foreach(var item in results)
            {
                txtResults.Text += $"{item.Url}: {item.Length} {Environment.NewLine}";
            }
            txtResults.Text += $"Time consume: {stopWatch.ElapsedMilliseconds}";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            source.Cancel();
        }

        private void PrintData(IEnumerable<string> results)
        {
            foreach (var item in results)
            {
                txtResults.Text += item;
            }
        }

        private void ReportProgress(object sender, ProgressReportData e)
        {
            progressBar.Value = e.Percentage;
            txtResults.Text += $"{e.Url} {Environment.NewLine}";
        }
    }
}
