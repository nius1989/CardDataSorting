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
        double[] radius = new double[] { 1 / 3.0, 2 / 3.0, 3 / 3.0 };
        readonly double DEFAULT_DIAMETER = 100;
        double scale = 1;
        Ellipse[] circles;
        double x = 0, y = 0;
        int userNum = 3;
        public void Initialize(int userNum)
        {
            this.userNum = userNum;
            this.Width = DEFAULT_DIAMETER;
            this.Height = DEFAULT_DIAMETER;
            Dispatcher.Invoke(new Action(() =>
            {
                circles = new Ellipse[userNum];
                for (int i = userNum - 1; i >= 0; i--)
                {
                    circles[i] = new Ellipse();
                    circles[i].Fill = new SolidColorBrush(STATICS.USER_COLOR[i]);
                    circles[i].Width = DEFAULT_DIAMETER;
                    circles[i].Height = DEFAULT_DIAMETER;
                    Matrix mtx = new Matrix();
                    mtx.Translate(-circles[i].Width/2, -circles[i].Height/2);
                    circles[i].RenderTransform = new MatrixTransform(mtx);
                    this.Children.Add(circles[i]);
                }
            }));
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

        public void Proportion(double diameter, params double[] radiuses)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.radius = radiuses;
                scale = diameter / DEFAULT_DIAMETER;
                for (int i = userNum - 1; i >= 0; i--)
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
            }));
        }
    }
}
