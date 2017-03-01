using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JellyFish
{
    public class KeyBoard
    {
        public static void pressAltShiftQ()
        {
            Console.WriteLine("b4 alt shift q");
            Thread.Sleep(2000);
            SendKeys.SendWait("%+q");
            Thread.Sleep(2000);
            Console.WriteLine("after alt shift q");
        }

        public static String copy()
        {
            Thread.Sleep(1000);
            SendKeys.SendWait("^(c)");
            String markersRaw = getClipboardText();
            Thread.Sleep(1000);
            return markersRaw;
        }

        public static String getClipboardText()
        {
            String returnText = null;
            
            Exception threadEx = null;
            Thread staThread = new Thread(
                delegate ()
                {
                    try
                    {
                        returnText = Clipboard.GetText(TextDataFormat.Text);
                    }

                    catch (Exception ex)
                    {
                        threadEx = ex;
                    }
                });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            return returnText;
        }
        public static void type(String text)
        {
            SendKeys.SendWait(text);
        }
    }
}
