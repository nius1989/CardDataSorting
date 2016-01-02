using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WordCloud
{
    public class Controler
    {
        WordCloudView wordCloudView;
        NodeList nodeList;
        GlowList glowList;
        Graph_Generator graphGenerator;
        internal NodeList NodeViewList
        {
            get
            {
                return nodeList;
            }

            set
            {
                nodeList = value;
            }
        }

        internal GlowList GlowList
        {
            get
            {
                return glowList;
            }

            set
            {
                glowList = value;
            }
        }
        public void UpdateMatrix(double[][] matrix)
        {
            Graph.SetFactorMatrix(matrix);
            graphGenerator.UpdateGraph(Graph.GetKeywords(), Graph.GetConMatrix());
        }
        internal Controler(WordCloudView controlerView)
        {
            this.wordCloudView = controlerView;
        }
        internal void Initialize()
        {
            Graph.Initialize(STATICS.FILE_LOC, STATICS.WORD_COUNT);
            nodeList = new NodeList();
            string[] wholeKeyword = Graph.GetAllKeywords();
            for (int i = 0; i < wholeKeyword.Length; i++)
            {
                NodeView node = new NodeView(wholeKeyword[i]);
                nodeList.AddNodeView(wholeKeyword[i], node);
            }
        }

        internal void Quit()
        {
            graphGenerator.Quit();
        }

        internal void SetScreenSize(double width, double height) {
            STATICS.SCREEN_WIDTH = width;
            STATICS.SCREEN_HEIGHT = height;
        }

        internal void StartUIThread()
        {
            graphGenerator = new Graph_Generator(this);
            graphGenerator.UpdateGraph(Graph.GetKeywords(), Graph.GetConMatrix());
            graphGenerator.StartGenGraph();
        }
        internal void UpdateNodes()
        {
            Point[] points=graphGenerator.GetPoints(STATICS.SCREEN_WIDTH, STATICS.SCREEN_HEIGHT);
            String[] keywordList = Graph.GetKeywords();
            nodeList.UpdateNodeList(keywordList,points);
            wordCloudView.ForceDirectedCloud.UpdateNodes(nodeList.GetNodeViews());
            //nodeList.UpdateNode(points);
            //glowList.UpdatePosition(points);
        }
        private void UpdateNodeList(String[] keywordList, Point[] points)
        {
            int index = 0;
            foreach (string keyword in keywordList) {
                //nodeList.MoveNode(keyword, points[index]);
                index++;
            }


            //glowList = new GlowList();
            //for (int i = 0; i < graph.GetKeywords().Length; i++)
            //{
            //    double rank = graph.GetRanking(graph.GetKeywords()[i]);
            //    if (rank > 0)
            //    {
            //        GlowView glow = new GlowView();
            //        glow.Initialize(this.userNum);
            //        double glowSize = graph.GetGlowSize(graph.GetKeywords()[i], STATICS.MIN_GLOW_SIZE, STATICS.MAX_GLOW_SIZE);
            //        double[] proportions = new double[] { 1.0 / 3.0, 2.0 / 3.0, 1.0 };
            //        glow.Proportion(glowSize, proportions);
            //        glowList.AddGlowView(graph.GetKeywords()[i], glow);
            //    }
            //}

        }
    }
}
