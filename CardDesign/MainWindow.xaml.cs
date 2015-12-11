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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardDesign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Control_Window controlWindow;
        public Control_Window ControlWindow
        {
            get { return controlWindow; }
            set { controlWindow = value; }
        }

        Controlers controlers;
        internal Controlers Controlers
        {
            get { return controlers; }
            set { controlers = value; }
        }

        Loaders loaders;
        internal Loaders Loaders
        {
            get { return loaders; }
            set { loaders = value; }
        } 
        
        Card_Layer cardLayer;
        public Card_Layer CardLayer
        {
            get { return cardLayer; }
            set { cardLayer = value; }
        }
        Bottom_Layer bottomLayer;
        public Bottom_Layer BottomLayer
        {
            get { return bottomLayer; }
            set { bottomLayer = value; }
        }
        Gesture_Indicator_Layer gestureIndicatorLayer;
        public Gesture_Indicator_Layer GestureIndicatorLayer
        {
            get { return gestureIndicatorLayer; }
            set { gestureIndicatorLayer = value; }
        }
        Linking_Gesture_Layer linkingGestureLayer;
        public Linking_Gesture_Layer LinkingGestureLayer
        {
            get { return linkingGestureLayer; }
            set { linkingGestureLayer = value; }
        }
        Menu_Layer menuLayer;
        private Grouping_Gesture_Layer groupingGestureLayer;
        public Grouping_Gesture_Layer GroupingGestureLayer
        {
            get { return groupingGestureLayer; }
            set { groupingGestureLayer = value; }
        }
        public Menu_Layer MenuLayer
        {
            get { return menuLayer; }
            set { menuLayer = value; }
        }
        Sorting_Gesture_Layer sortingGestureLayer;
        public Sorting_Gesture_Layer SortingGestureLayer
        {
            get { return sortingGestureLayer; }
            set { sortingGestureLayer = value; }
        }

        Rect boundary = new Rect();
        private string fileName = "";

        public Rect Boundary
        {
            get { return boundary; }
            set { boundary = value; }
        }
        public MainWindow()
        {
            InitializeComponent();

            if (System.Windows.Forms.Screen.AllScreens.Length >= 2)
            {
                STATICS.SCREEN_WIDTH = System.Windows.Forms.Screen.AllScreens[1].Bounds.Width;
                STATICS.SCREEN_HEIGHT = System.Windows.Forms.Screen.AllScreens[1].Bounds.Height;
                STATICS.SCREEN_NUM = 2;
                Console.WriteLine(STATICS.SCREEN_WIDTH + " " + STATICS.SCREEN_HEIGHT);
                STATICS.DEAULT_CARD_SIZE = new Size(0.08333 * STATICS.SCREEN_WIDTH, 0.11111 * STATICS.SCREEN_HEIGHT);
                STATICS.DEAULT_CARD_SIZE_WITH_BORDER = new Size(0.08333 * STATICS.SCREEN_WIDTH + 10, 0.11111 * STATICS.SCREEN_HEIGHT + 10); 
                System.Drawing.Rectangle screenBounds = System.Windows.Forms.Screen.AllScreens[1].Bounds;
                this.Left = screenBounds.Left;
                this.Top = screenBounds.Top;
            }
            else
            {
                STATICS.SCREEN_WIDTH = (int)SystemParameters.PrimaryScreenWidth;
                STATICS.SCREEN_HEIGHT = (int)SystemParameters.PrimaryScreenHeight;
                STATICS.SCREEN_NUM = 1;
                STATICS.DEAULT_CARD_SIZE = new Size(0.08333 * STATICS.SCREEN_WIDTH, 0.11111 * STATICS.SCREEN_HEIGHT);
                STATICS.DEAULT_CARD_SIZE_WITH_BORDER = new Size(0.08333 * STATICS.SCREEN_WIDTH + 10, 0.11111 * STATICS.SCREEN_HEIGHT + 10);
                this.Width = STATICS.SCREEN_WIDTH;
                this.Height = STATICS.SCREEN_HEIGHT;
                this.WindowState = System.Windows.WindowState.Maximized;
                this.Left = 0;
            }
            boundary = new Rect(0, 0, STATICS.SCREEN_WIDTH, STATICS.SCREEN_HEIGHT);
            Stylus.SetIsPressAndHoldEnabled(this, false);
            Stylus.SetIsTapFeedbackEnabled(this, false);
            Stylus.SetIsFlicksEnabled(this, false);
            Stylus.SetIsTouchFeedbackEnabled(this, false);

            this.Loaded += Window_Loaded; 
            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = 28 });
            controlWindow = new Control_Window(this);
            controlWindow.Show();

            this.Visibility = Visibility.Hidden;
        }
        public void InitializeViews()
        {
            cardLayer = new Card_Layer(this);                        
            menuLayer = new Menu_Layer(this);            
            gestureIndicatorLayer = new Gesture_Indicator_Layer();
            bottomLayer = new Bottom_Layer();
            linkingGestureLayer = new Linking_Gesture_Layer();
            groupingGestureLayer = new Grouping_Gesture_Layer();
            sortingGestureLayer = new Sorting_Gesture_Layer(this);

            MainContainer.Children.Add(bottomLayer);
            MainContainer.Children.Add(linkingGestureLayer);
            MainContainer.Children.Add(sortingGestureLayer);
            MainContainer.Children.Add(groupingGestureLayer);
            MainContainer.Children.Add(cardLayer);
            MainContainer.Children.Add(gestureIndicatorLayer);
            MainContainer.Children.Add(menuLayer);
        }

        internal void Reinitialize()
        {
            DeinitViews();
            InitializeViews();
            loaders.PaperCardLoader.LoadCardLayout(fileName);
        }

        public void DeinitViews() {
            MainContainer.Children.Remove(bottomLayer);
            MainContainer.Children.Remove(linkingGestureLayer);
            MainContainer.Children.Remove(sortingGestureLayer);
            MainContainer.Children.Remove(groupingGestureLayer);
            MainContainer.Children.Remove(cardLayer);
            MainContainer.Children.Remove(gestureIndicatorLayer);
            MainContainer.Children.Remove(menuLayer);
            sortingGestureLayer = null;            
            groupingGestureLayer = null;
            linkingGestureLayer = null;
            bottomLayer = null;
            gestureIndicatorLayer = null;
            menuLayer = null;
            cardLayer = null;
           

            Card_List.CardList.Clear();            
            Gesture_List.GestureList.Clear();
            Group_List.CardGroups.Clear();
            Group_List.GroupBox.Clear();
            Link_List.CardLinks.Clear();
            Point_List.TouchPointList.Clear();                       
        }

        internal void Start(String layoutFile) {
            InitializeViews();
            fileName = layoutFile;
            Controlers = new Controlers(this);
            Controlers.Initialize();
            Loaders = new Loaders(this);
            Loaders.Initialize(layoutFile);
            this.Visibility = Visibility.Visible;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            //Mouse.OverrideCursor = Cursors.None;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            //Mouse.OverrideCursor = Cursors.Arrow;
            base.OnMouseLeave(e);
        }
    }
}
