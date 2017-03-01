using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JellyFish
{
    public class AzureConnector
    {
        public static async Task<Coords> getTextFromAzure(byte[] image, String sub_image_path)
        {
            string fullResponse = "";
            try
            {
                /*
                MultipartForm form = new MultipartForm("http://localhost:8080/AdvantoWS/uploadFile");
                form.SetField("name", "file");
                form.SetField("filename", "azure_error.PNG");
                form.SetField("file", "azure_error.PNG");
                form.SendFile(@"C:\Users\bipadh\Pictures\azure_error.PNG");
                StringBuilder sb = form.ResponseText;
                String response = sb.ToString();
                */

                //String fileName = @"C:\Users\bipadh\Pictures\azure_error.PNG";
                HttpResponseMessage response;

                try
                {
                    using (
                    var client = new HttpClient())
                    using (var form = new MultipartFormDataContent())
                    {
                        //fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName, DispositionType = DispositionTypeNames.Attachment, Name = "file" };
                        form.Add(new StreamContent(new MemoryStream(image)), "file", "main_file.png");

                        form.Add(new StringContent(sub_image_path), String.Format("\"{0}\"", "name"));

                        // only for test purposes, for stable environment, use ApiRequest class.
                        response = client.PostAsync("http://localhost:8080/AdvantoWS/uploadFile", form).Result;

                        fullResponse = await response.Content.ReadAsStringAsync();
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //return JSONParser.getResponseJSON(fullResponse);
            return JSONParser.getCoords(fullResponse);
        }
    }
}
