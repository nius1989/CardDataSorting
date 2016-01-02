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
            AddGlows(controler.GlowList.GetGlowViews());
        }

        private void AddGlows(GlowView[] glowViews)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                foreach (GlowView glow in glowViews)
                {
                    this.Children.Add(glow);
                }
            }));
        }
    }
}
