using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TFIDF_Generator
{
    class Number_Remover
    {
        public Token[] RemoveNumber(Token[] tokens) {
            foreach (Token t in tokens)
            {
                if (!Regex.IsMatch(t.ProcessedContent, @"[a-z]")||t.ProcessedContent.Length==0||t.ProcessedContent.Equals(" ")) {
                    t.WordType = Token.WORDTYPE.NUMBER;
                }
            }
            return tokens;
        }
    }
}
