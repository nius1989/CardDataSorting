using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class My_Doc
    {
        My_Word[] content;

        internal My_Word[] Content
        {
            get { return content; }
        }

        public void SetContent(My_Word[] textWords)
        {
            content = textWords;
            Word_Frequency.Rank(this);
        }
        public String GetOringinalStringContent()
        {
            String result = "";
            foreach (My_Word tw in content)
            {
                if (tw.IsVisible)
                {
                    result += tw.OringinalContent + " ";
                }
            }
            return result.TrimEnd();
        }
        public String GetProcessedStringContent()
        {
            String result = "";
            foreach (My_Word tw in content)
            {
                if (tw.IsVisible)
                {
                    result += tw.ProcessedContent + " ";
                }
            }
            return result.TrimEnd();
        }
    }
}
