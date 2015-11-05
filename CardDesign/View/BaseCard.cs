using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CardDesign
{
    class BaseCard:Canvas
    {
        int UUID = 0;
        int zIndex = 0;
        Rectangle backgroundRect = new Rectangle();
        Rectangle hightlightMask = new Rectangle();        
        Color backgroundColor;
        Color hightlightColor = Colors.Gold;
        DoubleAnimation highlightAnim;
        Point defaultPostion = new Point(0, 0);
        Point previousPostion = new Point(0, 0);
        Point currentPosition = new Point(0, 0);
        double defaultRotation = 0;
        double currentScale = 1;
        double currentRotation = 0;
        int touchPointNum = 0;
        Vector direction = new Vector(1, 1);

        public BaseCard()
        {
            this.IsManipulationEnabled = true;
            this.Width = STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width;
            this.Height = STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height;
        }

        public void InitializeBaseCard(Color bgColor, Point defaultPosi, double defaultDegree, double defaultScale, int zidx)
        {

            this.backgroundColor = bgColor;
            backgroundRect.Width = STATICS.DEAULT_CARD_SIZE.Width;
            backgroundRect.Height = STATICS.DEAULT_CARD_SIZE.Height;
            backgroundRect.Opacity = 1;
            backgroundRect.StrokeThickness = 0;
            backgroundRect.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1,
                       -backgroundRect.Width / 2,
                       -backgroundRect.Height / 2));
            backgroundRect.Opacity = 0.9;                    

            hightlightMask.Width = this.Width;
            hightlightMask.Height = this.Height;
            hightlightMask.Fill = Brushes.Transparent;
            this.hightlightMask.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1,
                       -backgroundRect.Width / 2,
                       -backgroundRect.Height / 2));

            this.Children.Add(hightlightMask);
            this.Children.Add(backgroundRect);


            defaultPostion = defaultPosi;
            defaultRotation = defaultDegree;
            positCard(defaultPostion);
            rotateCard(defaultRotation);
            scaleCard(defaultScale);
            this.zIndex = zidx;
        }

        private void positCard(Point point)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                currentPosition.X += point.X;
                currentPosition.Y += point.Y;
                MatrixTransform xform = this.RenderTransform as MatrixTransform;
                Matrix matrix = xform.Matrix;
                matrix.Translate(point.X, point.Y);
                this.RenderTransform = new MatrixTransform(matrix);
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }
        private void rotateCard(double angle)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MatrixTransform xform = this.RenderTransform as MatrixTransform;
                Matrix matrix = xform.Matrix;
                matrix.RotateAt(angle,
                    matrix.OffsetX,
                matrix.OffsetY);
                currentRotation += angle;
                this.RenderTransform = new MatrixTransform(matrix);
            }));
        }
        private void scaleCard(double scale)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MatrixTransform xform = this.RenderTransform as MatrixTransform;
                Matrix matrix = xform.Matrix;
                matrix.ScaleAt(scale, scale,
                    matrix.OffsetX,
                matrix.OffsetY);
                currentScale = scale;
                this.RenderTransform = new MatrixTransform(matrix);
            }));
        }
    }
}
