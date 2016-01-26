using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WordCloud
{
    class GlowCloudView:Canvas
    {
        Controler controler;
        public GlowCloudView(Controler controler) {
            this.controler = controler;
            this.Width = STATICS.SCREEN_WIDTH;
            this.Height = STATICS.SCREEN_HEIGHT;
            UpdateGlows(controler.GlowList.GetGlowViews());
        }

        public void UpdateGlows(GlowView[] glowViews)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                List<GlowView> tobeRemoved = new List<GlowView>();
                foreach (System.Windows.UIElement ele in this.Children)
                {
                    GlowView gv = ele as GlowView;
                    if (gv.GetRanking() == 0)
                    {
                        tobeRemoved.Add(gv);
                    }
                }
                foreach (GlowView gv in tobeRemoved)
                {
                    this.Children.Remove(gv);
                }
                foreach (GlowView glow in glowViews)
                {
                    if (glow.GetRanking() > 0)
                    {
                        if (!this.Children.Contains(glow))
                        {
                            this.Children.Add(glow);
                        }
                    }
                }
                
            }), System.Windows.Threading.DispatcherPriority.Render);
        }
    }
}
