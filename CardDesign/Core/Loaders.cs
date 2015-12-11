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
        Paper_Card_Loader paperCardLoader;

        internal Paper_Card_Loader PaperCardLoader
        {
            get { return paperCardLoader; }
            set { paperCardLoader = value; }
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
            paperCardLoader = new Paper_Card_Loader(this);
        }
        internal void Initialize(String file)
        {
            //cardLoader.LoadCardLayout(file);
            //documentCardLoader.LoadCardLayout(file);
            paperCardLoader.LoadCardLayout(file);
            //groupBinControler.InitializeSortButton();       
        }
        internal void Deinitialize()
        {
            
        }
    }
}
