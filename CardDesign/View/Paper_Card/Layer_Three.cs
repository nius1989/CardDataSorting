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
    class Layer_Three : Layer_Base
    {
        TextBlock titleBlock = new TextBlock();
        TextBlock authorBlock = new TextBlock();
        TextBlock abstrackBlock = new TextBlock();
        public Layer_Three():base()
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

            authorBlock = new TextBlock();
            authorBlock.Width = STATICS.DEAULT_CARD_SIZE.Width;
            authorBlock.Foreground = new SolidColorBrush(Colors.Black);
            authorBlock.LineHeight = 1;
            authorBlock.TextWrapping = TextWrapping.Wrap;
            authorBlock.FontSize = 7;
            authorBlock.HorizontalAlignment = HorizontalAlignment.Center;
            authorBlock.FontStretch = FontStretches.Normal;
            authorBlock.TextAlignment = TextAlignment.Center;

            abstrackBlock = new TextBlock();
            abstrackBlock.Width = STATICS.DEAULT_CARD_SIZE.Width-4;
            abstrackBlock.Foreground = new SolidColorBrush(Colors.Black);
            abstrackBlock.LineHeight = 1;
            abstrackBlock.TextWrapping = TextWrapping.Wrap;
            abstrackBlock.TextAlignment = TextAlignment.Justify;
            abstrackBlock.FontSize = 4;
            abstrackBlock.HorizontalAlignment = HorizontalAlignment.Center;
            abstrackBlock.VerticalAlignment = VerticalAlignment.Center;
            abstrackBlock.FontStretch = FontStretches.Normal;
            this.Children.Add(titleBlock);
            this.Children.Add(authorBlock);
            this.Children.Add(abstrackBlock);
            
            RowDefinition rd = new RowDefinition();
            rd.Height = GridLength.Auto;
            this.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = GridLength.Auto;
            this.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = GridLength.Auto;
            this.RowDefinitions.Add(rd);

            Grid.SetRow(titleBlock, 0);
            Grid.SetRow(authorBlock, 1);
            Grid.SetRow(abstrackBlock, 2);
        }
        public override void SetPaper(My_Paper paper)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.titleBlock.Text = paper.Title;
                this.authorBlock.Text = String.Join(",", paper.Author.Select(s => s.Name));
                this.abstrackBlock.Text = paper.AbstractText;
            }));
        }
    }
}
