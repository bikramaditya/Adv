using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using JellyFish;
using System.Threading;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace HelliumClient
{
    class JellyFishWrapperUtil
    {
        internal static void collectMarker()
        {
            try
            {
                if (MainWindow.tds.browser.ToLower().Contains("chrome") 
                    || MainWindow.tds.browser.ToLower().Contains("edge") 
                    || MainWindow.tds.browser.ToLower().Contains("firefox") 
                    || MainWindow.tds.browser.ToLower().Contains("internet"))
                {
                    ProgressWindow.log_messege = "\nWaiting to collect markers";
                    ProgressWindow.worker.ReportProgress(0);                    
                    String markerDelay = ConfigurationManager.AppSettings["markerDelay"];

                    int delay = Int32.Parse(markerDelay) / 2;

                    Thread.Sleep(delay);

                    highlightWindow();

                    Thread.Sleep(delay);

                    JellyFish.KeyBoard.pressAltShiftQ();
                    clickCopyButton();
                    String copiedText = KeyBoard.copy();
                    MainWindow.dao.parseAndStore(copiedText);
                    clickCloseButton();
                }               
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        internal static async void scroll(TestStep step)
        {
            if (step.scrollText.Length == 0)
            {
                return;
            }
            Coords assert = null;

            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            // Do something.
            while (stopwatch.ElapsedMilliseconds < 60000 && assert == null)
            {
                assert = await JellyFishWrapperUtil.assertScroll(step);
                Thread.Sleep(500);
                ProgressWindow.log_messege = "Waiting to scroll over \"" + step.scrollText + "\"";
                ProgressWindow.worker.ReportProgress(0);
                while (true)
                {
                    Thread.Sleep(500);
                    if (!ProgressWindow.isPaused)
                    {
                        break;
                    }
                }
            }

            if (stopwatch.ElapsedMilliseconds > 60000)
            {
                stopwatch.Stop();
                MessageBoxResult result = MessageBox.Show("Didn't find " + step.scrollText + ".\nPlease check if page is loaded",
                                            "Help", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                stopwatch.Stop();

                
                int x = assert.X;
                int y = assert.Y;
                
                int w = assert.W;
                int h = assert.H;

                JellyFish.Mouse.moveAndScroll(x + w / 2 + (w * step.xAltOffset / 100), y + h / 2 + (h * step.yAltOffset / 100), step.scrollLines);
            }
        }

        private static async Task<Coords> assertScroll(TestStep step)
        {
            byte[] image = JellyFish.ScreenClipper.CaptureScreen();
            Coords response = await AzureConnector.getTextFromAzure(image, step.scrollText);
            
            return response;
        }

        public static void highlightWindow()
        {
            try
            {
                Thread.Sleep(1000);
                
                JellyFish.Mouse.moveAndClick(50, 7);
            }
            catch (Exception)
            {
            }
        }
        private static async void clickCloseButton()
        {
            TestStep closeStep = new TestStep();
            closeStep.text = "close";
            closeStep.occurance = 1;

            closeStep.altText = "copy";
            closeStep.altOccurance = 1;
            closeStep.xAltOffset = 700;

            Coords assert = null;

            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            // Do something.
            while (stopwatch.ElapsedMilliseconds < 60000 && assert == null)
            {
                assert = await JellyFishWrapperUtil.assert(closeStep);
                Thread.Sleep(1000);
            }

            if (stopwatch.ElapsedMilliseconds > 60000)
            {
                stopwatch.Stop();
                MessageBoxResult result = MessageBox.Show("Unable to close marker window.\nPlease close manually",
                                            "Help", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                findAndclick(closeStep);
            }
        }
        private static void clickCopyButton()
        {
            TestStep copyStep = new TestStep();
            copyStep.text = "copy";
            copyStep.occurance = 1;
            findAndclick(copyStep);
        }

        internal static void clearCache()
        {
            ProgressWindow.log_messege = "\nClearing cache";
            ProgressWindow.worker.ReportProgress(0);
            Thread.Sleep(1000);
            try
            {
                if (MainWindow.tds.browser.ToLower().Contains("chrome"))
                {
                    String cacheClearLocationChrome = ConfigurationManager.AppSettings["cacheClearLocationChrome"];
                    int x = Int32.Parse(cacheClearLocationChrome.Split(',')[0]);
                    int y = Int32.Parse(cacheClearLocationChrome.Split(',')[1]);
                    JellyFish.Mouse.moveAndClick(x, y);
                }  
                else if (MainWindow.tds.browser.ToLower().Contains("edge"))
                {
                    String cacheClearLocationChrome = ConfigurationManager.AppSettings["cacheClearLocationEdge"];
                    String appdataFolder = Environment.GetEnvironmentVariable("LocalAppData");
                    String path = appdataFolder + cacheClearLocationChrome;

                    List<String> directories = CustomSearcher.GetDirectories(path);
                    foreach(String dir in directories)
                    {
                        if (Directory.Exists(dir))
                        {
                            String[] splitz = dir.Split('\\');
                            try
                            {
                                String subdir = splitz[splitz.Length -1] ;

                                if (subdir.StartsWith("#"))
                                {
                                    Directory.Delete(dir,true);
                                }
                            } catch (Exception e) { Console.WriteLine(e.Message); }
                        }
                    }
                }
                else if (MainWindow.tds.browser.ToLower().Contains("internet"))
                {
                    //String command = "RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8";
                    try
                    {
                        Process.Start("RunDll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 8");
                       
                    }
                    catch (Exception objException)
                    {
                        Console.WriteLine(objException.Message);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            ProgressWindow.log_messege = "\nCache cleared";
            ProgressWindow.worker.ReportProgress(0);
            Thread.Sleep(3000);
        }
        public class CustomSearcher
        {
            public static List<string> GetDirectories(string path, string searchPattern = "*",
                SearchOption searchOption = SearchOption.TopDirectoryOnly)
            {
                if (searchOption == SearchOption.TopDirectoryOnly)
                    return Directory.GetDirectories(path, searchPattern).ToList();

                var directories = new List<string>(GetDirectories(path, searchPattern));

                for (var i = 0; i < directories.Count; i++)
                    directories.AddRange(GetDirectories(directories[i], searchPattern));

                return directories;
            }

            private static List<string> GetDirectories(string path, string searchPattern)
            {
                try
                {
                    return Directory.GetDirectories(path, searchPattern).ToList();
                }
                catch (UnauthorizedAccessException)
                {
                    return new List<string>();
                }
            }
        }
        internal static async void findAndclick(TestStep step)
        {
            byte[] image = JellyFish.ScreenClipper.CaptureScreen();
            Coords response = await AzureConnector.getTextFromAzure(image, step.text);

            int x = response.X;
            int y = response.Y;

            int w = response.W;
            int h = response.H;

            JellyFish.Mouse.moveAndClick(x + w / 2 + (w * step.xAltOffset / 100), y + h / 2 + (h * step.yAltOffset / 100));
        }
        internal static async Task<Coords> assertMarker(TestStep step)
        {
            byte[] image = JellyFish.ScreenClipper.CaptureScreen();
            Coords response = await AzureConnector.getTextFromAzure(image, step.markerAssert);

            return response;
        }
        internal static async Task<Coords> assert(TestStep step)
        {
            if (step.text.Length == 0 && step.altText.Length == 0 && step.assert.Length == 0)
            {
                return null;
            }
            byte[] image = JellyFish.ScreenClipper.CaptureScreen();

            Coords response = await AzureConnector.getTextFromAzure(image, step.text);

            if (response == null)
            {
                Console.WriteLine("Waiting for pageload: for text :" + step.assert);
                ProgressWindow.log_messege = "Waiting 1 sec more for pageload...";
                ProgressWindow.worker.ReportProgress(0);

                while (true)
                {
                    Thread.Sleep(500);
                    if (!ProgressWindow.isPaused)
                    {
                        break;
                    }
                }
            }
            return response;
        }
        internal static void type(TestStep step)
        {
            KeyBoard.type(step.textToType);
        }
    }
}
