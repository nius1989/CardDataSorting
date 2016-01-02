using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TFIDF_Generator
{
    class Tokeniser
    {

        public Token[] Partition(string input)
        {
            Regex r = new Regex("([\\t{}():;., \"“”\n])");
            List<Token> tokenList = new List<Token>();
            String stack = "";
            for (int i = 0; i < input.Length; i++) {
                if (r.IsMatch("" + input[i]))
                {
                    Token tokenPunc = new Token();
                    tokenPunc.OringinalContent = ""+input[i];
                    tokenList.Add(tokenPunc);
                    if (stack.Length > 0)
                    {
                        Token token = new Token();
                        token.OringinalContent = stack;
                        tokenList.Add(token);
                        stack = "";
                    }
                }
                else {
                    stack += input[i];
                }
            }
            return tokenList.ToArray();
        }
    }
}
