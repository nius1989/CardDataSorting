using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class My_Word
    {
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

        bool isVisible = true;

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        int index = -1;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
    }
}
