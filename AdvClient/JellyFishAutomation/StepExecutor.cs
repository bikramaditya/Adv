using JellyFish;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HelliumClient
{
    class StepExecutor
    {
        private TestStep step;
        private TestCase testCase;
        private BackgroundWorker worker;
        
        public StepExecutor(TestStep testStep, BackgroundWorker worker, TestCase tc)
        {
            this.step = testStep;
            this.testCase = tc;
            this.worker = worker;
        }

        internal async void execute(int no)
        {
            ProgressWindow.log_messege = "-->" + testCase.TestCaseName + " - Step no:" + no + " -> " + step.text + "/" + step.altText;
            this.worker.ReportProgress(0);


            Thread.Sleep(step.timeTowait);

            if(step.scrollLines > 0 && step.scrollText!=null && step.scrollText.Length > 0)
            {
                JellyFishWrapperUtil.scroll(step);
            }
            

            Thread.Sleep(1000);

            Coords assert = null;

            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            // Do something.
            while (stopwatch.ElapsedMilliseconds < 60000 && assert == null)
            {
                assert = await JellyFishWrapperUtil.assert(step);
                Thread.Sleep(1000);
            }

            if (stopwatch.ElapsedMilliseconds > 60000)
            {
                MessageBoxResult result = MessageBox.Show("Step not found, is the page loaded ?\n" + step,
                                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // Stop timing.
            stopwatch.Stop();

            if (step.isCold)
            {
                JellyFishWrapperUtil.clearCache();
            }

            if (step.text.Length > 0)
            {
                JellyFishWrapperUtil.findAndclick(step);
            }

            if (step.textToType.Length > 0)
            {
                JellyFishWrapperUtil.type(step);
            }
            if (step.isMarker)
            {
                stopwatch = new Stopwatch();

                stopwatch.Start();

                while (stopwatch.ElapsedMilliseconds < 60000 && assert == null)
                {
                    assert = await JellyFishWrapperUtil.assertMarker(step);
                    Thread.Sleep(1000);
                }

                if (stopwatch.ElapsedMilliseconds > 60000)
                {
                    MessageBoxResult result = MessageBox.Show("Step not found, is the page loaded ? \n" + step,
                                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                stopwatch.Stop();

                JellyFishWrapperUtil.collectMarker();
            }
        }
    }
}
