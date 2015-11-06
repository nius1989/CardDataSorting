using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardDesign
{
    /// <summary>
    /// Interaction logic for Card.xaml
    /// A class of the card
    /// </summary>
    public partial class Card : Canvas
    {
        Card_Controler cardControler;
        String uuid = "";
        String owner = "";
        int zIndex = 0;
        Rectangle backgroundRect = new Rectangle();
        Rectangle hightlightMask = new Rectangle();
        TextBlock sortingGroupText = new TextBlock();
        Linking_Icon linkingIcon;
        Copy_Icon copyIcon;
        Color backgroundColor;
        Color hightlightColor = Colors.Gold;
        double brightness = STATICS.START_CARD_BRIGHT;
        Point defaultPostion = new Point(0, 0);
        Point previousPostion = new Point(0, 0);
        Point currentPosition = new Point(0, 0);
        double defaultRotation = 0;
        double currentScale = 1;
        double currentRotation = 0;
        int touchPointNum = 0;
        List<String> groupLists = new List<string>();
        Vector direction = new Vector(1, 1);

        public Card_Controler CardControler
        {
            get { return cardControler; }
            set { cardControler = value; }
        }
        public double CurrentScale
        {
            get { return currentScale; }
            set { currentScale = value; }
        }
        public String Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        public int ZIndex
        {
            get { return zIndex; }
            set { zIndex = value; }
        }
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        public Color HightlightColor
        {
            get { return hightlightColor; }
            set { hightlightColor = value; }
        }
        public String UUID
        {
            get { return uuid; }
            set { uuid = value; }
        }

        public Point CurrentPosition
        {
            get { return currentPosition; }
            set { currentPosition = value; }
        }

        public double Brightness
        {
            get { return brightness; }
            set { brightness = value; }
        }
        public List<String> SortingGroups
        {
            get { return groupLists; }
            set { groupLists = value; }
        }
        public double CurrentRotation
        {
            get { return currentRotation; }
            set { currentRotation = value; }
        }
        public Point PreviousPostion
        {
            get { return previousPostion; }
            set { previousPostion = value; }
        }
        public Rectangle BackgroundRect
        {
            get { return backgroundRect; }
            set { backgroundRect = value; }
        }
        public Vector Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public Card(Card_Controler cardControl)
        {
            InitializeComponent();
            this.cardControler = cardControl;
            this.IsManipulationEnabled = true;

            this.Width = STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width;
            this.Height = STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height;
        }
        public virtual void InitializeCard(Color? maskColor, Point defaultPosi, double defaultDegree, double defaultScale, int zidx)
        {

            this.backgroundColor = maskColor != null ? maskColor.Value : Colors.White;
            backgroundRect.Width = STATICS.DEAULT_CARD_SIZE.Width;
            backgroundRect.Height = STATICS.DEAULT_CARD_SIZE.Height;
            backgroundRect.StrokeThickness = 0.3;
            backgroundRect.Stroke = new SolidColorBrush(Colors.Black);
            backgroundRect.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1,
                       -backgroundRect.Width / 2,
                       -backgroundRect.Height / 2));
            backgroundRect.Fill = new SolidColorBrush(backgroundColor);

            //ResetBrightness();

            sortingGroupText.Foreground = new SolidColorBrush(Colors.White);
            sortingGroupText.FontSize = 14;
            sortingGroupText.FontFamily = new FontFamily("Quartz MS");
            sortingGroupText.TextWrapping = TextWrapping.WrapWithOverflow;
            sortingGroupText.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1, -backgroundRect.Width / 2 + 7, -backgroundRect.Height / 2 - 14));

            Matrix matrix = Matrix.Identity;
            matrix.Translate(-backgroundRect.Width / 2, -backgroundRect.Height / 2 + 5);
            matrix.Rotate(180);
            matrix.Translate(-5, 9);

            hightlightMask.Width = this.Width;
            hightlightMask.Height = this.Height;
            hightlightMask.Fill = Brushes.Transparent;
            this.hightlightMask.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1,
                       -backgroundRect.Width / 2,
                       -backgroundRect.Height / 2));


            linkingIcon = new Linking_Icon(this);
            linkingIcon.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1,
                -linkingIcon.Width / 2,
                -(backgroundRect.Height - linkingIcon.Height / 2)));
            linkingIcon.Opacity = 0;

            copyIcon = new Copy_Icon(this);
            copyIcon.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1,
                -copyIcon.Width / 2,
                -copyIcon.Height / 2));
            copyIcon.Opacity = 0;

            this.Container.Children.Add(hightlightMask);
            this.Container.Children.Add(backgroundRect);
            this.Container.Children.Add(sortingGroupText);


            defaultPostion = defaultPosi;
            defaultRotation = defaultDegree;
            positCard(defaultPostion);
            rotateCard(defaultRotation);
            scaleCard(defaultScale);
            this.zIndex = zidx;

        }

        public bool Contain(Point p)
        {
            return Calculator.CalDistance(currentPosition, p) < STATICS.DEAULT_CARD_SIZE.Width / 2;
        }
        public void ChangeBrightness(double nsize, double osize)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                double rate = 5000.0;
                double opacitychange = (nsize - osize) / rate;
                double newOpacity = brightness + opacitychange;
                if (newOpacity > 1)
                {
                    newOpacity = 1;
                }
                else if (newOpacity < STATICS.START_CARD_BRIGHT)//if dehight
                {
                    newOpacity = STATICS.START_CARD_BRIGHT;
                }
                brightness = newOpacity;
                updateMaskColor();
            }));
        }
        public void ResetBrightness()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                updateMaskColor();
            }));
        }

        public void MoveCard(double x, double y, double duration)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Matrix fromMatrix = (this.RenderTransform as MatrixTransform).Matrix;
                Matrix toMatrix = new Matrix();
                toMatrix.Rotate(currentRotation);
                toMatrix.Scale(currentScale, currentScale);
                toMatrix.Translate(currentPosition.X + x, currentPosition.Y + y);
                if (duration > 0)
                {
                    LinearMatrixAnimation anim = new LinearMatrixAnimation(fromMatrix, toMatrix, TimeSpan.FromSeconds(duration));
                    MatrixTransform trans = new MatrixTransform();
                    this.RenderTransform.BeginAnimation(MatrixTransform.MatrixProperty, anim);
                }
                else
                {
                    this.RenderTransform = new MatrixTransform(toMatrix);
                }
                this.currentPosition = new Point(this.currentPosition.X + x, this.currentPosition.Y + y);
            }));
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
        private void scaleCard(double scale) {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MatrixTransform xform = this.RenderTransform as MatrixTransform;
                Matrix matrix = xform.Matrix;
                matrix.ScaleAt(scale,scale,
                    matrix.OffsetX,
                matrix.OffsetY);
                currentScale = scale;
                this.RenderTransform = new MatrixTransform(matrix);
            }));
        }
        public void Hightlight(Color color)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.hightlightMask.Fill = new SolidColorBrush(color); ;
                DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(STATICS.ANIMATION_DURATION * 0.3));
                this.hightlightMask.BeginAnimation(Canvas.OpacityProperty, animation);
            }));
        }
        public void Hightlight()
        {
            Dispatcher.BeginInvoke(new Action(() =>
               {
                   this.hightlightMask.Fill = new SolidColorBrush(hightlightColor);
                   DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(STATICS.ANIMATION_DURATION * 0.3));
                   this.hightlightMask.BeginAnimation(Canvas.OpacityProperty, animation);
               }));
        }

        public void Dehightlight()
        {
            Dispatcher.BeginInvoke(new Action(() =>
               {
                       DoubleAnimation animation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(STATICS.ANIMATION_DURATION * 0.3));
                       this.hightlightMask.BeginAnimation(Canvas.OpacityProperty, animation);
               }));
        }

        public void SortToGroup(String groupID)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!groupLists.Contains(groupID))
                {
                    groupLists.Add(groupID);
                    String str = "";
                    foreach (String s in groupLists)
                    {
                        str += Group_List.GroupBox[s].GroupTextBrief+" ";
                    }
                    sortingGroupText.Text = str;
                }
            }));
        }
        public void RemoveFromGroup(String groupNum)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (groupLists.Contains(groupNum))
                {
                    groupLists.Remove(groupNum);
                    String str = "";
                    foreach (String s in groupLists)
                    {
                        str += Group_List.GroupBox[s].GroupTextBrief + " ";
                    }
                    sortingGroupText.Text = str;
                }
            }));
        }
        private void updateMaskColor()
        {
            backgroundRect.Fill = new SolidColorBrush(Color.FromArgb(255,
               (byte)(backgroundColor.R * (brightness)),
               (byte)(backgroundColor.G * (brightness)),
               (byte)(backgroundColor.B * (brightness))));
        }
        protected override void OnTouchDown(TouchEventArgs e)
        {
            this.CaptureTouch(e.TouchDevice);
            cardControler.TouchDownCard(this, e);
            e.Handled = true;
            Hightlight();
            previousPostion = this.currentPosition;
            touchPointNum++;
            if (touchPointNum >= 3)
            {
                if (!this.Container.Children.Contains(linkingIcon))
                {
                    this.Container.Children.Add(linkingIcon);
                }

                if (!this.Container.Children.Contains(copyIcon))
                {
                    this.Container.Children.Add(copyIcon);
                }
                DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(STATICS.ANIMATION_DURATION * 0.3));
                linkingIcon.BeginAnimation(Canvas.OpacityProperty, animation);
                copyIcon.BeginAnimation(Canvas.OpacityProperty, animation);
            }
            base.OnTouchDown(e);
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            cardControler.TouchDownCard(this, e);
            e.Handled = true;
            base.OnTouchMove(e);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            cardControler.TouchUpCard(this, e);
            e.Handled = true;
            Dehightlight();
            touchPointNum=0;
            if (touchPointNum != 3)
            {
                DoubleAnimation animation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(STATICS.ANIMATION_DURATION * 0.3));
                linkingIcon.BeginAnimation(Canvas.OpacityProperty, animation);
                copyIcon.BeginAnimation(Canvas.OpacityProperty, animation);
                if (this.Container.Children.Contains(linkingIcon))
                {
                    this.Container.Children.Remove(linkingIcon);
                }
                if (this.Container.Children.Contains(copyIcon))
                {
                    this.Container.Children.Remove(copyIcon);
                }
            }
            base.OnTouchUp(e);
        }
        public override string ToString()
        {
            String result = "";
            Dispatcher.BeginInvoke(new Action(() =>
            {
                result = this.uuid + " " + this.Owner;
            }), System.Windows.Threading.DispatcherPriority.Send);
            return base.ToString();
        }
    }
}