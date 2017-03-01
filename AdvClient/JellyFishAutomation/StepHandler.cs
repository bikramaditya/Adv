using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace HelliumClient
{
    internal class StepHandler
    {
        private DoWorkEventArgs e;
        private TestCase tc;
        private BackgroundWorker worker;
        private TestStepReader _reader;
        private int inc = 0;
        public StepHandler(TestCase tc, BackgroundWorker worker, DoWorkEventArgs e)
        {
            this.tc = tc;
            this.worker = worker;
            this.e = e;
            _reader = new TestStepReader(tc);
        }
        internal void handleSteps()
        {
            JellyFish.Mouse.moveAndClick(80, 10);

            inc = 0;

            int init_count = _reader.getInitStepCount();

            handleInitSteps();

            while (inc < init_count)
            {
                Thread.Sleep(100);
            }
            Thread.Sleep(3000);
            for(int i = 0; i < tc.Itr; i++)
            { 
                handleLoopSteps(i);
            }
        }
        private void handleInitSteps()
        {            
            List<TestStep> init_steps = _reader.getInitSteps();

            for (int i = 0; i < init_steps.Count ; ++i)
            {
                ProgressWindow.log_messege = "   Init->"+tc.TestCaseName + " - step - " + i;

                worker.ReportProgress(0);
                Thread.Sleep(100);

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                while (true)
                {
                    if (ProgressWindow.isPaused)
                    {

                        Thread.Sleep(10);
                    }
                    else
                    {
                        break;
                    }
                }

                StepExecutor se = new StepExecutor(init_steps[i], worker,tc);

                se.execute(i);

                inc++;
                
                Thread.Sleep(100);
            }
        }

        private void handleLoopSteps(int itr)
        {
            List<TestStep> loop_steps = _reader.getLoopSteps();

            for (int i = 0; i < loop_steps.Count ; ++i)
            {
                ProgressWindow.log_messege = tc.TestCaseName + " - Iteration - " + itr + " of " + tc.Itr + " Step " + i + " of (" + loop_steps.Count + ")";
                
                worker.ReportProgress(0);
                Thread.Sleep(100);

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                while (true)
                {
                    if (ProgressWindow.isPaused)
                    {

                        Thread.Sleep(10);
                    }
                    else
                    {
                        break;
                    }
                }

                StepExecutor se = new StepExecutor(loop_steps[i], worker, tc);

                se.execute(i);                
            }
            ProgressWindow.ctr++;

            double perc = 100 * ProgressWindow.ctr / (double)ProgressWindow.maxItems;

            worker.ReportProgress((int)(perc));
            Thread.Sleep(100);
        }

        
    }
}