using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace JellyFish
{
    public class JSONParser
    {
        public List<Word> parseJSON(String jsonString, String keyWord)
        {
            List<Word> matchingWords = new List<Word>();

            //JSON json = new JSON();


            matchingWords.Add(new Word());

            return matchingWords;
        }

        public static Response getResponseJSON(string rawRsponse)
        {
            Response response = new Response();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(rawRsponse));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(response.GetType());
            response = ser.ReadObject(ms) as Response;
            ms.Close();
            return response;
        }

        public static List<Word> getMatchingWords(Response response, String text)
        {
            List<Word> words = new List<Word>();

            foreach (Region region in response.regions)
            {
                foreach (Line line in region.lines)
                {
                    foreach (Word word in line.words)
                    {
                        String obtained_word = word.text.ToLower();
                        String my_text = text.ToLower();                        
                        if (obtained_word.Equals(my_text))
                        {
                            String[] splitz = word.boundingBox.Split(',');
                            if (Int32.Parse(splitz[1]) < 50)
                            {
                                continue;
                            }
                            words.Add(word);
                        }
                    }
                }
            }
            return words;
        }

        internal static Coords getCoords(string fullResponse)
        {
            if(fullResponse==null || fullResponse.Length==0 || !fullResponse.Contains("-"))
            {
                return null;
            }
            else
            {
                String[] splitz = fullResponse.Split('-');
                int x = Int32.Parse(splitz[0]);
                int y = Int32.Parse(splitz[1]);
                int w = Int32.Parse(splitz[2]);
                int h = Int32.Parse(splitz[3]);

                return new Coords(x, y, w, h);
            }

        }
    }
}
