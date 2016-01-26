using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WordCloud
{
    class ForceDirectedCloudView : Canvas
    {
        Controler controler;
        public ForceDirectedCloudView(Controler contrler)
        {
            this.Width = STATICS.SCREEN_WIDTH;
            this.Height = STATICS.SCREEN_HEIGHT;
            this.controler = contrler;
        }

        public void UpdateNodes(NodeView[] nodeList)
        {
            Dispatcher.Invoke( new Action(() =>
            {
                List<NodeView> tobeRemoved = new List<NodeView>();
                foreach (System.Windows.UIElement ele in this.Children)
                {
                    NodeView nv = ele as NodeView;
                    if (nv.Ranking == 0)
                    {
                        tobeRemoved.Add(nv);
                    }
                }
                foreach (NodeView nv in tobeRemoved)
                {
                    this.Children.Remove(nv);
                }
                foreach (NodeView node in nodeList)
                {
                    if (node.Ranking > 0)
                    {
                        if (!this.Children.Contains(node))
                        {
                            this.Children.Add(node);
                        }
                    }
                }                
            }),System.Windows.Threading.DispatcherPriority.Render);
        }
    }
}
