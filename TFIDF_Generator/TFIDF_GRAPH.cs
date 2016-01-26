using LemmaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF_Generator
{
    class TFIDF_GRAPH
    {
        private class Node
        {
            internal double ranking=0;
            internal double[] rankingList;//Save rankings of all documents, sum up to *ranking*
            internal double[] userFactor;
            public Node Copy() {
                Node n = new Node();
                n.ranking = this.ranking;
                n.rankingList = new double[rankingList.Length];
                n.userFactor = new double[userFactor.Length];
                Array.Copy(rankingList, n.rankingList, rankingList.Length);
                Array.Copy(userFactor, n.userFactor, userFactor.Length);
                return n;
            }
        }
        double[][] factorMatrix=new double[4][];
        Dictionary<string, Node> mergedList = new Dictionary<string, Node>();//List with 1000 significant nodes
        Dictionary<string, Node> graphList = new Dictionary<string, Node>();//List with matrixSize nodes
        Dictionary<string, string> tokenOringinalFormList = new Dictionary<string, string>();
        string[] keywords;//selected #matrixSize words
        double[][] tokenGraph;//Connection between #matrixSize nodes
        Document[] docs;
        int matrixSize = 40;
        readonly int BUFFER_SIZE=500;

        public string[] Keywords
        {
            get
            {
                return keywords;
            }
        }
        //<summary>Initialize the merged list</summary>
        internal void InitializeGraph(Document[] documents)
        {
            this.docs = documents;
            for (int i = 0; i < 4; i++) {
                factorMatrix[i] = new double[documents.Length];
            }
            MergeList();
        }
        //<summary>Call this when update the TFIDF_Graph</summary>
        internal void SetFactorMatrix(double[][] mtx) {
            this.factorMatrix = mtx;
            UpdateMergedList();
            GenGraphList();
            LinkSharedWords();
            LinkMaxWord();
            NormalizeGraph();
        }
        internal void SetWordNumber(int count) {
            this.matrixSize = count;
        }
        internal string OriginWord(string key)
        {
            return tokenOringinalFormList[key];
        }

        internal double GetRanking(String key)
        {
            if (graphList.ContainsKey(key))
                return graphList[key].ranking;
            else
                return 0;
        }

        internal double[][] GetTokenGraph() {
            return tokenGraph;
        }
        internal double[] GetUserFactor(String key) {
            if (graphList.ContainsKey(key))
                return graphList[key].userFactor;
            else
                return new double[] { 0, 0, 0, 0 };
        }

        internal string[] GetAllPossibleKeywords() {
            return mergedList.Keys.ToArray();
        }
        private void NormalizeGraph()
        {
            double sum = 0;
            foreach (double[] r in tokenGraph) {
                sum += r.Sum();
            }
            double denominator = sum* 2;
            if (denominator > 0)
            {
                for (int i = 0; i < tokenGraph.Length; i++)
                {
                    for (int j = 0; j < tokenGraph[i].Length; j++)
                    {
                        tokenGraph[i][j] /= denominator;
                    }
                }
            }
            denominator = 0;
            foreach (string key in keywords) {
                denominator += graphList[key].ranking;
            }
            if (denominator > 0)
            {
                foreach (KeyValuePair<string, Node> pair in graphList)
                {
                    pair.Value.ranking /= denominator;
                    double maxFactor = pair.Value.userFactor.Max();
                    if (maxFactor > 0)
                    {
                        for (int i = 0; i < pair.Value.userFactor.Length; i++)
                        {
                            pair.Value.userFactor[i] /= maxFactor;
                        }
                    }
                }
            }
        }


        private void LinkMaxWord()
        {
            string[] sigWords = new string[docs.Length];
            int index = 0;
            foreach (TFIDF_Document doc in docs) {
                double max = double.MinValue;
                string maxKey = "";
                foreach (string key in Keywords) {
                    if (doc.HasToken(key)) {
                        if (graphList[key].ranking > max) {
                            max = graphList[key].ranking;
                            maxKey = key;
                        }
                    }
                }
                sigWords[index] = maxKey;
                index++;
            }
            foreach (string sig1 in sigWords)
            {
                int i = GetIndexInGraph(sig1);
                if (sig1.Length > 0)
                {
                    foreach (string sig2 in sigWords)
                    {
                        if (sig2.Length > 0 && !sig1.Equals(sig2))
                        {
                            int j = GetIndexInGraph(sig2);
                            if (tokenGraph[i][j] == 0)
                                tokenGraph[i][j] = (graphList[sig1].ranking + graphList[sig2].ranking) * 2;
                        }
                    }
                }
            }
        }

        private int GetIndexInGraph(string key)
        {
            int index=-1;
            for (int i = 0; i < Keywords.Length; i++)
            {
                if (Keywords[i].Equals(key))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private void LinkSharedWords()
        {
            int keywordNum = keywords.Length;
            if (keywordNum > 0)
            {
                    tokenGraph = new double[keywordNum][];
                    for (int m = 0; m < keywordNum; m++)
                    {
                        tokenGraph[m] = new double[keywordNum];
                    }
                int i = 0, j = 0;
                foreach (string k1 in keywords)
                {
                    j = 0;
                    foreach (string k2 in keywords)
                    {
                        if (!k1.Equals(k2))
                        {
                            if (IsTokenConnected(k1, k2))
                            {
                                tokenGraph[i][j] = graphList[k1].ranking + graphList[k2].ranking;
                                break;
                            }
                        }
                        j++;
                    }
                    i++;
                }
            }
        }

        private bool IsTokenConnected(string k1, string k2)
        {
            foreach (TFIDF_Document d in docs) {
                if (d.HasToken(k1) && d.HasToken(k2)) {
                    return true;
                }
            }
            return false;
        }

        private void GenGraphList()
        {
            var tokenWeight = mergedList.OrderByDescending(pair => pair.Value.ranking).Take(matrixSize);
            graphList.Clear();
            List<String> tempKeyList = new List<string>();
            foreach (KeyValuePair<string, Node> pair in tokenWeight)
            {
                if (pair.Value.ranking > 0)
                {
                    graphList.Add(pair.Key, pair.Value.Copy());
                    tempKeyList.Add(pair.Key);
                }
            }
            keywords = tempKeyList.ToArray();
        }
        private void UpdateMergedList() {
            foreach (Node n in mergedList.Values)
            {
                for (int i = 0; i < n.userFactor.Length; i++)
                {
                    n.userFactor[i] = 0;
                }
                for (int j = 0; j < n.userFactor.Length; j++)
                {
                    for (int i = 0; i < n.rankingList.Length; i++)
                    {
                        n.userFactor[j] += factorMatrix[j][i]*n.rankingList[i];
                    }
                }
            }
            foreach (Node n in mergedList.Values)
            {
                n.ranking = n.userFactor.Max();
            }
        }
        private void MergeList()
        {
            mergedList.Clear();
            tokenOringinalFormList.Clear();
            for (int i = 0; i < docs.Length; i++)
            {
                TFIDF_Document tfidDoc = docs[i] as TFIDF_Document;
                foreach (Token t in tfidDoc.Tokens)
                {
                    if (mergedList.ContainsKey(t.ProcessedContent))
                    {
                        mergedList[t.ProcessedContent].ranking += t.Ranking;
                        mergedList[t.ProcessedContent].rankingList[i] = t.Ranking;
                    }
                    else {
                        Node node = new Node();
                        node.rankingList = new double[docs.Length];
                        node.userFactor = new double[4];
                        node.ranking = t.Ranking;
                        node.rankingList[i] = t.Ranking;
                        mergedList.Add(t.ProcessedContent, node);
                        string rootWord = Stemmer.GetRootForm(t.OringinalContent);
                        tokenOringinalFormList.Add(t.ProcessedContent, rootWord);
                    }
                }
            }
            var tokenWeight = mergedList.OrderByDescending(pair => pair.Value.ranking).Take(BUFFER_SIZE);
            Dictionary<string, Node> tempList = new Dictionary<string, Node>();
            foreach (KeyValuePair<string, Node> pair in tokenWeight)
            {
                tempList.Add(pair.Key, pair.Value);
            }
            mergedList.Clear();
            mergedList = tempList;
        }
    }
}
