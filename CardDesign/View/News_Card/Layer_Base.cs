using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CardDesign
{
    class Layer_Base : Grid
    {
        public Layer_Base()
        {
            this.Width = STATICS.DEAULT_CARD_SIZE.Width;
            this.Height = STATICS.DEAULT_CARD_SIZE.Height;
            Matrix mx = new Matrix();
            mx.Translate(-this.Width / 2, -this.Height / 2);
            this.RenderTransform = new MatrixTransform(mx);
        }
        public virtual void SetArticle(My_News paper)
        {
        }
    }
}
