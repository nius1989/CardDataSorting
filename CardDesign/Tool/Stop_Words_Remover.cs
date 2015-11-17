using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    public class Stop_Words_Remover
    {
        String[] stopwords;
        public Stop_Words_Remover()
        {
            StreamReader reader = new StreamReader(@"Resource/Data/stop_words.txt");
            String nextLine = "";
            List<String> stopwordlist = new List<String>();
            while ((nextLine = reader.ReadLine()) != null)
            {
                stopwordlist.Add(nextLine.Trim());
            }
            stopwords = stopwordlist.ToArray();
            reader.Close();
        }

        public String RemoveStopwords(String input)
        {
            String result = input;
            foreach (String s in stopwords)
            {
                if (input.Equals(s))
                {
                    result = "STOPWORD";
                }
            }
            return result;
        }
    }
}
