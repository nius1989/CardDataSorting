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

        Sorting_Icon_Loader iconLoader;
        internal Sorting_Icon_Loader IconControler
        {
            get { return iconLoader; }
            set { iconLoader = value; }
        }

        public Loaders(MainWindow mainWindow)
        {
            cardLoader = new Card_Loader(mainWindow);
            iconLoader = new Sorting_Icon_Loader(mainWindow);
        }

        internal void Deinitialize()
        {
        }
    }
}
