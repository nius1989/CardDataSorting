using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CardDesign
{
    class News_Card_Loader
    {
        public class SavingCard
        {
            public SavingCard(String userID,
                String cardID,
                String title,
                String newsID,
                String author,
                String content)
            {
                this.cardID = cardID;
                this.content = content;
                this.userID = userID;
                this.author = author;
                this.title = title;
                this.newsID = newsID;
            }
            public String cardID;
            public String userID;
            public String title;
            public String author;
            public String content;
            public String newsID;
        }
        public class LoadingCard
        {
            public int[] position;
            public int rotate;
            public String cardID;
            public String userID;
            public String title;
            public String newsID;
            public String author;
            public String content;
        }

        Loaders loader;

        public Loaders Loader
        {
            get { return loader; }
            set { loader = value; }
        }
        static String configFileDir;
        public News_Card_Loader(Loaders loader)
        {
            this.loader = loader;
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
                        News_Card myCard = new News_Card(loader.MainWindow.Controlers.CardControler, readCard.userID);
                        myCard.UUID = readCard.cardID;
                        myCard.Owner = readCard.userID;
                        My_News news = new My_News();
                        news.Author = readCard.author;
                        news.Title = readCard.title;
                        news.Content = readCard.content;
                        myCard.News = news;
                        myCard.NewsID = readCard.newsID;
                        System.Windows.Media.Color color= System.Windows.Media.Colors.White;
                        switch (readCard.userID) {
                            case "Alex":
                                color = STATICS.USER_COLOR_CODE[0];
                                break;
                            case "Ben":
                                color = STATICS.USER_COLOR_CODE[1];
                                break;
                            case "Chris":
                                color = STATICS.USER_COLOR_CODE[2];
                                break;
                            case "Danny":
                                color = STATICS.USER_COLOR_CODE[3];
                                break;
                        }
                        myCard.InitializeCard(color, new Point(readCard.position[0], readCard.position[1]), readCard.rotate, 1, zindex++);                        
                        Card_List.AddCard(myCard);
                        loader.MainWindow.Controlers.UserControler.ReceiveCard(readCard.userID, myCard);
                        loader.MainWindow.CardLayer.AddCard(myCard);
                        Canvas.SetZIndex(myCard, myCard.ZIndex);
                    }
                }
            }
        }

        public static void LayoutPaperCard(String sourceFile)
        {
            try
            {
                My_News[] data = ReadData(sourceFile);
                List<SavingCard> savingCards = new List<SavingCard>();
                int id = 0;
                foreach (My_News d in data)
                {
                    SavingCard sc = new SavingCard("Alex", "1" + id, d.Title, "" + id, d.Author, d.Content);
                    savingCards.Add(sc);
                    id++;
                }

                id = 0;
                foreach (My_News d in data)
                {
                    SavingCard sc = new SavingCard("Ben", "2" + id, d.Title, "" + id, d.Author, d.Content);
                    savingCards.Add(sc);
                    id++;
                }
                id = 0;
                foreach (My_News d in data)
                {
                    SavingCard sc = new SavingCard("Chris", "3" + id, d.Title, "" + id, d.Author, d.Content);
                    savingCards.Add(sc);
                    id++;
                }
                id = 0;
                foreach (My_News d in data)
                {
                    SavingCard sc = new SavingCard("Danny", "4" + id, d.Title, "" + id, d.Author, d.Content);
                    savingCards.Add(sc);
                    id++;
                }
                GenerateLayout(@"Resource\Layout\news_card.txt", savingCards.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void GenerateLayout(string savingFile, SavingCard[] savingCards)
        {
            String filedir = Path.Combine(Environment.CurrentDirectory, savingFile);
            int cardPerPerson = 0;
            cardPerPerson = savingCards.Where(s => s.userID.Equals("Alex")).Count();
            LoadingCard[] loadingCards = new LoadingCard[savingCards.Length];
            Random rand = new Random();
            StreamWriter streamWriter = new StreamWriter(filedir);
            double totalLength = STATICS.MENU_BAR_SIZE.Width - STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width;
            double inv = totalLength / (cardPerPerson - 1);
            Point[][] stackPosi = new Point[4][];
            stackPosi[0] = new Point[cardPerPerson];
            for (int i = 0; i < cardPerPerson; i++) {
                stackPosi[0][i] = new Point((int)(inv * i), 0 );
            }
            stackPosi[1] = new Point[cardPerPerson];
            for (int i = 0; i < cardPerPerson; i++)
            {
                stackPosi[1][i] = new Point((int)(inv * -i), 0);
            }
            stackPosi[2] = new Point[cardPerPerson];
            for (int i = 0; i < cardPerPerson; i++)
            {
                stackPosi[2][i] = new Point(0, (int)(inv * i));
            }
            stackPosi[3] = new Point[cardPerPerson];
            for (int i = 0; i < cardPerPerson; i++)
            {
                stackPosi[3][i] = new Point(0, (int)(inv * -i));
            }
            Vector[] moveVector = new Vector[4];
            moveVector[0] = new Vector((STATICS.SCREEN_WIDTH-STATICS.MENU_BAR_SIZE.Width)/2+STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width/2,
                STATICS.SCREEN_HEIGHT - STATICS.MENU_BAR_SIZE.Height - STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height / 2);
            moveVector[1] = new Vector((STATICS.SCREEN_WIDTH + STATICS.MENU_BAR_SIZE.Width) / 2-STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width/2,
                STATICS.MENU_BAR_SIZE.Height + STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height/2) ;
            moveVector[2] = new Vector(STATICS.MENU_BAR_SIZE.Height+STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height/2,
                (STATICS.SCREEN_HEIGHT - STATICS.MENU_BAR_SIZE.Width) / 2+STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width/2);
            moveVector[3] = new Vector(STATICS.SCREEN_WIDTH-STATICS.MENU_BAR_SIZE.Height-STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height/2,
                 (STATICS.SCREEN_HEIGHT + STATICS.MENU_BAR_SIZE.Width) / 2-STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width/2);
            double[] rotation = new double[] { 0, 180, 90, 270 };
            for (int i = 0; i < loadingCards.Length; i++)
            {
                int userIndex = i / cardPerPerson;
                int cardIndex = i % cardPerPerson;
                loadingCards[i] = new LoadingCard();
                loadingCards[i].position = new int[] { (int)(stackPosi[userIndex][cardIndex].X+moveVector[userIndex].X),
                    (int)(stackPosi[userIndex][cardIndex].Y+moveVector[userIndex].Y)};
                loadingCards[i].rotate = (int)(rotation[userIndex]+ rand.Next(8) - 4);
                loadingCards[i].cardID = savingCards[i].cardID;
                loadingCards[i].userID = savingCards[i].userID;
                loadingCards[i].title = savingCards[i].title;
                loadingCards[i].newsID = savingCards[i].newsID;
                loadingCards[i].content = savingCards[i].content;
                loadingCards[i].author = savingCards[i].author;
                string result = JsonConvert.SerializeObject(loadingCards[i]);
                streamWriter.WriteLine(result);
            }
            streamWriter.Flush();
            streamWriter.Close();
        }

        private static My_News[] ReadData(String dataFile)
        {
            String nextLine = "";
            List<My_News> list = new List<My_News>();
            StreamReader reader = new StreamReader(dataFile);
            int count = 0;
            while ((nextLine = reader.ReadLine()) != null)
            {
                My_News newsData = JsonConvert.DeserializeObject<My_News>(nextLine);
                list.Add(newsData);
                count++;
                if (count > 100) {
                    break;
                }
            }
            return list.ToArray();
        }
    }
}
