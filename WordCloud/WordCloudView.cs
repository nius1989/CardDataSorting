using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WordCloud
{
    public class WordCloudView:Canvas
    {
        Controler controler;
        ForceDirectedCloudView forceDirectedCloud;
        GlowCloudView glowCloud;
        internal ForceDirectedCloudView ForceDirectedCloud
        {
            get
            {
                return forceDirectedCloud;
            }

            set
            {
                forceDirectedCloud = value;
            }
        }
        internal GlowCloudView GlowCloud
        {
            get
            {
                return glowCloud;
            }
        }
        public WordCloudView(double width, double height) {
            this.Width = width;
            this.Height = height;
            controler = new Controler(this);
            controler.SetScreenSize(width, height);
            controler.Initialize();
            glowCloud = new GlowCloudView(controler);
            ForceDirectedCloud = new ForceDirectedCloudView(controler);
            this.Children.Add(glowCloud);
            this.Children.Add(ForceDirectedCloud);
            controler.StartUIThread();
        }

        public Controler Controler
        {
            get
            {
                return controler;
            }
        }



        public void Quit() {
            Controler.Quit();
        }
    }
}
