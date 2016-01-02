using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WordCloud
{
    class GlowList
    {
        Dictionary<string, GlowView> glowViewList = new Dictionary<string, GlowView>();

        internal Dictionary<string, GlowView> NodeViewList
        {
            get
            {
                return glowViewList;
            }
        }
        public GlowView[] GetGlowViews()
        {
            return glowViewList.Values.ToArray();
        }
        public void AddGlowView(string identifier, GlowView glowView) {
            if (!glowViewList.ContainsKey(identifier)) {
                glowViewList.Add(identifier, glowView);
            }
        }

        public void UpdatePosition(Point[] list) {
            int i = 0;
            foreach (GlowView gv in glowViewList.Values)
            {
                gv.MoveTo(list[i].X, list[i].Y);
                i++;
            }
        }

        public void UpdateRadius(double[] sizes, double[][] list)
        {
            int i = 0;
            foreach (GlowView gv in glowViewList.Values)
            {
                gv.Proportion(sizes[i], list[i]);
                i++;
            }
        }
    }
}
