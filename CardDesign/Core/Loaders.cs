using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Loaders
    {
        Card_Loader cardLoader;
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

        public Loaders(MainWindow mainWindow)
        {
            cardLoader = new Card_Loader(mainWindow);
            groupBinLoader = new Sorting_Group_Loader(mainWindow);
        }

        internal void Deinitialize()
        {
        }
    }
}
