using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Word_Frequency
    {
        static Dictionary<String, double> idf = new Dictionary<string, double>();
        static Word_Frequency()
        {
            StreamReader reader = new StreamReader(@"Resource\Data\frequency.txt");
            String nextLine = "";
            while ((nextLine = reader.ReadLine()) != null)
            {
                String[] strs = nextLine.Split(',');
                idf.Add(strs[0], double.Parse(strs[1]));
            }
        }

        public static void Rank(My_Doc data) {
            Dictionary<String, double> tempDic = new Dictionary<string, double>();
            foreach (My_Word word in data.Content)
            {
                if (word.ProcessedContent != "STOPWORD" && word.ProcessedContent.Length > 0)
                {
                    if (!tempDic.ContainsKey(word.ProcessedContent))
                    {
                        tempDic.Add(word.ProcessedContent, 1);
                    }
                    else
                    {
                        tempDic[word.ProcessedContent]++;
                    }
                }
            }
            foreach (My_Word word in data.Content)
            {
                if (word.ProcessedContent != "STOPWORD" && word.ProcessedContent.Length > 0)
                {
                    word.Ranking = tempDic[word.ProcessedContent] * idf[word.ProcessedContent];                    
                }
                else {
                    word.Ranking = 0;
                }
            }
            List<My_Word> helpList = new List<My_Word>();
            for (int i = 0; i < data.Content.Length; i++) {
                helpList.Add(data.Content[i]);
            }
            helpList.Sort(delegate(My_Word a, My_Word b)
            {
                if (a.Ranking > b.Ranking) return -1;
                else if (a.Ranking == b.Ranking)
                {
                    if (a.OringinalContent.Length > b.OringinalContent.Length) return -1;
                    else return 1;
                }
                else return 1;
            });
            int t = 1;
            foreach (My_Word wm in helpList) {
                wm.Index = t;
                t++;
            }
        }
    }
}
