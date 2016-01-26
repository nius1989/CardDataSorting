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
        public Layer_Two()
            : base()
        {       
            titleBlock = new TextBlock();
            titleBlock.Width = STATICS.DEAULT_CARD_SIZE.Width;
            titleBlock.Foreground = new SolidColorBrush(Colors.Black);
            titleBlock.LineHeight = 1;
            titleBlock.TextWrapping = TextWrapping.Wrap;
            titleBlock.HorizontalAlignment = HorizontalAlignment.Left;
            titleBlock.TextAlignment = TextAlignment.Left;
            titleBlock.FontStretch = FontStretches.Normal;
            titleBlock.Margin = new Thickness(3);
            this.Children.Add(titleBlock);
            Grid.SetRow(titleBlock, 0);
            Grid.SetColumn(titleBlock, 0);
            Grid.SetColumnSpan(titleBlock, 2);

            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(STATICS.DEAULT_CARD_SIZE.Height-1.5* STATICS.ZOOMWHEEL_RADIUS);
            this.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = new GridLength(STATICS.ZOOMWHEEL_RADIUS);
            this.RowDefinitions.Add(rd);
            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(this.Width / 2);
            this.ColumnDefinitions.Add(gridCol1);
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol2.Width = new GridLength(this.Width / 2);
            this.ColumnDefinitions.Add(gridCol2);
        }
        public override void AddZoomWheel(ZoomWheel[] zm)
        {
            this.zoomWheels = zm;
            for (int i = 0; i < zoomWheels.Length; i++)
            {
                this.Children.Add(zoomWheels[i]);
                Grid.SetRow(zoomWheels[i], 1);
                Grid.SetColumn(zoomWheels[i], i);
            }
        }
        public override void RemoveZoomWheel()
        {
            foreach (ZoomWheel zm in zoomWheels) {
                this.Children.Remove(zm);
            }
        }
        public override void SetArticle(My_News news)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.titleBlock.Text = news.Title;
                if (news.Title.Length > 50)
                {
                    titleBlock.FontSize = 15;

                }
                if (news.Title.Length > 100)
                {
                    titleBlock.FontSize = 10;
                }
            }));
        }
    }
}
