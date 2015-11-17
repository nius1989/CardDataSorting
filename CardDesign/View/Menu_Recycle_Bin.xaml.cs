﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CardDesign
{
    /// <summary>
    /// Interaction logic for Menu_Recycle_Bin.xaml
    /// </summary>
    public partial class Menu_Recycle_Bin : Canvas
    {
        double xCoord;
        double yCoord;

        public double XCoord
        {
            get { return xCoord; }
            set { xCoord = value; }
        }

        public double YCoord
        {
            get { return yCoord; }
            set { yCoord = value; }

        }
        public Menu_Recycle_Bin()
        {
            InitializeComponent();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(@"Resource\Image\recycle_bin.png", UriKind.Relative));
            this.Background = ib;
        }
    }
}
