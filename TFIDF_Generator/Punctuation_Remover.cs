using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF_Generator
{
    class Punctuation_Remover
    {
        public Token[] MarkPunc(Token[] tokens)
        {
            foreach (Token t in tokens)
            {
                if (t.OringinalContent.Length == 1)
                {
                    if (char.IsPunctuation((t.OringinalContent[0])) ||
                        t.OringinalContent[0] == ' ' ||
                        t.OringinalContent[0] == '\n' ||
                        t.OringinalContent[0] == '\r')
                    {

                        t.WordType = Token.WORDTYPE.PUNCTUATION;
                    }
                }
            }
            return tokens;
        }
    }
}
