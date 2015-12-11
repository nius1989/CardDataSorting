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
        TextBlock authorBlock = new TextBlock();
        public Layer_Two():base()
        {
            titleBlock = new TextBlock();
            titleBlock.Width = STATICS.DEAULT_CARD_SIZE.Width;
            titleBlock.Foreground = new SolidColorBrush(Colors.Black);
            titleBlock.LineHeight = 1;
            titleBlock.TextWrapping = TextWrapping.Wrap;
            titleBlock.FontSize = 15;
            titleBlock.HorizontalAlignment = HorizontalAlignment.Center;
            titleBlock.TextAlignment = TextAlignment.Center;
            titleBlock.FontStretch = FontStretches.Normal;
            titleBlock.FontWeight = FontWeights.Bold;

            authorBlock = new TextBlock();
            authorBlock.Width = STATICS.DEAULT_CARD_SIZE.Width;
            authorBlock.Foreground = new SolidColorBrush(Colors.Black);
            authorBlock.LineHeight = 1;
            authorBlock.TextWrapping = TextWrapping.Wrap;
            authorBlock.FontSize = 9;
            authorBlock.TextAlignment = TextAlignment.Center;
            authorBlock.HorizontalAlignment = HorizontalAlignment.Center;
            authorBlock.FontStretch = FontStretches.Normal;
            this.Children.Add(titleBlock);
            this.Children.Add(authorBlock);

            RowDefinition rd = new RowDefinition();
            rd.Height = GridLength.Auto;
            this.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = GridLength.Auto;
            this.RowDefinitions.Add(rd);

            Grid.SetRow(titleBlock, 0);
            Grid.SetRow(authorBlock, 1);
        }
        public override void SetPaper(My_Paper paper)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.titleBlock.Text = paper.Title;
                this.authorBlock.Text = String.Join(",", paper.Author.Select(s => s.Name));
                if (paper.Title.Length > 50)
                {
                    titleBlock.FontSize = 13;

                }
                if (paper.Title.Length > 100)
                {
                    titleBlock.FontSize = 10;
                }
            }));
        }
    }
}
