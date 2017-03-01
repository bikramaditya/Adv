using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using JellyFish;
using System.Configuration;

namespace HelliumClient
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public static BackgroundWorker worker;
        public static bool isPaused = false;
        public static string log_messege;
        private List<TestCase> sortedList_selected_testcases;
        public static int maxItems = 0;
        public static double ctr = 0.0;
        public static JellyFishMain jf;
        public ProgressWindow(List<TestCase> sortedList_selected_testcases)
        {
            this.sortedList_selected_testcases = sortedList_selected_testcases;
            InitializeComponent();

            initializeJellyFish();

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;

            worker = new BackgroundWorker();

            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            maxItems = getMaxCount();

            pbCalculationProgress.Minimum = 1;
            pbCalculationProgress.Maximum = 100;

            child_steps.Text = "Starting...";
            worker.RunWorkerAsync(maxItems);
        }

        private void initializeJellyFish()
        {
            String url = ConfigurationManager.AppSettings["URL"];

            String subsKey = ConfigurationManager.AppSettings["Subscriptionkey"];
            String apiKey = ConfigurationManager.AppSettings["APIMKey"];

            jf = new JellyFishMain(url, subsKey, apiKey);
        }

        private int getMaxCount()
        {
            int max_count = 0;
            foreach (TestCase tc in sortedList_selected_testcases)
            {
                max_count += tc.Itr;
            }
            return max_count;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
            this.Hide();
            this.Owner.Show();
        }
        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void startPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPaused)
            {
                isPaused = false;
                startPauseButton.Content = "Pause";
            }
            else
            {
                isPaused = true;
                startPauseButton.Content = "Continue";
            }
                      
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int? maxItems = e.Argument as int?;
            
            foreach(TestCase tc in sortedList_selected_testcases)
            {
                StepHandler sth = new StepHandler(tc, worker, e);
                sth.handleSteps();                
            }
        }
        

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                child_steps.Text = "Cancelled";
            }
            else
            {
                child_steps.Text = "Completed";
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (maxItems == 0) return;
            if(log_messege != null && log_messege.Length > 0)
            { 
                child_steps.AppendText("\n" + log_messege);
                log_messege = "";
            }
            
            child_steps.CaretIndex = child_steps.Text.Length;
            var rect = child_steps.GetRectFromCharacterIndex(child_steps.CaretIndex);
            child_steps.ScrollToHorizontalOffset(rect.Right);
            child_steps.Focus();

            int percent = e.ProgressPercentage;

            if (percent > 0)
            {
                pbCalculationProgress.Value = percent;
                labelPercentage.Content = pbCalculationProgress.Value + " %";
            }
        }
    }
   
}
