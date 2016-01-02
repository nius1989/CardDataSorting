using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WordCloud
{
    class NodeList
    {
        Dictionary<string, NodeView> nodeViewList = new Dictionary<string, NodeView>();//Include all 1000 keywords

        public void AddNodeView(string identifier, NodeView nodeView) {
            if (!nodeViewList.ContainsKey(identifier)) {
                nodeViewList.Add(identifier, nodeView);
            }
        }
        public NodeView[] GetNodeViews() {
            return nodeViewList.Values.ToArray();
        }
        

        internal void UpdateNodeList(string[] keywordList, Point[] points)
        {
            ClearRankings();
            if (keywordList != null && keywordList.Length > 0 && points != null && points.Length == keywordList.Length)
            {
                int index = 0;
                foreach (string keyword in keywordList)
                {
                    NodeView nv = nodeViewList[keyword];
                    nv.MoveTo(points[index].X, points[index].Y);
                    nv.SetRanking(Graph.GetRanking(keyword));
                    index++;
                }
            }
        }
        internal void ClearRankings() {
            foreach (KeyValuePair<string, NodeView> pair in nodeViewList) {
                pair.Value.SetRanking(0);
            }
        }
    }
}
