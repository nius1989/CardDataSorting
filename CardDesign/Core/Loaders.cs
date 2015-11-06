using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    public class Loaders
    {
        Card_Loader cardLoader;
        Document_Card_Loader documentCardLoader;

        internal Document_Card_Loader DocumentCardLoader
        {
            get { return documentCardLoader; }
            set { documentCardLoader = value; }
        }
        public Card_Loader CardLoader
        {
            get { return cardLoader; }
            set { cardLoader = value; }
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
            cardLoader = new Card_Loader(this);
            groupBinLoader = new Sorting_Group_Loader(this);
            documentCardLoader = new Document_Card_Loader(this);
        }
        internal void Initialize(String file)
        {
            //mainWindow.Loaders.CardLoader.LoadCardLayout(file);
            mainWindow.Loaders.DocumentCardLoader.LoadCardLayout(file);
            //mainWindow.Loaders.GroupBinControler.InitializeSortButton();       
        }
        internal void Deinitialize()
        {
            
        }
    }
}
