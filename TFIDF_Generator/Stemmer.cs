using LemmaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF_Generator
{
    class Stemmer
    {
        static ILemmatizer lmtz = new LemmatizerPrebuiltCompact(LemmaSharp.LanguagePrebuilt.English);
        public static Token[] Stem(Token[] tokens)
        {
            PorterStemmer stemmer = new PorterStemmer();
            foreach (Token t in tokens)
            {
                string word = t.OringinalContent.ToLower();
                string root = getRootForm(lmtz, word);
                t.ProcessedContent= stemmer.stemTerm(root).ToLower();
            }
            return tokens;
        }
        public static string GetRootForm(string word) {
            return getRootForm(lmtz, word);
        }
        private static string getRootForm(LemmaSharp.ILemmatizer lmtz, string word)
        {
            string wordLower = word.ToLower();
            string lemma = lmtz.Lemmatize(wordLower);
            return lemma;
        }
    }
}
