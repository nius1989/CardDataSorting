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
    class Document_Card_Loader
    {
        Stemmer stemmer = new Stemmer();
        Stop_Words_Remover remover = new Stop_Words_Remover();
        public class SavingCard
        {
            public SavingCard(String userID, String cardID, String text)
            {
                this.cardID = cardID;
                this.text = text;
                this.userID = userID;
            }
            public String cardID;
            public String text;
            public String userID;
        }
        public class LoadingCard
        {
            public int[] position;
            public int rotate;
            public String cardID;
            public String userID;
            public String text;
        }
        Loaders loader;

        public Loaders Loader
        {
            get { return loader; }
            set { loader = value; }
        }
        public Document_Card_Loader(Loaders loader)
        {
            this.loader = loader;
        }

        static String configFileDir;
        public static void GenerateLayout(String savingFile, SavingCard[] savingCards)
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
                        loadingCards[i].position = new int[]{stackPosi["Alex"][0]+rand.Next(60)-30,stackPosi["Alex"][1]+rand.Next(60)-30};
                    else
                        loadingCards[i].position = new int[] { stackPosi["Ben"][0] + rand.Next(60) - 30, stackPosi["Ben"][1] + rand.Next(60) - 30 };
                    loadingCards[i].rotate = rand.Next(8) - 4;
                    loadingCards[i].cardID = savingCards[i].cardID;
                    loadingCards[i].userID = savingCards[i].userID;
                    loadingCards[i].text = savingCards[i].text;
                    string result = JsonConvert.SerializeObject(loadingCards[i]);
                    streamWriter.WriteLine(result);
                    index++;
                }
            }
            streamWriter.Flush();
            streamWriter.Close();
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
                        Document_Card myCard = new Document_Card(loader.MainWindow.Controlers.CardControler);
                        myCard.UUID = readCard.cardID;
                        myCard.Owner = readCard.userID;
                        String review = readCard.text;
                        String[] words = review.Split(' ');
                        List<My_Word> wordlist = new List<My_Word>();
                        foreach (String w in words)
                        {
                            My_Word oneWord = new My_Word();
                            oneWord.OringinalContent = w;
                            String procW = w.ToLower();
                            procW = Punctuation_Remover.RemovePunctuation(procW);
                            procW = remover.RemoveStopwords(procW);
                            if (!procW.Equals("STOPWORD") && procW.Length > 0)
                            {
                                procW = stemmer.Stem(procW);
                            }
                            oneWord.ProcessedContent = procW;
                            wordlist.Add(oneWord);
                        }
                        My_Doc doc = new My_Doc();
                        My_Word[] wordArray = wordlist.ToArray();                       
                        doc.SetContent(wordArray);
                        myCard.Doc = doc;
                        myCard.InitializeCard(null, new Point(readCard.position[0], readCard.position[1]), readCard.rotate, 1, zindex++);
                        
                        Card_List.AddCard(myCard);
                        loader.MainWindow.Controlers.UserControler.ReceiveCard(readCard.userID, myCard);
                        loader.MainWindow.CardLayer.AddCard(myCard);
                        Canvas.SetZIndex(myCard, myCard.ZIndex);
                    }
                }
            }
        }

        public static void LayoutDocumentCard(String sourceFile)
        {
            try
            {
                Data_Loading_Template[] data = ReadData(sourceFile);
                List<SavingCard> savingCards = new List<SavingCard>();
                foreach (Data_Loading_Template d in data)
                {
                    SavingCard sc = new SavingCard("Alex", d.ReviewerID + "_" + d.UnixReviewTime, d.ReviewText);
                    savingCards.Add(sc);
                }
                GenerateLayout(@"Resource\Layout\document_card.txt", savingCards.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static Data_Loading_Template[] ReadData(String dataFile)
        {
            String nextLine = "";
            List<Data_Loading_Template> list = new List<Data_Loading_Template>();
            StreamReader reader = new StreamReader(dataFile);
            while ((nextLine = reader.ReadLine()) != null)
            {
                try
                {
                    Data_Loading_Template reviewData = JsonConvert.DeserializeObject<Data_Loading_Template>(nextLine);
                    list.Add(reviewData);
                }
                catch (Exception ex) { }
            }
            return list.ToArray();
        }
    }
}
