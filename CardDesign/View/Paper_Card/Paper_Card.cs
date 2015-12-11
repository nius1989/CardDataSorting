using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CardDesign
{
    class Paper_Card : Card
    {
        My_Paper paper;
        internal My_Paper Paper
        {
            get { return paper; }
            set { paper = value; }
        }
        public override double CurrentScale
        {
            get
            {
                return base.CurrentScale;
            }
            set
            {
                int total = semanticLayers.Length;
                double interval = (STATICS.MAX_CARD_SCALE - STATICS.MIN_CARD_SCALE) / total;
                int newIndex = (int)((this.CurrentScale - STATICS.MIN_CARD_SCALE) / interval);
                if (newIndex != currentLayerIndex)
                {
                    currentLayerIndex = newIndex;
                    ChangeSize(newIndex);
                }
                base.CurrentScale = value;
            }
        }
        Layer_Base[] semanticLayers;
        Layer_Base currentLayer;
        int currentLayerIndex=0;
        public Paper_Card(Card_Controler controler) : base(controler) {
            List<Layer_Base> list = new List<Layer_Base>();
            list.Add(new Layer_One());
            list.Add(new Layer_Two());
            list.Add(new Layer_Three());
            list.Add(new Layer_One());
            semanticLayers = list.ToArray();
        }

        public override void InitializeCard(Color? maskColor, Point defaultPosi, double defaultDegree, double defaultScale, int zidx)
        {
            base.InitializeCard(maskColor, defaultPosi, defaultDegree, defaultScale, zidx);
            foreach (Layer_Base layer in semanticLayers)
            {
                layer.SetPaper(paper);
            }
            currentLayer = semanticLayers[0];
            this.Container.Children.Add(currentLayer);
        }

        public void ChangeSize(int index) {

            this.Container.Children.Remove(currentLayer);
            currentLayer = semanticLayers[index];
            this.Container.Children.Add(currentLayer);
        }
    }
}
