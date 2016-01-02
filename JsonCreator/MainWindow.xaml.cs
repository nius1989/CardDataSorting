using System;
using System.IO;
using System.Windows;
using TFIDF_Generator;

namespace JsonCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ArticleList list = new ArticleList();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Article art = new Article();
            art.Title = titleTextBox.Text.Trim();
            art.Author = authorTextBox.Text.Trim();
            art.Time = timeTextBox.Text.Trim();
            art.Tag = tagTextBox.Text.Trim();
            art.Content = articleTextBox.Text.Trim();
            list.Add(art);
            listTextBlock.Text = list.GetListTitle();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            titleTextBox.Text = "";
            authorTextBox.Text = "";
            timeTextBox.Text = "";
            tagTextBox.Text = "";
            articleTextBox.Text = "";

        }

        private void saveButton_Click_1(object sender, RoutedEventArgs e)
        {
            list.Save();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            list.Load();
            listTextBlock.Text = list.GetListTitle();
        }

        private void processButton_Click(object sender, RoutedEventArgs e)
        {
            String loc = "newsDocs.txt";
            TFIDF_Documents docs = new TFIDF_Documents();
            String filedir = Path.Combine(Environment.CurrentDirectory, loc);
            docs.Load(filedir);
            docs.Process();
        }
    }
}
