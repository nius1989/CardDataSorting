using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace CardDesign
{
    /// <summary>
    /// A class to control the card.
    /// With provided configuration, generate the json file and save on the harddrive.
    /// Defines a DefaultCard as a json template, to generate json file
    /// </summary>
    public class Card_Loader
    {
        Loaders loader;

        internal Loaders Loader
        {
            get { return loader; }
            set { loader = value; }
        }
        static String configFileDir;
        public class LoadingCard
        {
            public String dir;
            public int[] position;
            public int rotate;
            public int[] color;
            public String cardID;
            public String userID;
            public String text;

        }

        public class SavingCard
        {
            public SavingCard(String dir, String cardID, String text, String userID, Color color)
            {
                this.dir = dir;
                this.cardID = cardID;
                this.text = text;
                this.userID = userID;
                this.color = color;
            }
            public String dir;
            public String cardID;
            public String text;
            public String userID;
            public Color color;
        }

        public Card_Loader(Loaders loader)
        {
            this.loader = loader;
        }
        public static String GenerateLayout(String savingFile, int lineNum, SavingCard[] savingCards)
        {
            String filedir = Path.Combine(Environment.CurrentDirectory, savingFile);
            int rowInv = 50;
            int cardPerPerson = 0;

            cardPerPerson = savingCards.Where(s => s.userID.Equals("Alex")).Count();
            int columNum = (cardPerPerson + 1) / lineNum;
            int columInv = (int)((STATICS.SCREEN_HEIGHT - STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width) / (columNum - 1));

            LoadingCard[] loadingCards = new LoadingCard[savingCards.Length];
            int Gx = (STATICS.SCREEN_WIDTH - STATICS.SCREEN_HEIGHT) / 2 + (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width / 2;
            int Gy = STATICS.SCREEN_HEIGHT - (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height / 2 - rowInv * (lineNum - 1);
            Random rand = new Random();
            StreamWriter streamWriter = new StreamWriter(filedir);
            if (STATICS.ALEX_ACTIVE)
            {
                int index = 0;
                for (int i = 0; i < loadingCards.Length; i++)
                {
                    if (savingCards[i].userID.Equals("Alex"))
                    {
                        loadingCards[i] = new LoadingCard();
                        loadingCards[i].dir = Path.Combine(Environment.CurrentDirectory, savingCards[i].dir);
                        loadingCards[i].position = new int[] { index % columNum * columInv + Gx, rowInv * (index / columNum) + Gy };
                        loadingCards[i].rotate = rand.Next(8) - 4;
                        loadingCards[i].color = new int[] { savingCards[i].color.R, savingCards[i].color.G, savingCards[i].color.B };
                        loadingCards[i].cardID = savingCards[i].cardID;
                        loadingCards[i].userID = savingCards[i].userID;
                        loadingCards[i].text = savingCards[i].text;
                        string result = JsonConvert.SerializeObject(loadingCards[i]);
                        streamWriter.WriteLine(result);
                        index++;
                    }
                }
            }

            
            cardPerPerson = 0;
            foreach (SavingCard c in savingCards)
            {
                if (c.userID.Equals("Ben"))
                {
                    cardPerPerson++;
                }
            }
            columNum = (cardPerPerson + 1) / lineNum;
            columInv = (int)((STATICS.SCREEN_HEIGHT - STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width) / (columNum - 1));
            if (STATICS.BEN_ACTIVE)
            {
                Gx = (STATICS.SCREEN_WIDTH + STATICS.SCREEN_HEIGHT) / 2 - (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width / 2;
                Gy = (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height / 2 + rowInv * (lineNum - 1);
                int index = 0;
                for (int i = 0; i < loadingCards.Length; i++)
                {
                    if (savingCards[i].userID.Equals("Ben"))
                    {
                        loadingCards[i] = new LoadingCard();
                        loadingCards[i].dir = Path.Combine(Environment.CurrentDirectory, savingCards[i].dir);
                        loadingCards[i].position = new int[] { Gx - index % columNum * columInv, Gy - rowInv * (index / columNum) };
                        loadingCards[i].rotate = rand.Next(8) - 4 + 180;
                        loadingCards[i].color = new int[] { savingCards[i].color.R, savingCards[i].color.G, savingCards[i].color.B };
                        loadingCards[i].cardID = savingCards[i].cardID;
                        loadingCards[i].userID = savingCards[i].userID;
                        loadingCards[i].text = savingCards[i].text;
                        string result = JsonConvert.SerializeObject(loadingCards[i]);
                        streamWriter.WriteLine(result);
                        index++;
                    }
                }
            }
            cardPerPerson = 0;
            foreach (SavingCard c in savingCards)
            {
                if (c.userID.Equals("Chris"))
                {
                    cardPerPerson++;
                }
            }
            columNum = (cardPerPerson + 1) / lineNum;
            columInv = (int)((STATICS.SCREEN_HEIGHT - STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width) / (columNum - 1));
            if (STATICS.CHRIS_ACTIVE)
            {
                Gx = (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height / 2 + rowInv * (lineNum - 1);
                Gy = (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width / 2;
                int index = 0;
                for (int i = 0; i < loadingCards.Length; i++)
                {
                    if (savingCards[i].userID.Equals("Chris"))
                    {
                        loadingCards[i] = new LoadingCard();
                        loadingCards[i].dir = Path.Combine(Environment.CurrentDirectory, savingCards[i].dir);
                        loadingCards[i].position = new int[] { Gx - rowInv * (index / columNum), Gy + index % columNum * columInv };
                        loadingCards[i].rotate = rand.Next(8) - 4 + 90;
                        loadingCards[i].color = new int[] { savingCards[i].color.R, savingCards[i].color.G, savingCards[i].color.B };
                        loadingCards[i].cardID = savingCards[i].cardID;
                        loadingCards[i].userID = savingCards[i].userID;
                        loadingCards[i].text = savingCards[i].text;
                        string result = JsonConvert.SerializeObject(loadingCards[i]);
                        streamWriter.WriteLine(result);
                        index++;
                    }
                }
            }
            cardPerPerson = 0;
            foreach (SavingCard c in savingCards)
            {
                if (c.userID.Equals("Danny"))
                {
                    cardPerPerson++;
                }
            }
            columNum = (cardPerPerson + 1) / lineNum;
            columInv = (int)((STATICS.SCREEN_HEIGHT - STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width) / (columNum - 1));
            if (STATICS.DANNY_ACTIVE)
            {
                Gx = STATICS.SCREEN_WIDTH - ((int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height / 2 + rowInv * (lineNum - 1));
                //Gx = 0;
                Gy = STATICS.SCREEN_HEIGHT - (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width / 2;
                int index = 0;
                for (int i = 0; i < loadingCards.Length; i++)
                {
                    if (savingCards[i].userID.Equals("Danny"))
                    {
                        loadingCards[i] = new LoadingCard();
                        loadingCards[i].dir = Path.Combine(Environment.CurrentDirectory, savingCards[i].dir);
                        loadingCards[i].position = new int[] { Gx + rowInv * (index / columNum), Gy - index % columNum * columInv };
                        loadingCards[i].rotate = rand.Next(8) - 4 + 270;
                        loadingCards[i].color = new int[] { savingCards[i].color.R, savingCards[i].color.G, savingCards[i].color.B };
                        loadingCards[i].cardID = savingCards[i].cardID;
                        loadingCards[i].userID = savingCards[i].userID;
                        loadingCards[i].text = savingCards[i].text;
                        string result = JsonConvert.SerializeObject(loadingCards[i]);
                        streamWriter.WriteLine(result);
                        index++;
                    }
                }
            }
            streamWriter.Flush();
            streamWriter.Close();
            return "";
        }
        public void LoadCardLayout(String dir)
        {
            configFileDir = Path.Combine(Environment.CurrentDirectory, dir);
            if (File.Exists(configFileDir))
            {
                StreamReader reader = new StreamReader(configFileDir);
                String nextLine = "";
                while ((nextLine = reader.ReadLine()) != null)
                {
                    LoadingCard readCard = JsonConvert.DeserializeObject<LoadingCard>(nextLine);
                    int zindex = 0;
                    if (readCard.userID == "Alex" && STATICS.ALEX_ACTIVE ||
                        readCard.userID == "Ben" && STATICS.BEN_ACTIVE ||
                        readCard.userID == "Chris" && STATICS.CHRIS_ACTIVE ||
                        readCard.userID == "Danny" && STATICS.DANNY_ACTIVE)
                    {
                        Card myCard = new Card(loader.MainWindow.Controlers.CardControler);
                        myCard.InitializeCard(
                            Color.FromArgb(255, (byte)readCard.color[0], (byte)readCard.color[1], (byte)readCard.color[2]),
                            new Point(readCard.position[0], readCard.position[1]), readCard.rotate, 1, zindex++);
                        myCard.UUID = readCard.cardID;
                        myCard.Owner = readCard.userID;
                        Card_List.AddCard(myCard);
                        loader.MainWindow.Controlers.UserControler.ReceiveCard(readCard.userID, myCard);
                        loader.MainWindow.CardLayer.AddCard(myCard);
                        Canvas.SetZIndex(myCard, myCard.ZIndex);
                    }
                }
            }
        }
        public static void LayoutActivityCard()
        {
            try
            {
                SavingCard[] savingCards = new SavingCard[]{
                    new SavingCard(@"Resource\Card\Activity Card\Slide1.JPG",""+0,"Being Encouraged","Alex",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide2.JPG",""+1,"Being Needed","Alex",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide3.JPG",""+2,"Blow Bubbles","Alex",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide4.JPG",""+3,"Count from 1 to 10","Alex",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide5.JPG",""+4,"Dance","Alex",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide6.JPG",""+5,"Deep Breath","Alex",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide7.JPG",""+6,"Draw","Alex",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide10.JPG",""+7,"Talk to Counselor","Alex",Colors.Yellow),

                    new SavingCard(@"Resource\Card\Activity Card\Slide11.JPG",""+8,"Find Someone to Help","Alex",Colors.Red),
                    new SavingCard(@"Resource\Card\Activity Card\Slide12.JPG",""+9,"Help Someone Else","Alex",Colors.Red),
                    new SavingCard(@"Resource\Card\Activity Card\Slide13.JPG",""+10,"Humor","Alex",Colors.Red),
                    new SavingCard(@"Resource\Card\Activity Card\Slide14.JPG",""+11,"Listen to Music","Alex",Colors.Red),
                    new SavingCard(@"Resource\Card\Activity Card\Slide15.JPG",""+12,"Massage","Alex",Colors.Red),
                    new SavingCard(@"Resource\Card\Activity Card\Slide16.JPG",""+13,"Meditation","Alex",Colors.Red),
                    new SavingCard(@"Resource\Card\Activity Card\Slide28.JPG",""+14,"Talk to Oneself","Alex",Colors.Red),
                    new SavingCard(@"Resource\Card\Activity Card\Slide8.JPG",""+15,"Smell Flowers","Alex",Colors.Red),
                    
                    new SavingCard(@"Resource\Card\Activity Card\Slide1.JPG",""+0,"Being Encouraged","Ben",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide2.JPG",""+1,"Being Needed","Ben",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide3.JPG",""+2,"Blow Bubbles","Ben",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide4.JPG",""+3,"Count from 1 to 10","Ben",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide5.JPG",""+4,"Dance","Ben",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide6.JPG",""+5,"Deep Breath","Ben",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide7.JPG",""+6,"Draw","Ben",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide10.JPG",""+7,"Talk to Counselor","Ben",Colors.Yellow),


                    new SavingCard(@"Resource\Card\Activity Card\Slide32.JPG",""+16,"Squeeze Hands","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Activity Card\Slide21.JPG",""+17,"Read","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Activity Card\Slide23.JPG",""+18,"Sit by Oneself","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Activity Card\Slide24.JPG",""+19,"Sleep","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Activity Card\Slide26.JPG",""+20,"Talk to Pet","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Activity Card\Slide9.JPG",""+21,"Walk","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Activity Card\Slide17.JPG",""+22,"Muscle Relaxation","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Activity Card\Slide18.JPG",""+23,"Trampoline","Ben",Colors.Green),

                   new SavingCard(@"Resource\Card\Activity Card\Slide1.JPG",""+0,"Being Encouraged","Chris",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide2.JPG",""+1,"Being Needed","Chris",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide3.JPG",""+2,"Blow Bubbles","Chris",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide4.JPG",""+3,"Count from 1 to 10","Chris",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide5.JPG",""+4,"Dance","Chris",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide6.JPG",""+5,"Deep Breath","Chris",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide7.JPG",""+6,"Draw","Chris",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide10.JPG",""+7,"Talk to Counselor","Chris",Colors.Yellow),

                    new SavingCard(@"Resource\Card\Activity Card\Slide11.JPG",""+8,"Find Someone to Help","Chris",Colors.Blue),
                    new SavingCard(@"Resource\Card\Activity Card\Slide12.JPG",""+9,"Help Someone Else","Chris",Colors.Blue),
                    new SavingCard(@"Resource\Card\Activity Card\Slide13.JPG",""+10,"Humor","Chris",Colors.Blue),
                    new SavingCard(@"Resource\Card\Activity Card\Slide14.JPG",""+11,"Listen to Music","Chris",Colors.Blue),
                    new SavingCard(@"Resource\Card\Activity Card\Slide15.JPG",""+12,"Massage","Chris",Colors.Blue),
                    new SavingCard(@"Resource\Card\Activity Card\Slide16.JPG",""+13,"Meditation","Chris",Colors.Blue),
                    new SavingCard(@"Resource\Card\Activity Card\Slide28.JPG",""+14,"Talk to Oneself","Chris",Colors.Blue),
                    new SavingCard(@"Resource\Card\Activity Card\Slide8.JPG",""+15,"Smell Flowers","Chris",Colors.Blue),

                    new SavingCard(@"Resource\Card\Activity Card\Slide1.JPG",""+0,"Being Encouraged","Danny",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide2.JPG",""+1,"Being Needed","Danny",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide3.JPG",""+2,"Blow Bubbles","Danny",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide4.JPG",""+3,"Count from 1 to 10","Danny",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide5.JPG",""+4,"Dance","Danny",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide6.JPG",""+5,"Deep Breath","Danny",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide7.JPG",""+6,"Draw","Danny",Colors.Yellow),
                    new SavingCard(@"Resource\Card\Activity Card\Slide10.JPG",""+7,"Talk to Counselor","Danny",Colors.Yellow),


                    new SavingCard(@"Resource\Card\Activity Card\Slide32.JPG",""+16,"Squeeze Hands","Danny",Colors.Magenta),
                    new SavingCard(@"Resource\Card\Activity Card\Slide21.JPG",""+17,"Read","Danny",Colors.Magenta),
                    new SavingCard(@"Resource\Card\Activity Card\Slide23.JPG",""+18,"Sit by Oneself","Danny",Colors.Magenta),
                    new SavingCard(@"Resource\Card\Activity Card\Slide24.JPG",""+19,"Sleep","Danny",Colors.Magenta),
                    new SavingCard(@"Resource\Card\Activity Card\Slide26.JPG",""+20,"Talk to Pet","Danny",Colors.Magenta),
                    new SavingCard(@"Resource\Card\Activity Card\Slide9.JPG",""+21,"Walk","Danny",Colors.Magenta),
                    new SavingCard(@"Resource\Card\Activity Card\Slide17.JPG",""+22,"Muscle Relaxation","Danny",Colors.Magenta),
                    new SavingCard(@"Resource\Card\Activity Card\Slide18.JPG",""+23,"Trampoline","Danny",Colors.Magenta)
                };
                GenerateLayout(@"Resource\Layout\layout_activity.txt", 2, savingCards);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static void LayoutProblemCard()
        {
            try
            {
                SavingCard[] savingCards = new SavingCard[]{
                    new SavingCard(@"Resource\Card\Contextual Card\Slide1.JPG",""+0,"Angry","Alex",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide4.JPG",""+1,"Tired","Alex",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide5.JPG",""+2,"Worried","Alex",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide6.JPG",""+3,"Think Bad Things","Alex",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide2.JPG",""+4,"Anxious","Alex",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide7.JPG",""+5,"Scare of Height","Alex",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide8.JPG",""+6,"Scare of Speak in Public","Alex",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide3.JPG",""+7,"Depressed","Alex",Colors.Green),

                    new SavingCard(@"Resource\Card\Contextual Card\Slide9.JPG",""+8,"Scare of Crowd","Alex",Colors.Gold),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide10.JPG",""+9,"Afraid of Complexity","Alex",Colors.Gold),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide14.JPG",""+10,"Afraid of Punishment","Alex",Colors.Gold),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide11.JPG",""+11,"Afraid of Stranger","Alex",Colors.Gold),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide15.JPG",""+12,"Afraid of Failure","Alex",Colors.Gold),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide12.JPG",""+13,"Afraid of Leaving Parents","Alex",Colors.Gold),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide16.JPG",""+14,"Afraid of Light","Alex",Colors.Gold),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide13.JPG",""+15,"Afraid of Dark","Alex",Colors.Gold),

                    
                    new SavingCard(@"Resource\Card\Contextual Card\Slide1.JPG",""+0,"Angry","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide4.JPG",""+1,"Tired","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide5.JPG",""+2,"Worried","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide6.JPG",""+3,"Think Bad Things","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide2.JPG",""+4,"Anxious","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide7.JPG",""+5,"Scare of Height","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide8.JPG",""+6,"Scare of Speak in Public","Ben",Colors.Green),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide3.JPG",""+7,"Depressed","Ben",Colors.Green),

                    new SavingCard(@"Resource\Card\Contextual Card\Slide17.JPG",""+16,"Afraid of Sound","Ben",Colors.Blue),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide30.JPG",""+17,"Fight or Damage","Ben",Colors.Blue),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide19.JPG",""+18,"Hard to Make Eye Contact","Ben",Colors.Blue),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide20.JPG",""+19,"Hard to Pay Attention","Ben",Colors.Blue),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide21.JPG",""+20,"Hard to Understand","Ben",Colors.Blue),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide22.JPG",""+21,"Repetitive Behavior","Ben",Colors.Blue),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide23.JPG",""+22,"Lack of Patience","Ben",Colors.Blue),
                    new SavingCard(@"Resource\Card\Contextual Card\Slide7.JPG",""+23,"Nervous","Ben",Colors.Blue)
                };
                GenerateLayout(@"Resource\Layout\layout_problem.txt", 2, savingCards);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
