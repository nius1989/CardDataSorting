using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFIDF_Generator;

namespace WordCloud
{
    class Graph
    {
        static TFIDF_Documents tfidfDocs = new TFIDF_Documents();
        static double minRank=double.MaxValue;
        static double maxRank =double.MinValue;

        internal static void Initialize(String loc,int number) {
            String filedir = Path.Combine(Environment.CurrentDirectory, loc);
            tfidfDocs.SetWordNumber(number);
            tfidfDocs.Load(filedir);
            tfidfDocs.Process();
        }
        internal static void SetFactorMatrix(double[][] matrix) {
            tfidfDocs.SetFactorMatrix(matrix);
            foreach (string key in tfidfDocs.GetGraphKeys())
            {
                double rank = tfidfDocs.GetRanking(key);
                if (rank < minRank)
                {
                    minRank = rank;
                }
                if (rank > maxRank)
                {
                    maxRank = rank;
                }
            }
        }
        internal static String GetOriginalWord(String key) {
            return tfidfDocs.GetOriginalKey(key);
        }
        internal static double[] GetUserFactor(String key)
        {
            return tfidfDocs.GetUserFactor(key);
        }
        internal static double GetRanking(String key) {
            return tfidfDocs.GetRanking(key);
        }

        internal static string[] GetKeywords()
        {
            return tfidfDocs.GetGraphKeys();
        }

        internal static double[][] GetConMatrix()
        {
            return tfidfDocs.GetConnectedGraph();
        }

        internal static string[] GetAllKeywords() {
            return tfidfDocs.GetAllKeywords();
        }
        internal static double GetFontSize(double ranking, double minFont, double maxFont) {
            if (maxRank == minRank) {
                return 1;
            }
            double a = (maxFont - minFont) / (maxRank - minRank);
            double b = minFont - a * minRank;
            double font = a * ranking + b;
            if (font < 1) {
                font = 1;
            }
            return font;
        }

        internal static double GetGlowSize(String key, double minRadius, double maxRadiu)
        {
            if (maxRank == minRank)
            {
                return 1;
            }
            double a = (maxRadiu - minRadius) / (maxRank - minRank);
            double b = minRadius - a * minRank;
            double rank = tfidfDocs.GetRanking(key);
            return a * rank + b;
        }
    }
}
