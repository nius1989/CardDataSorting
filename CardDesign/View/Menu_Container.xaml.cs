using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        TextBox bin_textBox = new TextBox();

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
            addBoxButton.Content = "Create Box";
            Canvas.SetLeft(addBoxButton, 50);
            Canvas.SetTop(addBoxButton, 5);
            this.Children.Add(addBoxButton);
            addBoxButton.Click += AddBoxButton_Click;

            recycleButton.Width = (this.Width - 200) / 3;
            recycleButton.Height = (this.Height - 10);
            Canvas.SetLeft(recycleButton, 100 + addBoxButton.Width);
            Canvas.SetTop(recycleButton, 5);
            this.Children.Add(recycleButton);

            resetButton.Width = (this.Width - 200) / 3;
            resetButton.Height = (this.Height - 10);
            resetButton.Content = "Reset";
            Canvas.SetLeft(resetButton, 150 + addBoxButton.Width + recycleButton.Width);
            Canvas.SetTop(resetButton, 5);
            this.Children.Add(resetButton);

            keyboard = new FloatingTouchScreenKeyboard();
            keyboard.IsDragHelperAllowedToHide = true;
            keyboard.Width = STATICS.MENU_BAR_SIZE.Width;
            keyboard.Height = 250;
            keyboard.Placement = System.Windows.Controls.Primitives.PlacementMode.Relative;
            keyboard.PlacementTarget = menuLayer;
            keyboard.TouchDown += Keyboard_TouchDown;
            keyboard.MouseLeftButtonDown += Keyboard_MouseLeftButtonDown;

            bin_textBox.Width = keyboard.Width;
            bin_textBox.Height = 30;
            bin_textBox.Background = new SolidColorBrush(Colors.LightGray);
            bin_textBox.Foreground = new SolidColorBrush(Colors.Black);
            bin_textBox.MaxLines = 2;
            bin_textBox.KeyDown += Bin_textBox_KeyDown;


            if (STATICS.ALEX_ACTIVE && User.Equals("Alex"))
            {
                double x = (STATICS.SCREEN_WIDTH - this.Width) / 2;
                double y = STATICS.SCREEN_HEIGHT - this.Height - keyboard.Height;
                keyboard.PlacementRectangle = new Rect(x, y, keyboard.Width, keyboard.Height);
                Matrix mtTB = new Matrix();
                mtTB.Translate(x, y - bin_textBox.Height);
                bin_textBox.RenderTransform = new MatrixTransform(mtTB);
            }
            else if (STATICS.BEN_ACTIVE && User.Equals("Ben"))
            {
                //To-be Finished
            }
            else if (STATICS.CHRIS_ACTIVE && User.Equals("Chris"))
            {
                Matrix mtx = new Matrix();
                mtx.Rotate(90);
                keyboard.RenderTransform = new MatrixTransform(mtx);
                double x = STATICS.MENU_BAR_SIZE.Height + keyboard.Height;
                double y = (STATICS.SCREEN_HEIGHT - keyboard.Width) / 2;
                keyboard.PlacementRectangle = new Rect(x, y, keyboard.Width, keyboard.Height);
                Matrix mtTB = new Matrix();
                mtTB.Rotate(90);
                mtTB.Translate(x + bin_textBox.Height, y);
                bin_textBox.RenderTransform = new MatrixTransform(mtTB);
            }
            else if (STATICS.DANNY_ACTIVE && User.Equals("Danny"))
            {
                Matrix mtx = new Matrix();
                mtx.Rotate(-90);
                keyboard.RenderTransform = new MatrixTransform(mtx);
                double x = STATICS.SCREEN_WIDTH - keyboard.Height - STATICS.MENU_BAR_SIZE.Height;
                double y = (STATICS.SCREEN_HEIGHT + keyboard.Width) / 2;
                keyboard.PlacementRectangle = new Rect(x, y, keyboard.Width, keyboard.Height);
                Matrix mtTB = new Matrix();
                mtTB.Rotate(-90);
                mtTB.Translate(x - bin_textBox.Height, y);
                bin_textBox.RenderTransform = new MatrixTransform(mtTB);
            }
            menuLayer.Children.Add(bin_textBox);
            bin_textBox.Visibility = Visibility.Hidden;
            InitializeComponent();
        }

        private void Bin_textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                menuLayer.MainWindow.Controlers.SortingBoxControler.CreateGroup(user,
                bin_textBox.Text,
                bin_textBox.Text,
                bin_textBox.Text.Substring(0, 3),
                new Point(STATICS.SCREEN_WIDTH/2, STATICS.SCREEN_HEIGHT / 2)
                );
                bin_textBox.Visibility = Visibility.Hidden;
                bin_textBox.Text = "";
                keyboard.IsOpen = false;
                addBoxButton.Content = "Create Box";
            }
        }

        private void Keyboard_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //debugging code
            bin_textBox.Focus();

        }

        private void Keyboard_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            bin_textBox.Focus();
        }


        private void AddBoxButton_Click(object sender, RoutedEventArgs e)
        {
            if (addBoxButton.Content.Equals("Create Box"))
            {
                bin_textBox.Visibility = Visibility.Visible;
                bin_textBox.Text = "";
                keyboard.IsOpen = true;
                addBoxButton.Content = "Cancel";
            }
            else
            {
                bin_textBox.Visibility = Visibility.Hidden;
                bin_textBox.Text = "";
                keyboard.IsOpen = false;
                addBoxButton.Content = "Create Box";
            }
        }
    }
}
