using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CardDesign
{
    class ZoomWheel:Canvas
    {
        double importance = 0;
        Ellipse outerCircle=new Ellipse();
        Ellipse innerCircle = new Ellipse();
        string userId = "Alex";

        public string UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

        public ZoomWheel(string userIndex) {
            this.Width = STATICS.ZOOMWHEEL_RADIUS;
            this.Height = STATICS.ZOOMWHEEL_RADIUS;
            outerCircle.Width = this.Width;
            outerCircle.Height = this.Height;
            this.UserId = userIndex;
            outerCircle.Stroke = new SolidColorBrush(STATICS.USER_COLOR[UserId]);
            outerCircle.StrokeThickness = 1;
            Matrix mtx = new Matrix();
            mtx.Translate(-this.Width / 2, -this.Height / 2);
            outerCircle.RenderTransform = new MatrixTransform(mtx);

            innerCircle.Width = this.Width;
            innerCircle.Height = this.Height;
            innerCircle.Fill = new SolidColorBrush(STATICS.USER_COLOR[UserId]);
            mtx = new Matrix();
            mtx.Translate(-this.Width / 2, -this.Height / 2);
            mtx.Scale(importance, importance);
            innerCircle.RenderTransform = new MatrixTransform(mtx);
            this.Children.Add(outerCircle);
            this.Children.Add(innerCircle);
        }

        internal void UpdateZoom(double size) {
            importance = (size - STATICS.MIN_CARD_SCALE) / STATICS.MAX_CARD_SCALE;
            Dispatcher.Invoke(new Action(() =>
            {
                Matrix mtx = new Matrix();
                mtx = new Matrix();
                mtx.Translate(-this.Width / 2, -this.Height / 2);
                mtx.Scale(importance, importance);
                innerCircle.RenderTransform = new MatrixTransform(mtx);
            }));
        }
    }
}
