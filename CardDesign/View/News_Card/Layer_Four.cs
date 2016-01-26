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
    class Layer_Four : Layer_Base
    {
        TextBlock titleBlock = new TextBlock();
        TextBlock abstrackBlock = new TextBlock();
        ZoomWheel[] zoomWheels;
        public Layer_Four() :base()
        {            
            titleBlock = new TextBlock();
            titleBlock.Width = STATICS.DEAULT_CARD_SIZE.Width;
            titleBlock.Foreground = new SolidColorBrush(Colors.Black);
            titleBlock.LineHeight = 1;
            titleBlock.TextWrapping = TextWrapping.Wrap;
            titleBlock.FontSize = 8;
            titleBlock.HorizontalAlignment = HorizontalAlignment.Center;
            titleBlock.FontStretch = FontStretches.Normal;
            titleBlock.TextAlignment = TextAlignment.Center;
            titleBlock.FontWeight = FontWeights.Bold;

            abstrackBlock = new TextBlock();
            abstrackBlock.Width = STATICS.DEAULT_CARD_SIZE.Width-4;
            abstrackBlock.Foreground = new SolidColorBrush(Colors.Black);
            abstrackBlock.LineHeight = 1;
            abstrackBlock.TextWrapping = TextWrapping.Wrap;
            abstrackBlock.TextAlignment = TextAlignment.Justify;
            abstrackBlock.FontSize = 5;
            abstrackBlock.HorizontalAlignment = HorizontalAlignment.Center;
            abstrackBlock.VerticalAlignment = VerticalAlignment.Center;
            abstrackBlock.FontStretch = FontStretches.Normal;
            this.Children.Add(titleBlock);
            this.Children.Add(abstrackBlock);

            Grid.SetRow(titleBlock, 0);
            Grid.SetColumn(titleBlock, 0);
            Grid.SetColumnSpan(titleBlock, 2);

            Grid.SetRow(abstrackBlock, 1);
            Grid.SetColumn(abstrackBlock, 0);
            Grid.SetColumnSpan(abstrackBlock, 2);

            RowDefinition rd = new RowDefinition();
            rd.Height = GridLength.Auto;
            this.RowDefinitions.Add(rd);

            rd = new RowDefinition();
            rd.Height = GridLength.Auto;
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
                this.abstrackBlock.Text = news.Content;
            }));
        }
    }
}
