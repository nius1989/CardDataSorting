using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WordCloud
{
    public class STATICS
    {
        public static double SCREEN_WIDTH = 0;
        public static double SCREEN_HEIGHT = 0;
        public static Color[] USER_COLOR = new Color[] {
            Color.FromArgb(255,255,174,174),
            Color.FromArgb(255,255,255,0),
            Color.FromArgb(255, 176, 229, 124),
            Color.FromArgb(255,86,186,236)};
        public static int MIN_FONT_SIZE = 20;
        public static int MAX_FONT_SIZE = 60;
        public static int MIN_GLOW_SIZE = 100;
        public static int MAX_GLOW_SIZE = 400;
        public static double DEFAULT_GLOW_DIAMETER = 100;
        internal static string FILE_LOC= @"Resource\Data\newsDocs.txt";
        internal static int WORD_COUNT=40;
    }
}
