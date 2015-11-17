using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Punctuation_Remover
    {
        public static String RemovePunctuation(String input)
        {
            String result = "";
            String noPunc = new string(input.Where(c => !char.IsPunctuation(c)).ToArray());
            result = noPunc;
            return result;
        }
    }
}
