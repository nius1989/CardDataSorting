using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    public class Loaders
    {
        News_Card_Loader newsCardLoader;

        internal News_Card_Loader NewsCardLoader
        {
            get { return newsCardLoader; }
            set { newsCardLoader = value; }
        }

        Sorting_Group_Loader groupBinLoader;
        internal Sorting_Group_Loader GroupBinControler
        {
            get { return groupBinLoader; }
            set { groupBinLoader = value; }
        }
        MainWindow mainWindow;

        public MainWindow MainWindow
        {
            get { return mainWindow; }
            set { mainWindow = value; }
        }
        public Loaders(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            groupBinLoader = new Sorting_Group_Loader(this);
            newsCardLoader = new News_Card_Loader(this);
        }
        internal void Initialize(String file)
        {
            newsCardLoader.LoadCardLayout(file);     
        }
        internal void Deinitialize()
        {
            
        }
    }
}
