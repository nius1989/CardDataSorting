using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for LinkingIcon.xaml
    /// </summary>
    public partial class Linking_Icon : Canvas
    {
        Card card;

        public Card Card
        {
            get { return card; }
            set { card = value; }
        }
        public Linking_Icon(Card card)
        {
            InitializeComponent();
            this.card = card;
            ImageBrush brush = new ImageBrush();
            BitmapImage img = new BitmapImage(new Uri(@"Resource\Image\link_icon.png", UriKind.RelativeOrAbsolute));
            brush.ImageSource = img;
            rect.Fill = brush;
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            this.CaptureTouch(e.TouchDevice);
            card.CardControler.TouchDownCard(this,e);
            e.Handled = true;
            base.OnTouchDown(e);
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            card.CardControler.TouchMoveCard(this, e); 
            e.Handled = true;
            base.OnTouchMove(e);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            card.CardControler.TouchUpCard(this, e);
            e.Handled = true;
            base.OnTouchUp(e);
        }
    }
}
