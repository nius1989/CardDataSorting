using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF_Generator
{
    class Stopword_Remover
    {
        string[] stopwords;
        public Stopword_Remover()
        {
            StreamReader reader = new StreamReader(@"Resource\Data\stop_words.txt");
            string nextLine = "";
            List<string> stopwordlist = new List<string>();
            PorterStemmer stemmer = new PorterStemmer();
            while ((nextLine = reader.ReadLine()) != null)
            {
                stopwordlist.Add(stemmer.stemTerm(nextLine.Trim()));
            }
            stopwords = stopwordlist.ToArray();
            reader.Close();
        }

        public Token[] RemoveStopwords(Token[] tokens)
        {
            foreach (Token t in tokens)
            {
                    foreach (String s in stopwords)
                    {
                        if (s.Equals(t.ProcessedContent))
                        {
                            t.WordType = Token.WORDTYPE.STOPWORD;
                            break;
                        }
                    }
            }
            return tokens;
        }
    }
}
