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
    class Sorting_Group_Loader
    {
        Loaders loader;
        public Sorting_Group_Loader(Loaders loader)
        {
            this.loader = loader;
        }


        public static String[] ButtontText = new String[]{
                "Physical","Mental","Social"
               };

        public static String[] ButtonTextBrif = new String[]{
                "Phy","Men","Soc"
               };

       
    }
}
