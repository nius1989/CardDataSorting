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
using WpfKb.Controls;

namespace CardDesign
{
    /// <summary>
    /// Interaction logic for Group_Menu.xaml
    /// </summary>
    public partial class Menu_Container : Canvas
    {
        Menu_Layer menuLayer;
        String user;
        Button addBoxButton = new Button();
        Menu_Recycle_Bin recycleButton = new Menu_Recycle_Bin();
        Button resetButton = new Button();
        FloatingTouchScreenKeyboard keyboard;

        public String User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }

        public Menu_Container(Menu_Layer menuLayer, String user)
        {
            this.User = user;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(@"Resource\Image\menu_bg.png", UriKind.Relative));
            this.Background = ib;
            this.menuLayer = menuLayer;
            this.Height = STATICS.MENU_BAR_SIZE.Height;
            this.Width = STATICS.MENU_BAR_SIZE.Width;
            addBoxButton.Width = (this.Width - 200) / 3;
            addBoxButton.Height = (this.Height - 10);
            addBoxButton.Content = "Add Box";
            Canvas.SetLeft(addBoxButton, 50);
            Canvas.SetTop(addBoxButton, 5);
            this.Children.Add(addBoxButton);
            addBoxButton.Click += AddBoxButton_Click;

            recycleButton.Width = (this.Width - 200) / 3;
            recycleButton.Height = (this.Height - 10);
            Canvas.SetLeft(recycleButton, 100+addBoxButton.Width);
            Canvas.SetTop(recycleButton, 5);
            this.Children.Add(recycleButton);

            resetButton.Width = (this.Width - 200) / 3;
            resetButton.Height = (this.Height - 10);
            resetButton.Content = "Reset";
            Canvas.SetLeft(resetButton, 150 + addBoxButton.Width+recycleButton.Width);
            Canvas.SetTop(resetButton, 5);
            this.Children.Add(resetButton);

            //keyboard = new FloatingTouchScreenKeyboard();
            //keyboard.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
            //keyboard.Width = 900;
            //keyboard.Height = 400;
            //menuLayer.Children.Add(keyboard);

            InitializeComponent();
        }

        private void AddBoxButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("here");
            keyboard.Visibility=Visibility.Visible;
        }
    }
}
