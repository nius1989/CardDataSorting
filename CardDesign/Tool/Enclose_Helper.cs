using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Enclose_Helper
    {
        public static bool PNPoly(My_Point[] Newpoint, double testx, double testy)
        {
            int i, j;
            bool c = false;
            for (i = 0, j = 3; i < 4; j = i++)
            {
                if (((Newpoint[i].CurrentPoint.Position.Y > testy) != (Newpoint[j].CurrentPoint.Position.Y > testy)) &&
               (testx < (Newpoint[j].CurrentPoint.Position.X - Newpoint[i].CurrentPoint.Position.X) * (testy - Newpoint[i].CurrentPoint.Position.Y) / (Newpoint[j].CurrentPoint.Position.Y - Newpoint[i].CurrentPoint.Position.Y) + Newpoint[i].CurrentPoint.Position.X))
                    c = !c;
            }
            return c;
        }
    }
}
