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
    class Paper_Card_Loader
    {
        public class SavingCard
        {
            public SavingCard(String userID,
                String cardID,
                String title,
                Author[] author,
                String abstractText,
                String doi,
                String page,
                String year)
            {
                this.cardID = cardID;
                this.abstractText = abstractText;
                this.userID = userID;
                this.author = author;
                this.doi = doi;
                this.page = page;
                this.year = year;
                this.title = title;
                
            }
            public String cardID;
            public String userID;
            public String title;
            public Author[] author;
            public String abstractText;
            public String doi;
            public String page;
            public String year;
        }
        public class LoadingCard
        {
            public int[] position;
            public int rotate;
            public String cardID;
            public String userID;
            public String title;
            public Author[] author;
            public String abstractText;
        }

        Loaders loader;

        public Loaders Loader
        {
            get { return loader; }
            set { loader = value; }
        }
        static String configFileDir;
        public Paper_Card_Loader(Loaders loader)
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
                        Paper_Card myCard = new Paper_Card(loader.MainWindow.Controlers.CardControler);
                        myCard.UUID = readCard.cardID;
                        myCard.Owner = readCard.userID;
                        My_Paper paper = new My_Paper();
                        paper.Author = readCard.author;
                        paper.Title = readCard.title;
                        paper.AbstractText = readCard.abstractText;
                        myCard.Paper = paper;
                        myCard.InitializeCard(null, new Point(readCard.position[0], readCard.position[1]), readCard.rotate, 1, zindex++);                        
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
                My_Paper[] data = ReadData(sourceFile);
                List<SavingCard> savingCards = new List<SavingCard>();
                foreach (My_Paper d in data)
                {
                    SavingCard sc = new SavingCard("Alex", d.Doi, d.Title, d.Author, d.AbstractText, d.Doi, d.Page, d.Year);
                    savingCards.Add(sc);
                }
                GenerateLayout(@"Resource\Layout\paper_card.txt", savingCards.ToArray());
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
            int index = 0;
            Dictionary<String, int[]> stackPosi = new Dictionary<string, int[]>();
            stackPosi.Add("Alex", new int[] { 
                (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width, 
                STATICS.SCREEN_HEIGHT - (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height});
            stackPosi.Add("Ben", new int[] { 
                STATICS.SCREEN_WIDTH -  (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width , 
                STATICS.SCREEN_HEIGHT - (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height});
            stackPosi.Add("Chris", new int[] { 
                (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width, 
                STATICS.SCREEN_HEIGHT - (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height });
            stackPosi.Add("Danny", new int[] { 
                (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Width, 
                STATICS.SCREEN_HEIGHT - (int)STATICS.DEAULT_CARD_SIZE_WITH_BORDER.Height});
            for (int i = 0; i < loadingCards.Length; i++)
            {
                if (savingCards[i].userID.Equals("Alex") && STATICS.ALEX_ACTIVE ||
                    savingCards[i].userID.Equals("Ben") && STATICS.BEN_ACTIVE ||
                    savingCards[i].userID.Equals("Chris") && STATICS.CHRIS_ACTIVE ||
                    savingCards[i].userID.Equals("Danny") && STATICS.DANNY_ACTIVE)
                {
                    loadingCards[i] = new LoadingCard();
                    if (i < loadingCards.Length / 2)
                        loadingCards[i].position = new int[] { stackPosi["Alex"][0] + rand.Next(60) - 30, stackPosi["Alex"][1] + rand.Next(60) - 30 };
                    else
                        loadingCards[i].position = new int[] { stackPosi["Ben"][0] + rand.Next(60) - 30, stackPosi["Ben"][1] + rand.Next(60) - 30 };
                    loadingCards[i].rotate = rand.Next(8) - 4;
                    loadingCards[i].cardID = savingCards[i].cardID;
                    loadingCards[i].userID = savingCards[i].userID;
                    loadingCards[i].title = savingCards[i].title;
                    loadingCards[i].abstractText = savingCards[i].abstractText;
                    loadingCards[i].author = savingCards[i].author;
                    string result = JsonConvert.SerializeObject(loadingCards[i]);
                    streamWriter.WriteLine(result);
                    index++;
                }
            }
            streamWriter.Flush();
            streamWriter.Close();
        }

        private static My_Paper[] ReadData(String dataFile)
        {
            String nextLine = "";
            List<My_Paper> list = new List<My_Paper>();
            StreamReader reader = new StreamReader(dataFile);
            while ((nextLine = reader.ReadLine()) != null)
            {
                My_Paper paperData = JsonConvert.DeserializeObject<My_Paper>(nextLine);
                list.Add(paperData);

            }
            return list.ToArray();
        }
    }
}
