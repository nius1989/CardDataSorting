using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF_Generator
{
    class TFIDF_Document : Document
    {
        Token[] tokens;
        Dictionary<string, double> termFrequency = new Dictionary<string, double>();
        string newsID="";
        double[] userFactors=new double[] { 1,1,1 };
        public Dictionary<string, double> TermFrequency
        {
            get
            {
                return termFrequency;
            }

            set
            {
                termFrequency = value;
            }
        }

        internal Token[] Tokens
        {
            get
            {
                return tokens;
            }

            set
            {
                tokens = value;
            }
        }

        public string NewsID
        {
            get
            {
                return newsID;
            }

            set
            {
                newsID = value;
            }
        }
        public void SetUserFacotrs(double[] factors) {
            this.userFactors = factors;
        }
        public void SetToken(Token[] tokens)
        {
            this.Tokens = tokens;
        }
        public void CalTF()
        {
            foreach (Token token in Tokens)
            {
                if (token.WordType == Token.WORDTYPE.REGULAR)
                {
                    if (!TermFrequency.ContainsKey(token.ProcessedContent))
                    {
                        TermFrequency.Add(token.ProcessedContent, 1);
                    }
                    else
                    {
                        TermFrequency[token.ProcessedContent]++;
                    }
                }
            }
        }
        public void CalTFIDF(Dictionary<string, double> inverseDocumentFrequency)
        {
            foreach (Token token in Tokens)
            {
                if (token.WordType == Token.WORDTYPE.REGULAR)
                {
                    token.Ranking = termFrequency[token.ProcessedContent] * inverseDocumentFrequency[token.ProcessedContent];
                    //token.Ranking = termFrequency[token.ProcessedContent];//Term Frequency Only
                }
            }
        }
        public bool HasToken(String token) {
            foreach (Token t in tokens) {
                if (t.ProcessedContent.Equals(token)) {
                    return true;
                }
            }
            return false;
        }
    }
}
