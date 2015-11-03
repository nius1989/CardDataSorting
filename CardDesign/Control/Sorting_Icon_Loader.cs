using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CardDesign
{
    /// <summary>
    /// The class to initialize the sorting icons
    /// </summary>
    class Sorting_Icon_Loader
    {
        public Sorting_Icon_Loader(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        MainWindow mainWindow;

        public static String[] ButtontText = new String[]{
                "Physical","Mental","Social"
               };

        public static String[] ButtonTextBrif = new String[]{
                "Phy","Men","Soc"
               };


        public MainWindow MainWindow
        {
            get { return mainWindow; }
            set { mainWindow = value; }
        }

        public void InitializeSortButton()
        {
            Menu_Layer menuLayer = mainWindow.MenuLayer;
            int index = 0;

                for (char ch = 'A'; ch <= 'C'; ch++)
                {
                    String groupid = ch.ToString();
                    double x = (STATICS.SCREEN_WIDTH - STATICS.MENU_BAR_SIZE.Width) / 2;
                    double y = STATICS.SCREEN_HEIGHT/2;
                    Group_List.GroupButtons[groupid] = new Menu_Sort_Box(menuLayer, "", "" + ch, ButtontText[index], ButtonTextBrif[index]);
                    Group_List.GroupButtons[groupid].IsManipulationEnabled = true;
                    Group_List.GroupButtons[groupid].IsHitTestVisible = true;
                    Matrix matrix = new Matrix(1, 0, 0, 1, x + index * (STATICS.MENU_BAR_SIZE.Width-Group_List.GroupButtons[groupid].Width)/2, y);
                    Group_List.GroupButtons[groupid].RenderTransform = new MatrixTransform(matrix);
                    Group_List.GroupButtons[groupid].SetStartPosition(matrix.OffsetX + Group_List.GroupButtons[ch.ToString()].Width / 2,
                        matrix.OffsetY + Group_List.GroupButtons[ch.ToString()].Height / 2);
                    menuLayer.AddGroupButton(Group_List.GroupButtons[ch.ToString()]);
                    index++;
                }
           
        }
    }
}
