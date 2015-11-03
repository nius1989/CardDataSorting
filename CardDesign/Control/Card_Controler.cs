using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CardDesign
{
    class Card_Controler
    {
        MainWindow mainWindow;
        Thread jointInterestDetectionThread;
        bool isRunning = false;
        DateTime[] startTimes = new DateTime[STATICS.CARD_NUMBER];

        public Card_Controler(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
        }

        public void Start()
        {
            if (isRunning)
            {
                isRunning = false;
                Thread.Sleep(100);
                jointInterestDetectionThread.Abort();
            }
            for (int i = 0; i < startTimes.Length; i++)
            {
                startTimes[i] = DateTime.MaxValue;
            }
            jointInterestDetectionThread = new Thread(new ThreadStart(scan));
            isRunning = true;
            jointInterestDetectionThread.Start();

        }
        public void Quit() {
            if (jointInterestDetectionThread.IsAlive) {
                jointInterestDetectionThread.Abort();
                jointInterestDetectionThread = null;
            }
        }
        private void scan()
        {
            while (isRunning)
            {
                Thread.Sleep(5000);
                for (int cardIndex = 0; cardIndex < STATICS.CARD_NUMBER; cardIndex++)
                {
                    int countSame = 0;

                    if (STATICS.ALEX_ACTIVE && mainWindow.Controlers.UserControler.UserList["Alex"].Cards.Count > 0
                        && mainWindow.Controlers.UserControler.UserList["Alex"].Cards[cardIndex].Brightness > 0.8
                        && !mainWindow.Controlers.UserControler.UserList["Alex"].Cards[cardIndex].IsJointInterested)
                    {
                        countSame++;
                    }
                    if (STATICS.BEN_ACTIVE && mainWindow.Controlers.UserControler.UserList["Ben"].Cards.Count > 0
                        && mainWindow.Controlers.UserControler.UserList["Ben"].Cards[cardIndex].Brightness > 0.8
                        && !mainWindow.Controlers.UserControler.UserList["Ben"].Cards[cardIndex].IsJointInterested)
                    {
                        countSame++;
                    }
                    if (STATICS.CHRIS_ACTIVE && mainWindow.Controlers.UserControler.UserList["Chris"].Cards.Count > 0
                        && mainWindow.Controlers.UserControler.UserList["Chris"].Cards[cardIndex].Brightness > 0.8
                        && !mainWindow.Controlers.UserControler.UserList["Chris"].Cards[cardIndex].IsJointInterested)
                    {
                        countSame++;
                    }
                    if (STATICS.DANNY_ACTIVE && mainWindow.Controlers.UserControler.UserList["Danny"].Cards.Count > 0
                        && mainWindow.Controlers.UserControler.UserList["Danny"].Cards[cardIndex].Brightness > 0.8
                        && !mainWindow.Controlers.UserControler.UserList["Danny"].Cards[cardIndex].IsJointInterested)
                    {
                        countSame++;
                    }
                    if (countSame > 1)
                    {
                        if (STATICS.ALEX_ACTIVE)
                            mainWindow.Controlers.UserControler.UserList["Alex"].Cards[cardIndex].HightlightJointInterest();
                        if (STATICS.BEN_ACTIVE)
                            mainWindow.Controlers.UserControler.UserList["Ben"].Cards[cardIndex].HightlightJointInterest();
                        if (STATICS.CHRIS_ACTIVE)
                            mainWindow.Controlers.UserControler.UserList["Chris"].Cards[cardIndex].HightlightJointInterest();
                        if (STATICS.DANNY_ACTIVE)
                            mainWindow.Controlers.UserControler.UserList["Danny"].Cards[cardIndex].HightlightJointInterest();
                        startTimes[cardIndex] = DateTime.Now;
                        mainWindow.ControlWindow.UpdateTextInfo("Card " +
                            mainWindow.Controlers.UserControler.UserList["Alex"].Cards[cardIndex].UID + " is Joint interest @ " +
                            startTimes[cardIndex].ToString(), 2);

                    }
                }
                for (int i = 0; i < startTimes.Length; i++)
                {
                    DateTime time = startTimes[i];
                    if (time != DateTime.MaxValue)
                    {
                        if ((DateTime.Now - time).TotalSeconds > STATICS.JOINT_INTEREST_DURATION)
                        {
                            mainWindow.ControlWindow.UpdateTextInfo("Card " + mainWindow.Controlers.UserControler.UserList["Alex"].Cards[i].UID + " is time out", 2);
                            mainWindow.Controlers.UserControler.UserList["Alex"].Cards[i].DehightJointInterest();
                            mainWindow.Controlers.UserControler.UserList["Ben"].Cards[i].DehightJointInterest();
                            mainWindow.Controlers.UserControler.UserList["Chris"].Cards[i].DehightJointInterest();
                            mainWindow.Controlers.UserControler.UserList["Danny"].Cards[i].DehightJointInterest();
                            startTimes[i] = DateTime.MaxValue;
                        }
                    }
                }
            }
        }
        public Card CopyCard(Card card)
        {
            Card cardToBeAdd = null;
            mainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                Card cardToBeCyp = card;
                cardToBeAdd = new Card(mainWindow.Loaders.CardLoader);
                cardToBeAdd.InitializeCard(cardToBeCyp.ImgFile,
                                cardToBeCyp.CardText,
                                cardToBeCyp.BackgroundColor,
                                cardToBeCyp.CurrentPosition,
                                cardToBeCyp.CurrentRotation,
                                cardToBeCyp.CurrentScale,
                                cardToBeCyp.ZIndex);
                cardToBeAdd.UID = cardToBeCyp.UID;
                cardToBeAdd.Owner = cardToBeCyp.Owner;
                cardToBeAdd.CardText = cardToBeCyp.CardText;
                Card_List.AddCard(cardToBeAdd);
                mainWindow.Controlers.UserControler.ReceiveCard(cardToBeAdd.Owner, cardToBeAdd);
                Canvas.SetZIndex(cardToBeCyp, Card_List.CardList.Count);
                Canvas.SetZIndex(cardToBeAdd, cardToBeAdd.ZIndex);
                mainWindow.CardLayer.AddCard(cardToBeAdd);
            }));
            return cardToBeAdd;
        }
    }
}
