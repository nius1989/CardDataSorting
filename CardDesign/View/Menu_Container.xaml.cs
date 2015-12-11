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
        Menu_Recycle_Bin recycleButton;
        Button resetButton = new Button();
        FloatingTouchScreenKeyboard keyboard;
        TextBox bin_textBox = new TextBox();

        TextBox resetNotification = new TextBox();
        TextBox recycleNotification = new TextBox();
        Boolean isClicked = false;

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

        public Boolean IsClicked
        {
            get { return isClicked; }
        }

        public Menu_Recycle_Bin RecycleButton
        {
            get
            {
                return recycleButton;
            }

            set
            {
                recycleButton = value;
            }
        }

        public Menu_Container(Menu_Layer menuLayer, String user)
        {
            this.User = user;
            recycleButton = new Menu_Recycle_Bin(this);
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

            RecycleButton.Width = (this.Width - 200) / 3;
            RecycleButton.Height = (this.Height - 10);
            Canvas.SetLeft(RecycleButton, 100 + addBoxButton.Width);
            Canvas.SetTop(RecycleButton, 5);
            this.Children.Add(RecycleButton);

            resetButton.Width = (this.Width - 200) / 3;
            resetButton.Height = (this.Height - 10);
            resetButton.Content = "Reset";
            resetButton.TouchDown += ResetButton_Pressed;
            resetButton.TouchUp += ResetButton_Released;
            Canvas.SetLeft(resetButton, 150 + addBoxButton.Width + RecycleButton.Width);
            Canvas.SetTop(resetButton, 5);
            this.Children.Add(resetButton);
                     
            InitializeComponent();
        }



        public void InitializeContainer() {
            keyboard = new FloatingTouchScreenKeyboard();
            keyboard.IsDragHelperAllowedToHide = true;
            keyboard.Width = STATICS.MENU_BAR_SIZE.Width;
            keyboard.Height = 260;
            keyboard.TouchDown += Keyboard_TouchDown;
            keyboard.MouseLeftButtonDown += Keyboard_MouseLeftButtonDown;
            keyboard.Visibility = Visibility.Hidden;

            bin_textBox.Width = keyboard.Width;
            bin_textBox.Height = 30;
            bin_textBox.Background = new SolidColorBrush(Colors.LightGray);
            bin_textBox.Foreground = new SolidColorBrush(Colors.Black);
            bin_textBox.MaxLines = 2;
            bin_textBox.KeyDown += Bin_textBox_KeyDown;
            bin_textBox.Visibility = Visibility.Hidden;

            resetNotification.Width = 100;
            resetNotification.Height = 100;
            resetNotification.IsManipulationEnabled = false;
            resetNotification.AppendText("The board will be \nreset if all three\nreset buttons are \npressed at the \nsame time");
            resetNotification.Visibility = Visibility.Hidden;
            Canvas.SetLeft(resetNotification, 170 + addBoxButton.Width + recycleButton.Width);
            Canvas.SetTop(resetNotification, this.Height * -2);
            this.Children.Add(resetNotification);

            recycleNotification.Width = 80;
            recycleNotification.Height = 50;
            recycleNotification.IsManipulationEnabled = false;
            recycleNotification.AppendText("The category \nbox will be \ndeleted.");
            recycleNotification.Visibility = Visibility.Hidden;
            Canvas.SetLeft(recycleNotification, 125 + addBoxButton.Width);
            Canvas.SetTop(recycleNotification, this.Height * -1);
            this.Children.Add(recycleNotification);

            Matrix mtKBD = new Matrix();
            mtKBD.Translate(-5, keyboard.Height * -1);
            keyboard.RenderTransform = new MatrixTransform(mtKBD);

            Matrix mtTB = new Matrix();
            mtTB.Translate(0, -1 * (keyboard.Height + bin_textBox.Height + 100));
            bin_textBox.RenderTransform = new MatrixTransform(mtTB);

            if (STATICS.ALEX_ACTIVE && User.Equals("Alex"))
            {

                RecycleButton.XCoord = STATICS.SCREEN_WIDTH / 2;
                RecycleButton.YCoord = STATICS.SCREEN_HEIGHT - this.Height / 2;
            }
            else if (STATICS.BEN_ACTIVE && User.Equals("Ben"))
            {
                //To-be Finished
            }
            else if (STATICS.CHRIS_ACTIVE && User.Equals("Chris"))
            {
                RecycleButton.XCoord = this.Height / 2;
                RecycleButton.YCoord = STATICS.SCREEN_HEIGHT / 2;
            }
            else if (STATICS.DANNY_ACTIVE && User.Equals("Danny"))
            {
                RecycleButton.XCoord = STATICS.SCREEN_WIDTH - this.Height / 2;
                RecycleButton.YCoord = STATICS.SCREEN_HEIGHT / 2;
            }
            this.Container.Children.Add(bin_textBox);
            this.Container.Children.Add(keyboard);
        }
        private void Bin_textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                String brief = bin_textBox.Text.Length > 2 ? bin_textBox.Text.Substring(0, 3) : bin_textBox.Text;
                menuLayer.MainWindow.Controlers.SortingBoxControler.CreateGroup(user,
                bin_textBox.Text,
                bin_textBox.Text,
                brief,
                new Point(STATICS.SCREEN_WIDTH/2, STATICS.SCREEN_HEIGHT / 2)
                );
                bin_textBox.Visibility = Visibility.Hidden;
                bin_textBox.Text = "";
                addBoxButton.Content = "Create Box";
                keyboard.Visibility = Visibility.Hidden;
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
                keyboard.Visibility = Visibility.Visible;
                bin_textBox.Text = "";
                addBoxButton.Content = "Cancel";
            }
            else
            {
                bin_textBox.Visibility = Visibility.Hidden;
                keyboard.Visibility = Visibility.Hidden;
                bin_textBox.Text = "";
                addBoxButton.Content = "Create Box";
            }
        }

        private void displayResetNotification()
        {
            if (isClicked)
            {
                resetNotification.Visibility = Visibility.Visible;
            }
            else
            {
                resetNotification.Visibility = Visibility.Hidden;
            }
        }

        public void displayRecycleNotification()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                recycleNotification.Visibility = Visibility.Visible;
            }));
        }

        public void removeRecycleNotification()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                recycleNotification.Visibility = Visibility.Hidden;
            }));
        }

        private void ResetButton_Pressed(object sender, RoutedEventArgs e)
        {
            isClicked = true;
            menuLayer.resetBoard();
            displayResetNotification();
        }

        private void ResetButton_Released(object sender, RoutedEventArgs e)
        {
            isClicked = false;
            displayResetNotification();
        }
    }
}
