using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CardDesign
{
    class Layer_Two : Layer_Base
    {
        TextBlock titleBlock = new TextBlock();
        ZoomWheel[] zoomWheels;
        string userId="Alex";
        public Layer_Two(string userId)
            : base()
        {
            

            this.userId = userId;
            titleBlock = new TextBlock();
            titleBlock.Width = STATICS.DEAULT_CARD_SIZE.Width;
            titleBlock.Foreground = new SolidColorBrush(Colors.Black);
            titleBlock.LineHeight = 1;
            titleBlock.TextWrapping = TextWrapping.Wrap;
            titleBlock.HorizontalAlignment = HorizontalAlignment.Left;
            titleBlock.TextAlignment = TextAlignment.Left;
            titleBlock.FontStretch = FontStretches.Normal;
            this.Children.Add(titleBlock);
            Grid.SetRow(titleBlock, 0);
            Grid.SetColumn(titleBlock, 0);
            Grid.SetColumnSpan(titleBlock, 2);

            List<ZoomWheel> templist = new List<ZoomWheel>();
            int index = 0;
            for (int i = 0; i < STATICS.USER_IDS.Length; i++)
            {
                if (!STATICS.USER_IDS[i].Equals(userId) && STATICS.USER_ACTIVE[i])
                {
                    ZoomWheel zw = new ZoomWheel(STATICS.USER_IDS[i]);
                    zw.VerticalAlignment = VerticalAlignment.Center;
                    zw.HorizontalAlignment = HorizontalAlignment.Center;
                    zw.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1, zw.Width / 2, zw.Height / 2));
                    templist.Add(zw);
                    this.Children.Add(zw);
                    Grid.SetRow(zw, 1);
                    Grid.SetColumn(zw, index);
                    index++;
                }
            }
            zoomWheels = templist.ToArray();
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(STATICS.DEAULT_CARD_SIZE.Height-zoomWheels[0].Height);
            this.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = new GridLength(zoomWheels[0].Height);
            this.RowDefinitions.Add(rd);
            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(this.Width / 2);
            this.ColumnDefinitions.Add(gridCol1);
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol2.Width = new GridLength(this.Width / 2);
            this.ColumnDefinitions.Add(gridCol2);
        }
        public override void SetArticle(My_News news)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.titleBlock.Text = news.Title;
                if (news.Title.Length > 50)
                {
                    titleBlock.FontSize = 5;

                }
                if (news.Title.Length > 100)
                {
                    titleBlock.FontSize = 2;
                }
            }));
        }
    }
}
