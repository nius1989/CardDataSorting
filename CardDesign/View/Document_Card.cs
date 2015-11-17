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
        TextBlock textBlock;
        My_Doc doc;
        double maxRate = -1;
        internal My_Doc Doc
        {
            get { return doc; }
            set { doc = value; }
        }
        public Document_Card(Card_Controler controler):base(controler){
            textBlock = new TextBlock();
            textBlock.Width = STATICS.DEAULT_CARD_SIZE.Width;
            textBlock.Height = STATICS.DEAULT_CARD_SIZE.Height;
            textBlock.Foreground = new SolidColorBrush(Colors.Black);
            textBlock.LineHeight = 1;
            textBlock.TextWrapping = TextWrapping.Wrap; 
            textBlock.FontSize = 30;
            textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
            textBlock.FontStretch = FontStretches.Normal;
            textBlock.FontStyle = FontStyles.Normal;
            Matrix mx = new Matrix();
            mx.Translate(-textBlock.Width / 2, -textBlock.Height / 2);
            textBlock.RenderTransform = new MatrixTransform(mx);
        }
        public override void InitializeCard(Color? maskColor, Point defaultPosi, double defaultDegree, double defaultScale, int zidx)
        {
            maxRate = doc.Content.Max(s => s.Ranking);
            this.textBlock.Text = doc.GetOringinalStringContent();
            base.InitializeCard(maskColor, defaultPosi, defaultDegree, defaultScale, zidx);
            base.Container.Children.Add(textBlock);
            UpdateText();
        }

        public void UpdateText() {
            double rate = (CurrentScale-STATICS.MIN_CARD_SCALE) / (STATICS.MAX_CARD_SCALE-STATICS.MIN_CARD_SCALE);
            int cutIndex = (int)(rate * rate * this.doc.Content.Length) + 1;
            foreach (My_Word mw in this.doc.Content) {
                if (mw.Index <= cutIndex)
                    mw.IsVisible = true;
                else
                    mw.IsVisible = false;
            }
            this.textBlock.Text = doc.GetOringinalStringContent();
            adjustFontSize();
        }

        private void adjustFontSize()
        {
            // Set up string. 
            double boxWidth = textBlock.Width;
            double boxHeight = textBlock.Height;

            String currentString = textBlock.Text;
            double fsize = 70;
            double n = 0;
            double tHeight = 0;
            Size size = new Size();
            while (fsize > 7)
            {
                var formattedText = new FormattedText(
                    currentString,
                    System.Globalization.CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(this.textBlock.FontFamily, this.textBlock.FontStyle, this.textBlock.FontWeight, this.textBlock.FontStretch),
                    fsize,
                    Brushes.Black);

                size = new Size(formattedText.Width, formattedText.Height);
                n = Math.Ceiling(size.Width / boxWidth);
                tHeight = n * size.Height + (n + 1) * textBlock.LineHeight;
                if (tHeight < boxHeight)
                {
                    break;
                }
                else
                    fsize--;
            }
            //Console.WriteLine("(" + size.Width + "," + size.Height + ") "
            //    + fsize + " " + n + " " + tHeight +
            //    " (" + boxWidth + "," + boxHeight + ")");
            textBlock.FontSize = fsize - 1; 
            textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
            textBlock.FontStretch = FontStretches.Normal;
            textBlock.FontStyle = FontStyles.Normal;
        }
    }
}
