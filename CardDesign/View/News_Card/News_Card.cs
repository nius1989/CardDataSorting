using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CardDesign
{
    public partial class News_Card : Card
    {
        My_News news;
        string newsID = "";
        string userId = "Alex";
        ZoomWheel[] zoomWheels;
        Layer_Base[] semanticLayers;
        Layer_Base currentLayer;
        int currentLayerIndex = 0;
        internal string NewsID
        {
            get
            {
                return newsID;
            }

            set
            {
                newsID = value;
            }
        }
        internal My_News News
        {
            get { return news; }
            set { news = value; }
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
                    this.Container.Children.Remove(currentLayer);
                    currentLayer.RemoveZoomWheel();
                    currentLayer = semanticLayers[newIndex];
                    currentLayer.AddZoomWheel(zoomWheels);
                    this.Container.Children.Add(currentLayer);
                }
                base.CurrentScale = value;
            }
        }

        public News_Card(Card_Controler controler,string userId) : base(controler) {
            List<Layer_Base> list = new List<Layer_Base>();
            this.userId = userId;
            List<ZoomWheel> templist = new List<ZoomWheel>();
            int index = 0;
            for (int i = 0; i < STATICS.USER_IDS.Length; i++)
            {
                if (!STATICS.USER_IDS[i].Equals(userId) && STATICS.USER_ACTIVE[i])
                {
                    ZoomWheel zw = new ZoomWheel(STATICS.USER_IDS[i]);
                    zw.VerticalAlignment = VerticalAlignment.Center;
                    zw.HorizontalAlignment = HorizontalAlignment.Center;
                    zw.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1, zw.Width / 2, zw.Height / 2));
                    templist.Add(zw);
                    index++;
                }
            }
            zoomWheels = templist.ToArray();

            list.Add(new Layer_One());
            list.Add(new Layer_Two());
            list.Add(new Layer_Three());
            list.Add(new Layer_Four());
            semanticLayers = list.ToArray();
        }

        public override void InitializeCard(Color? maskColor, Point defaultPosi, double defaultDegree, double defaultScale, int zidx)
        {
            base.InitializeCard(maskColor, defaultPosi, defaultDegree, defaultScale, zidx);
            foreach (Layer_Base layer in semanticLayers)
            {
                layer.SetArticle(news);
            }
            currentLayer = semanticLayers[0];
            this.Container.Children.Add(currentLayer);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            Card_Controler.UpdateZoomWheels(Owner, newsID,this.CurrentScale);
            base.OnTouchUp(e);
        }
        internal void UpdateZoomWheel(string owner, double scale)
        {
            foreach (ZoomWheel zw in zoomWheels)
            {
                if (zw.UserId == owner)
                    zw.UpdateZoom(scale);
            }
        }
    }
}
