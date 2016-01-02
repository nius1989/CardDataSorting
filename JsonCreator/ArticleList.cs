using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFIDF_Generator;

namespace JsonCreator
{
    class ArticleList
    {
        Dictionary<String, Article> list = new Dictionary<string, Article>();
        Dictionary<string, double> inverseDocumentFrequency = new Dictionary<string, double>();
        String loc = "newsDocs.txt";
        public String GetListTitle()
        {
            String titles = "";
            foreach (Article art in list.Values)
            {
                titles += art.Title + "\n";
            }
            return titles;
        }
        public void Add(Article article)
        {
            if (!list.Keys.Contains(article.Title))
            {
                list.Add(article.Title, article);
            }
        }

        public void Save()
        {
            StreamWriter streamWriter = new StreamWriter(loc);
            foreach (Article art in list.Values)
            {
                String jstr = JsonConvert.SerializeObject(art);
                streamWriter.WriteLine(jstr);
            }
            streamWriter.Flush();
            streamWriter.Close();
        }

        public void Load()
        {
            String filedir = Path.Combine(Environment.CurrentDirectory, loc);
            if (!File.Exists(filedir))
            {
                File.Create(filedir);
                return;
            }

            list = new Dictionary<string, Article>();
            String nextLine = "";
            StreamReader reader = new StreamReader(loc);
            int count = 0;
            while ((nextLine = reader.ReadLine()) != null)
            {
                Article newsData = JsonConvert.DeserializeObject<Article>(nextLine);
                list.Add(newsData.Title, newsData);
                count++;
            }
            reader.Close();
        }
    }
}
