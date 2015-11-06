using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Data_Loading_Template
    {
        String reviewText;

        public String ReviewText
        {
            get { return reviewText; }
            set { reviewText = value; }
        }
        String reviewerID;

        public String ReviewerID
        {
            get { return reviewerID; }
            set { reviewerID = value; }
        }
        long unixReviewTime;

        public long UnixReviewTime
        {
            get { return unixReviewTime; }
            set { unixReviewTime = value; }
        }
        String reviewName;

        public String ReviewName
        {
            get { return reviewName; }
            set { reviewName = value; }
        }
        double overall;

        public double Overall
        {
            get { return overall; }
            set { overall = value; }
        }
        String asin;

        public String Asin
        {
            get { return asin; }
            set { asin = value; }
        }
        String summary;

        public String Summary
        {
            get { return summary; }
            set { summary = value; }
        }
        int[] helpful;

        public int[] Helpful
        {
            get { return helpful; }
            set { helpful = value; }
        }
        String reviewTime;

        public String ReviewTime
        {
            get { return reviewTime; }
            set { reviewTime = value; }
        }
    }
}
