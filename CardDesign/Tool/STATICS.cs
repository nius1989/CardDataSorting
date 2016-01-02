using System.Collections.Generic;

namespace CardDesign
{
    class STATICS
    {
        public static bool DEBUG_MODE = true;
        public static int SCREEN_WIDTH = 1920;
        public static int SCREEN_HEIGHT = 1080;
        public static int SCREEN_NUM = 1;
        public static System.Windows.Size DEAULT_CARD_SIZE = new System.Windows.Size(160,120);//default card size
        public static System.Windows.Size DEAULT_CARD_SIZE_WITH_BORDER = new System.Windows.Size(170,130);//default card size
        public static double POINT_REFRESH_RATE = 100;//refresh the data every "refreshRate" millionseconds
        public static int GESTURE_REFRESH_RATE = 80;
        public static int MAX_TOUCH_POINT = 12;//Maximum Touch Points Allowed
        public static double MIN_GESTURE_LIFE = 0;//detect gesture after xx millionseconds
        public static double MIN_DISTANCE_FOR_MOVE = 30;
        public static double MIN_LONG_PRESS_LIFE = 300;//Min time to active emphasizing gesture
        public static double MIN_SHOW_LIFE = 1000;//Min time to active showing gesture
        public static double START_CARD_BRIGHT = 1.0;
        public static int CARD_NUMBER = 16;
        public static double MAX_CARD_SCALE = 3;
        public static double MIN_CARD_SCALE = 0.5;
        public static double ANIMATION_DURATION = 0.5;
        public static Dictionary<string, System.Windows.Media.Color> USER_COLOR = new Dictionary<string, System.Windows.Media.Color>();
        public static System.Windows.Media.Color[] USER_COLOR_CODE = new System.Windows.Media.Color[] {
            System.Windows.Media.Color.FromArgb(255,255,0,0),
            System.Windows.Media.Color.FromArgb(255,255,255,0),
            System.Windows.Media.Color.FromArgb(255, 0, 255, 0),
            System.Windows.Media.Color.FromArgb(255,0,0,255)};
        public static string[] USER_IDS = new string[] { "Alex", "Ben", "Chris", "Danny" };
        public static bool ALEX_ACTIVE = true;
        public static bool BEN_ACTIVE = false;
        public static bool CHRIS_ACTIVE = true;
        public static bool DANNY_ACTIVE = true;
        public static bool[] USER_ACTIVE = new bool[] { ALEX_ACTIVE, BEN_ACTIVE, CHRIS_ACTIVE, DANNY_ACTIVE };
        public static int USER_NUMER = 3;
        public static System.Drawing.Size MENU_BAR_SIZE = new System.Drawing.Size(600, 50);
        //public static System.Drawing.Rectangle COLLABORATIVE_ZOON = new System.Drawing.Rectangle(STATICS.SCREEN_WIDTH / 4, 0, STATICS.SCREEN_WIDTH / 2, 3 * STATICS.SCREEN_HEIGHT / 4);
        public static System.Drawing.Rectangle COLLABORATIVE_ZOON = new System.Drawing.Rectangle(300, 0, 500, 300);
        public static int NEWS_NUMBER = 15;
    }
}
