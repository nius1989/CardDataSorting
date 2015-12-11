using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class My_Paper
    {
        String title;

        public String Title
        {
            get { return title; }
            set { title = value; }
        }
        Author[] author;

        public Author[] Author
        {
            get { return author; }
            set { author = value; }
        }
        String doi;

        public String Doi
        {
            get { return doi; }
            set { doi = value; }
        }
        String abstractText;

        public String AbstractText
        {
            get { return abstractText; }
            set { abstractText = value; }
        }
        String page;

        public String Page
        {
            get { return page; }
            set { page = value; }
        }
        String year;

        public String Year
        {
            get { return year; }
            set { year = value; }
        }
    }
}
