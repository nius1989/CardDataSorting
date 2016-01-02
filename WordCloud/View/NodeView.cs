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
    class NodeView:Canvas
    {
        TextBlock txtBlock = new TextBlock();
        double x = 0, y = 0;
        string text = "default";
        double fontSize = 10;
        double ranking = 0;
        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }
        public NodeView(string keyword) {
            this.text = keyword;
            this.txtBlock.Text = Graph.GetOriginalWord(text).ToUpper();
            txtBlock.TextAlignment = System.Windows.TextAlignment.Center;
            txtBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            txtBlock.FontSize = fontSize;
            txtBlock.FontFamily = new FontFamily("Segoe UI");
            Border border = new Border();
            border.Width = 400;
            border.Height = 300;
            border.Child = txtBlock;
            Matrix mx = new Matrix();
            mx.Translate(-border.Width / 2, -border.Height / 2);
            border.RenderTransform = new MatrixTransform(mx);
            this.Children.Add(border);
        }
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public double Ranking
        {
            get
            {
                return ranking;
            }

        }

        public void MoveTo(double x, double y) {
            this.X = x;
            this.Y = y;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Matrix mx = new Matrix();
                mx.Translate(x, y);
                this.RenderTransform = new MatrixTransform(mx);
            }));
        }

        public void SetRanking(double ranking) {
            this.ranking=ranking;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                txtBlock.FontSize = Graph.GetFontSize(this.Ranking,STATICS.MIN_FONT_SIZE,STATICS.MAX_FONT_SIZE);
            }));
        }
    }
}
