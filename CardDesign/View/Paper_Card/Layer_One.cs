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
    class Layer_One : Layer_Base
    {
        TextBlock textBlock = new TextBlock();
        public Layer_One():base() {
            textBlock = new TextBlock();
            textBlock.Width = STATICS.DEAULT_CARD_SIZE.Width;
            textBlock.Foreground = new SolidColorBrush(Colors.Black);
            textBlock.LineHeight = 1;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.FontSize = 20;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontStretch = FontStretches.Normal;
            textBlock.FontWeight = FontWeights.Bold;
            this.Children.Add(textBlock);
        }

        public override void SetPaper(My_Paper paper)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                textBlock.Text = paper.Title;
                if (paper.Title.Length > 50)
                {
                    textBlock.FontSize = 15;

                }
                if (paper.Title.Length > 100)
                {
                    textBlock.FontSize = 12;
                
                }
            }));
        }
    }
}
