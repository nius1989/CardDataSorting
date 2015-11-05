using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CardDesign
{
    public class Controlers
    {
        private Touch_Controler touchControler;

        public Touch_Controler TouchControler
        {
            get { return touchControler; }
            set { touchControler = value; }
        }
        private Gesture_Controler gestureControler;

        public Gesture_Controler GestureControler
        {
            get { return gestureControler; }
            set { gestureControler = value; }
        }
        private Card_Controler cardControler;

        public Card_Controler CardControler
        {
            get { return cardControler; }
            set { cardControler = value; }
        }
        private User_Controler userControler;

        public User_Controler UserControler
        {
            get { return userControler; }
            set { userControler = value; }
        }

        Sorting_Group_Controler sortingBoxControler;

        internal Sorting_Group_Controler SortingBoxControler
        {
            get{return sortingBoxControler;}
            set{sortingBoxControler = value;}
        }

        public MainWindow MainWindow
        {
            get
            {
                return mainWindow;
            }

            set
            {
                mainWindow = value;
            }
        }


        MainWindow mainWindow;

        public Controlers(MainWindow mainWindow)
        {

            STATICS.USER_COLOR["Alex"] = Color.FromArgb(255, 255, 102, 0);
            STATICS.USER_COLOR["Ben"] = Color.FromArgb(255, 0, 255, 0);
            STATICS.USER_COLOR["Chris"] = Color.FromArgb(255, 255, 255, 255);
            STATICS.USER_COLOR["Danny"] = Color.FromArgb(255, 128, 0, 128);
            this.MainWindow = mainWindow;
            
        }
        public void InitializeControlers() {
            userControler = new User_Controler(this);
            cardControler = new Card_Controler(this);
            cardControler.Start();
            touchControler = new Touch_Controler(this);
            gestureControler = new Gesture_Controler(this);
            gestureControler.Start();
            sortingBoxControler = new Sorting_Group_Controler(this);
        }
        public void Deinitialize()
        {
            Point_List.Clear();
            Link_List.CardLinks.Clear();
            Group_List.CardGroups.Clear();
            Card_List.CardList.Clear();
            cardControler.Quit();
            cardControler = null;
            gestureControler.quit();
            gestureControler = null;
            userControler.UserList.Clear();
            userControler = null;
            touchControler = null;
        }
    }
}
