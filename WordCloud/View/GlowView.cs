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
    class GlowView:Canvas
    {
        double[] radius = new double[] { 0, 0, 0, 0 };
        double scale = 1;
        Ellipse[] circles;
        double x = 0, y = 0;
        string keyword;
        public GlowView(string keyword)
        {
            this.Width = STATICS.DEFAULT_GLOW_DIAMETER;
            this.Height = STATICS.DEFAULT_GLOW_DIAMETER;
            circles = new Ellipse[4];
            this.keyword = keyword;
            for (int i = 3; i >= 0; i--)
            {
                circles[i] = new Ellipse();
                circles[i].Fill = new SolidColorBrush(STATICS.USER_COLOR[i]);
                circles[i].Width = STATICS.DEFAULT_GLOW_DIAMETER;
                circles[i].Height = STATICS.DEFAULT_GLOW_DIAMETER;
                Matrix mtx = new Matrix();
                mtx.Translate(-circles[i].Width / 2, -circles[i].Height / 2);
                circles[i].RenderTransform = new MatrixTransform(mtx);
                this.Children.Add(circles[i]);
            }
        }
        public double GetRanking() {
            return radius.Max();
        }
        public void MoveTo(double x, double y)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.x = x;
                this.y = y;
                Matrix mtx = new Matrix();
                mtx.Scale(scale, scale);
                mtx.Translate(x, y);
                this.RenderTransform = new MatrixTransform(mtx);
            }));
        }
        public void ClearUserFactors() {
            this.radius = new double[] { 0, 0, 0, 0 };
        }
        public void Proportion(double diameter, double[] radiuses)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.radius = radiuses;
                scale = diameter / STATICS.DEFAULT_GLOW_DIAMETER;
                for (int i = 3; i >= 0; i--)
                {
                    Matrix mtx = new Matrix();
                    mtx.Translate(-circles[i].Width / 2, -circles[i].Height / 2);
                    mtx.Scale(radius[i], radius[i]);
                    circles[i].RenderTransform = new MatrixTransform(mtx);
                }
                Matrix mtxG = new Matrix();
                mtxG.Scale(scale, scale);
                mtxG.Translate(x, y);
                this.RenderTransform = new MatrixTransform(mtxG);
                var sorted = radius.OrderByDescending(s=>s);
                int order = 0;
                foreach (double d in sorted) {
                    int index = Array.IndexOf(radius, d);
                    Canvas.SetZIndex(circles[index], order);
                    order++;
                }
            }));
        }
    }
}
