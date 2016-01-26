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
            graphGenerator.IsRunning = false;
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
            glowList = new GlowList();
            string[] wholeKeyword = Graph.GetAllKeywords();
            for (int i = 0; i < wholeKeyword.Length; i++)
            {
                NodeView node = new NodeView(wholeKeyword[i]);
                nodeList.AddNodeView(wholeKeyword[i], node);
                GlowView glow = new GlowView(wholeKeyword[i]);
                glowList.AddGlowView(wholeKeyword[i], glow);
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
            //points and keywordList are one-to-one mapped.
            Point[] points=graphGenerator.GetPoints(STATICS.SCREEN_WIDTH, STATICS.SCREEN_HEIGHT);
            String[] keywordList = Graph.GetKeywords();
            nodeList.UpdateNodeList(keywordList,points);
            glowList.UpdateGlowList(keywordList,points);
            wordCloudView.ForceDirectedCloud.UpdateNodes(nodeList.GetNodeViews());
            wordCloudView.GlowCloud.UpdateGlows(glowList.GetGlowViews());
        }        
    }
}
