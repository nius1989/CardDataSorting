using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF_Generator
{
    class Token
    {
        public enum WORDTYPE { DEFAULT,PUNCTUATION, STOPWORD,NUMBER, REGULAR }
        String oringinalContent;

        public String OringinalContent
        {
            get { return oringinalContent; }
            set { oringinalContent = value; }
        }
        String processedContent;

        public String ProcessedContent
        {
            get { return processedContent; }
            set { processedContent = value; }
        }
        double ranking = 0;

        public double Ranking
        {
            get { return ranking; }
            set { ranking = value; }
        }

        public WORDTYPE WordType
        {
            get
            {
                return wordType;
            }

            set
            {
                wordType = value;
            }
        }

        WORDTYPE wordType=WORDTYPE.DEFAULT;

    }
}
