using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF_Generator
{
     public class TFIDF_Documents
    {
        Dictionary<String, Document> documentList = new Dictionary<string, Document>();
        Dictionary<string, double> inverseDocumentFrequency = new Dictionary<string, double>();
        String loc = "newsDocs.txt";
        TFIDF_GRAPH graph = new TFIDF_GRAPH();

        public void SetFactorMatrix(double[][] matrix) {
            graph.SetFactorMatrix(matrix);
        }
        public void SetWordNumber(int count)
        {
            graph.SetWordNumber(count);
        }

        public double[][] GetConnectedGraph()
        {
            return graph.GetTokenGraph();
        }

        public string[] GetGraphKeys()
        {
            return graph.Keywords;
        }

        public string GetOriginalKey(String key)
        {
            return graph.OriginWord(key);
        }

        public double GetRanking(String key)
        {
            return graph.GetRanking(key);
        }

        public double[] GetUserFactor(String key) {
            return graph.GetUserFactor(key);
        }

        public string[] GetAllKeywords() {
            return graph.GetAllPossibleKeywords();
        }
        public void Load(String loc)
        {
            this.loc = loc;
            documentList = new Dictionary<string, Document>();
            String nextLine = "";
            StreamReader reader = new StreamReader(loc);
            int count = 0;
            int newsIndex = 0;
            while ((nextLine = reader.ReadLine()) != null)
            {
                Document newsData = JsonConvert.DeserializeObject<Document>(nextLine);
                TFIDF_Document tiarticle = new TFIDF_Document();
                tiarticle.Title = newsData.Title;
                tiarticle.Author = newsData.Author;
                tiarticle.Time = newsData.Time;
                tiarticle.Tag = newsData.Tag;
                tiarticle.Content = newsData.Content;
                tiarticle.NewsID = "" + newsIndex++;
                documentList.Add(""+count, tiarticle);
                count++;
            }
            reader.Close();
        }

        private void CalIDF()
        {
            foreach (Document art in documentList.Values)
            {
                Dictionary<string, double> termFrequency = (art as TFIDF_Document).TermFrequency;
                foreach (KeyValuePair<string, double> pair in termFrequency)
                {
                    if (inverseDocumentFrequency.Keys.Contains(pair.Key))
                    {
                        inverseDocumentFrequency[pair.Key] += pair.Value;
                    }
                    else {
                        inverseDocumentFrequency.Add(pair.Key, pair.Value);
                    }
                }
            }
            foreach (String key in inverseDocumentFrequency.Keys.ToArray())
            {
                inverseDocumentFrequency[key] = Math.Log(documentList.Count / (inverseDocumentFrequency[key] + 1));
            }
        }
        public void Process()
        {

            foreach (TFIDF_Document art in documentList.Values)
            {
                Tokeniser tokeniser = new Tokeniser();
                Token[] tokens = tokeniser.Partition(art.Content);

                Punctuation_Remover pRemover = new Punctuation_Remover();
                tokens = pRemover.MarkPunc(tokens);
                
                tokens = Stemmer.Stem(tokens);

                Stopword_Remover swRemover = new Stopword_Remover();
                tokens = swRemover.RemoveStopwords(tokens);

                Number_Remover nRemover = new Number_Remover();
                tokens = nRemover.RemoveNumber(tokens);

                foreach (Token t in tokens)
                {
                    if (t.WordType == Token.WORDTYPE.DEFAULT)
                    {
                        t.WordType = Token.WORDTYPE.REGULAR;
                    }
                }
                art.SetToken(tokens);
                art.CalTF();
            }
            CalIDF();
            foreach (TFIDF_Document art in documentList.Values)
            {
                art.CalTFIDF(inverseDocumentFrequency);         
            }
            graph.InitializeGraph(documentList.Values.ToArray());
        }       
    }
}
