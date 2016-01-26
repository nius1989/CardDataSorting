using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CardDesign
{
    public class Card_Controler
    {
        Controlers control;
        DateTime[] startTimes = new DateTime[STATICS.CARD_NUMBER];

        public Controlers Control
        {
            get
            {
                return control;
            }

            set
            {
                control = value;
            }
        }

        public Card_Controler(Controlers control) {
            this.Control = control;
        }
        public News_Card SearchCard(string owner,string newsID) {
            var cards = Card_List.CardList.FindAll(s => s.Owner == owner);
            foreach (News_Card nc in cards) {
                if (nc.NewsID == newsID) {
                    return nc;
                }
            }
            return null;
        }
        public Card CopyCard(Card card)
        {
            Card cardToBeAdd = null;
            Control.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                Card cardToBeCyp = card;
                cardToBeAdd = new Card(this);
                cardToBeAdd.InitializeCard(
                                cardToBeCyp.BackgroundColor,
                                cardToBeCyp.CurrentPosition,
                                cardToBeCyp.CurrentRotation,
                                cardToBeCyp.CurrentScale,
                                cardToBeCyp.ZIndex);
                cardToBeAdd.UUID = cardToBeCyp.UUID+"_Copy";
                cardToBeAdd.Owner = cardToBeCyp.Owner;
                Card_List.AddCard(cardToBeAdd);
                Control.UserControler.ReceiveCard(cardToBeAdd.Owner, cardToBeAdd);
                Canvas.SetZIndex(cardToBeCyp, Card_List.CardList.Count);
                Canvas.SetZIndex(cardToBeAdd, cardToBeAdd.ZIndex);
                Control.MainWindow.CardLayer.AddCard(cardToBeAdd);
            }));
            return cardToBeAdd;
        }

        public void TouchDownCard(Canvas element, TouchEventArgs e)
        {
            TouchPoint point = e.GetTouchPoint(control.MainWindow.CardLayer);
            control.TouchControler.TouchDown(element, element.GetType(), e.TouchDevice.Id, point);
            control.MainWindow.ControlWindow.UpdateTextInfo(control.TouchControler.ToString(), 1);
            if (element is Linking_Icon || element is Copy_Icon)
            {
                Matrix mtx = (element.RenderTransform as MatrixTransform).Matrix;
                mtx.ScaleAt(1.5, 1.5, mtx.OffsetX , mtx.OffsetY );
                element.RenderTransform = new MatrixTransform(mtx);
                control.MainWindow.ControlWindow.UpdateTextInfo(control.TouchControler.ToString(), 1);            
            }
        }

        public void TouchMoveCard(Canvas element, TouchEventArgs e)
        {
            TouchPoint point = e.GetTouchPoint(control.MainWindow.CardLayer);
            control.TouchControler.TouchMove(this, this.GetType(), e.TouchDevice, point);
            control.MainWindow.ControlWindow.UpdateTextInfo(control.TouchControler.ToString(), 1);
        }

        public void TouchUpCard(Canvas element, TouchEventArgs e)
        {
            TouchPoint point = e.GetTouchPoint(control.MainWindow.CardLayer);
            control.TouchControler.TouchUp(e.TouchDevice, point);
            control.MainWindow.ControlWindow.UpdateTextInfo(control.TouchControler.ToString(), 1);
            if (element is Linking_Icon||element is Copy_Icon) {
                Matrix mtx = (element.RenderTransform as MatrixTransform).Matrix;
                mtx.ScaleAt(1.0 / 1.5, 1.0 / 1.5, mtx.OffsetX, mtx.OffsetY);
                element.RenderTransform = new MatrixTransform(mtx);
            }
        }

        internal static void UpdateZoomWheels(string owner, string newsID, double scale)
        {
            var cards = Card_List.CardList.FindAll(s => s.Owner != owner);
            foreach (News_Card nc in cards)
            {
                if (nc.NewsID == newsID)
                {
                    nc.UpdateZoomWheel(owner, scale);
                }
            }
        }
    }
}
