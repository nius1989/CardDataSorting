using System;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardDesign
{
    /// <summary>
    /// Interaction logic for Card_Layer.xaml
    /// </summary>
    public partial class Card_Layer : Canvas
    {
        MainWindow mainWindow;

        public MainWindow MainWindow
        {
            get { return mainWindow; }
            set { mainWindow = value; }
        }
        public Card_Layer(MainWindow mainWindow)
        {
            InitializeComponent();
            this.IsManipulationEnabled = false;
            this.mainWindow = mainWindow;
            this.Height = STATICS.SCREEN_HEIGHT;
            this.Width = STATICS.SCREEN_WIDTH;
        }
        public void AddCard(Card card)
        {
            this.Children.Add(card);
        }
        protected override void OnTouchDown(TouchEventArgs e)
        {
            TouchPoint point = e.GetTouchPoint(this);
            mainWindow.Controlers.TouchControler.TouchDown(this, this.GetType(), e.TouchDevice.Id, point);
            mainWindow.ControlWindow.UpdateTextInfo(mainWindow.Controlers.TouchControler.ToString(), 1);
            e.Handled = true;
            base.OnTouchDown(e);
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            TouchPoint point = e.GetTouchPoint(this);
            mainWindow.Controlers.TouchControler.TouchMove(this, this.GetType(), e.TouchDevice, point);
            if (STATICS.DEBUG_MODE)
                mainWindow.ControlWindow.UpdateTextInfo(mainWindow.Controlers.TouchControler.ToString(), 1);
            e.Handled = true;
            base.OnTouchMove(e);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            TouchPoint point = e.GetTouchPoint(this);
            mainWindow.Controlers.TouchControler.TouchUp(e.TouchDevice, point);
            mainWindow.ControlWindow.UpdateTextInfo(mainWindow.Controlers.TouchControler.ToString(), 1);
            e.Handled = true;
            base.OnTouchUp(e);
        }
        protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            // Adjust Z-order
            Card element = e.Source as Card;
            if (element.ZIndex < this.Children.Count - 1)
            {
                foreach (FrameworkElement child in this.Children)
                {
                    Card c = child as Card;
                    if (element.ZIndex < c.ZIndex)
                    {
                        Canvas.SetZIndex(c, --c.ZIndex);
                    }
                }
                Canvas.SetZIndex(element, this.Children.Count - 1);
                element.ZIndex = this.Children.Count - 1;
            }
            e.Handled = true;
            base.OnManipulationStarting(e);
        }

        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e)
        {
            e.TranslationBehavior = new InertiaTranslationBehavior();
            if (e.InitialVelocities.LinearVelocity.Length < 6)
            {
                e.TranslationBehavior.InitialVelocity = e.InitialVelocities.LinearVelocity;
            }
            else
            {
                Vector newVector = e.InitialVelocities.LinearVelocity;
                newVector.Normalize();
                newVector.X *= 6;
                newVector.Y *= 6;
                e.TranslationBehavior.InitialVelocity = newVector;
            }
            e.TranslationBehavior.DesiredDeceleration = 20.0 * 96.0 / (1000.0 * 1000.0);
            e.Handled = true;
        }
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            var element = e.Source as Card;
            ManipulationDelta delta = e.DeltaManipulation;

            if (delta.Translation.Length > 0.0001)
            {
                Matrix matrix = new Matrix();
                if (STATICS.MIN_CARD_SCALE < element.CurrentScale * delta.Scale.X &&
                    element.CurrentScale * delta.Scale.X < STATICS.MAX_CARD_SCALE)
                {
                    element.CurrentScale = element.CurrentScale * delta.Scale.X;
                    matrix.Scale(element.CurrentScale, element.CurrentScale);
                    if (e.Source is Document_Card)
                    {
                        var d = e.Source as Document_Card;
                        d.UpdateText();
                    }
                }
                else
                {
                    matrix.Scale(element.CurrentScale, element.CurrentScale);     
                }

                element.CurrentRotation += delta.Rotation;
                matrix.Rotate(element.CurrentRotation);
                if (e.IsInertial)
                {
                    if (element.CurrentPosition.X >= STATICS.SCREEN_WIDTH)
                    {
                        element.Direction = new Vector(element.Direction.X * -1, element.Direction.Y);
                        element.CurrentPosition = new Point(STATICS.SCREEN_WIDTH - 1, element.CurrentPosition.Y);
                    }
                    if (element.CurrentPosition.X <= 0)
                    {
                        element.Direction = new Vector(element.Direction.X * -1, element.Direction.Y);
                        element.CurrentPosition = new Point(1, element.CurrentPosition.Y);
                    }
                    if (element.CurrentPosition.Y >= STATICS.SCREEN_HEIGHT)
                    {
                        element.Direction = new Vector(element.Direction.X, element.Direction.Y * -1);
                        element.CurrentPosition = new Point(element.CurrentPosition.X, STATICS.SCREEN_HEIGHT - 1);
                    }
                    if (element.CurrentPosition.Y <= 0)
                    {
                        element.Direction = new Vector(element.Direction.X, element.Direction.Y * -1);
                        element.CurrentPosition = new Point(element.CurrentPosition.X, 1);
                    }
                    Point newPosi = new Point(element.CurrentPosition.X + delta.Translation.X * element.Direction.X,
                         element.CurrentPosition.Y + delta.Translation.Y * element.Direction.Y);
                    element.CurrentPosition = newPosi;
                    matrix.Translate(element.CurrentPosition.X, element.CurrentPosition.Y);
                }
                else
                {
                    Point newPosi = new Point(element.CurrentPosition.X + delta.Translation.X,
                        element.CurrentPosition.Y + delta.Translation.Y);
                    if (newPosi.X < STATICS.SCREEN_WIDTH &&
                                        newPosi.X > 0 &&
                                        newPosi.Y < STATICS.SCREEN_HEIGHT &&
                                        newPosi.Y > 0)
                    {
                        element.CurrentPosition = newPosi;
                        matrix.Translate(element.CurrentPosition.X, element.CurrentPosition.Y);
                    }
                    else
                    {
                        matrix.Translate(element.CurrentPosition.X, element.CurrentPosition.Y);
                    }                
                }

                element.RenderTransform = new MatrixTransform(matrix);
                mainWindow.LinkingGestureLayer.Move(element);
            }
            else {
                element.Direction = new Vector(1, 1);
            }
            e.Handled = true;

        }
    }
}
