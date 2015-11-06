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
    class Document_Card:Card
    {
        String text;
        TextBlock textBlock;

        public String Text
        {
            get { return text; }
            set { text = value; }
        }
        public Document_Card(Card_Controler controler):base(controler){
            textBlock = new TextBlock();
            textBlock.Width = STATICS.DEAULT_CARD_SIZE.Width-2;
            textBlock.Height = STATICS.DEAULT_CARD_SIZE.Height-2;
            textBlock.Foreground = new SolidColorBrush(Colors.Black);
            textBlock.FontSize = 7;
            textBlock.LineHeight = 9;
            textBlock.TextWrapping = TextWrapping.Wrap;
            Matrix mx = new Matrix();
            mx.Translate(-textBlock.Width / 2, -textBlock.Height / 2);
            textBlock.RenderTransform = new MatrixTransform(mx);
        }
        public override void InitializeCard(Color? maskColor, Point defaultPosi, double defaultDegree, double defaultScale, int zidx)
        {
            this.textBlock.Text = text;
            base.InitializeCard(maskColor, defaultPosi, defaultDegree, defaultScale, zidx);
            base.Container.Children.Add(textBlock);
        }
    }
}
