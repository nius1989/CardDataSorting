using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CardDesign
{
    /// <summary>
    /// Interaction logic for Control_Window.xaml
    /// </summary>
    public partial class Control_Window : Window
    {
        delegate void Update_Control_Window(String text,int window);
        MainWindow mainWindow;
        Dictionary<String, FileInfo> layoutFiles = new Dictionary<String, FileInfo>();  // List that will hold the files and subfiles in path
        FileInfo selectedLayoutFile;
        String recordDir = @"Output";
        StreamWriter streamWriter;
        String cardCate = "activity";
        public Control_Window(MainWindow mainwin)
        {
            InitializeComponent();
            this.mainWindow = mainwin;
            InitializeControlWindow();
        }

        private void InitializeControlWindow()
        {

            //Card_Loader.LayoutActivityCard();
            //Card_Loader.LayoutProblemCard();
            Document_Card_Loader.LayoutDocumentCard(@"Resource\Data\data_100.txt");
            DirectoryInfo dir = new DirectoryInfo(@"Resource\Layout\");
            try
            {
                foreach (FileInfo f in dir.GetFiles("*"))
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = f.Name;
                    ComBox.Items.Add(item);
                    layoutFiles.Add(f.Name, f);
                }
            }
            catch
            {
                Console.WriteLine("Directory {0}  \n could not be accessed!!!!", dir.FullName);
                return;  // We alredy got an error trying to access dir so dont try to access it again
            }            
        }
        public void UpdateTextInfo(String text, int window)
        {
            if (STATICS.DEBUG_MODE)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    switch (window)
                    {
                        case 1:
                            infoTextBox1.Text = text;
                            break;
                        case 2:
                            infoTextBox2.Text = text + "\n" + infoTextBox2.Text;
                            break;
                        default:
                            return;
                    }

                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }
        public void SaveRecord(Record_Helper.RecordObj recordObj) {
            string result = JsonConvert.SerializeObject(recordObj);
            streamWriter.WriteLine(result);
        }

        private void ComBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selected = (ComboBoxItem)ComBox.SelectedItem;
            selectedLayoutFile=layoutFiles[selected.Content.ToString()];
            if (selected.Content.ToString().IndexOf("activity") >= 0)
            {
                cardCate = "activity";
            }
            else if (selected.Content.ToString().IndexOf("problem") >= 0)
            {
                cardCate = "problem";
            }
            else if (selected.Content.ToString().IndexOf("document") >= 0)
            {
                cardCate = "document";
            }
        }
        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            recordDir += @"\" + groupName.Text.Trim()+@"\";
            if (!Directory.Exists(recordDir)) {
                Directory.CreateDirectory(recordDir);
            }
            String relativeFile =recordDir+"\\"+ groupName.Text.Trim() + "-" + cardCate + ".txt";
            String recordFullFile = System.IO.Path.Combine(Environment.CurrentDirectory, relativeFile);
            streamWriter = new StreamWriter(recordFullFile);
            mainWindow.Start(selectedLayoutFile.FullName);
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            String relativeFile = recordDir + "\\" + groupName.Text.Trim()+ "-" + cardCate + ".csv";
            String recordFullFile = System.IO.Path.Combine(Environment.CurrentDirectory, relativeFile);
            StreamWriter streamWriterSave = new StreamWriter(recordFullFile);
            foreach (Card c in Card_List.CardList) {
                streamWriterSave.WriteLine(c.Owner + "," + c.UUID + "," + "," + c.CurrentPosition.ToString() + "," + String.Join(",", c.SortingGroups.Select(s=>s.GroupID).ToArray()));
            }
            streamWriterSave.Flush();
            streamWriterSave.Close();
        }

        private void Button_Click_End(object sender, RoutedEventArgs e)
        {
            recordDir = @"Output";
            if (streamWriter != null)
            {
                streamWriter.Flush();
                streamWriter.Close();
                streamWriter = null;
            }
            infoTextBox1.Text = "";
            infoTextBox2.Text = "";
            mainWindow.Controlers.Deinitialize();
            mainWindow.Loaders.Deinitialize();
            mainWindow.DeinitViews();           
        }
    }
}
