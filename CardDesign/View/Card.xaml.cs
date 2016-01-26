using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        List<Menu_Sort_Box> groupLists = new List<Menu_Sort_Box>();
        Vector direction = new Vector(1, 1);

        public Card_Controler CardControler
        {
            get { return cardControler; }
            set { cardControler = value; }
        }
        public virtual double CurrentScale
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
        public List<Menu_Sort_Box> SortingGroups
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

            this.backgroundColor = Colors.White;
            backgroundRect.Width = STATICS.DEAULT_CARD_SIZE.Width;
            backgroundRect.Height = STATICS.DEAULT_CARD_SIZE.Height;
            backgroundRect.StrokeThickness = 2;
            backgroundRect.Stroke = new SolidColorBrush(maskColor != null ? maskColor.Value : Colors.Black);
            backgroundRect.Fill = new SolidColorBrush(backgroundColor);
            Matrix mtx = new Matrix();
            mtx.Translate(-backgroundRect.Width / 2, -backgroundRect.Height / 2);
            backgroundRect.RenderTransform = new MatrixTransform(mtx);

            ResetBrightness();

            sortingGroupText.Foreground = new SolidColorBrush(Colors.Gray);
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
                       -hightlightMask.Width / 2,
                       -hightlightMask.Height / 2));


            linkingIcon = new Linking_Icon(this);
            linkingIcon.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1,
                +backgroundRect.Width/2- linkingIcon.Width,
                -(backgroundRect.Height/2)));
            linkingIcon.Opacity = 0;

            copyIcon = new Copy_Icon(this);
            copyIcon.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1,
                 +backgroundRect.Width / 2 - copyIcon.Width,
                 +(backgroundRect.Height / 2-copyIcon.Height)));
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

        
        public void UpdateTransform()
        {
            ScaleTransform st = new ScaleTransform();
            st.ScaleX = currentScale;
            st.ScaleY = currentScale;
            RotateTransform rt = new RotateTransform();
            rt.Angle = currentRotation;
            TranslateTransform tt = new TranslateTransform();
            tt.X = currentPosition.X;
            tt.Y = currentPosition.Y;

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(st);
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            this.RenderTransform = tg;
            if (this.FindName("TransformGroup") == null)
                this.RegisterName("TransformGroup", tg);
        }
        public void MoveCard(double x, double y, double duration)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (duration > 0)
                {
                    if (this.RenderTransform != null)
                    {
                        UpdateTransform();
                    }
                    TranslateTransform trans = (this.RenderTransform as TransformGroup).Children[2] as TranslateTransform;
                    DoubleAnimation anim1 = new DoubleAnimation(currentPosition.X, currentPosition.X + x, TimeSpan.FromSeconds(duration));
                    DoubleAnimation anim2 = new DoubleAnimation(currentPosition.Y, currentPosition.Y + y, TimeSpan.FromSeconds(duration));
                    trans.BeginAnimation(TranslateTransform.XProperty, anim1);
                    trans.BeginAnimation(TranslateTransform.YProperty, anim2);
                    currentPosition.X += x;
                    currentPosition.Y += y;
                }
                else
                {
                    currentPosition.X += x;
                    currentPosition.Y += y;
                    UpdateTransform();
                }
            }));
        }
        private void positCard(Point point)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                currentPosition.X += point.X;
                currentPosition.Y += point.Y;
                UpdateTransform();
            }));
        }
        private void rotateCard(double angle)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                currentRotation += angle;
                UpdateTransform();
            }));
        }
        private void scaleCard(double scale)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                currentScale = scale;
                UpdateTransform();
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

        public void SortToGroup(Menu_Sort_Box msBox)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!groupLists.Contains(msBox))
                {
                    groupLists.Add(msBox);
                    String str = "";
                    foreach (Menu_Sort_Box s in groupLists)
                    {
                        str += s.GroupTextBrief+" ";
                    }
                    sortingGroupText.Text = str;
                }
            }));
        }
        public void RemoveFromGroup(Menu_Sort_Box msBox)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (groupLists.Contains(msBox))
                {
                    groupLists.Remove(msBox);
                    String str = "";
                    foreach (Menu_Sort_Box s in groupLists)
                    {
                        str += s.GroupTextBrief + " ";
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