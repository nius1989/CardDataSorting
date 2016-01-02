using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WordCloud
{
    class GlowList
    {
        Dictionary<string, GlowView> glowList = new Dictionary<string, GlowView>();

        public GlowView[] GetGlowViews()
        {
            return glowList.Values.ToArray();
        }
        public void AddGlowView(string identifier, GlowView glowView) {
            if (!glowList.ContainsKey(identifier)) {
                glowList.Add(identifier, glowView);
            }
        }

        public void UpdateGlowList(string[] keywordList, Point[] points) {
            ClearUserFactor();
            if (keywordList != null && keywordList.Length > 0 && points != null && points.Length == keywordList.Length)
            {
                int index = 0;
                foreach (string key in keywordList)
                {
                    GlowView gv = glowList[key];
                    gv.MoveTo(points[index].X, points[index].Y);
                    gv.Proportion(Graph.GetGlowSize(key, STATICS.MIN_GLOW_SIZE, STATICS.MAX_GLOW_SIZE),Graph.GetUserFactor(key));
                    index++;
                }
            }
        }
        public void ClearUserFactor() {
            foreach (GlowView gv in glowList.Values) {
                gv.ClearUserFactors();
            }
        }
    }
}
