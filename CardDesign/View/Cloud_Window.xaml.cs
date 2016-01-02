using System;
using System.Collections.Generic;
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
using WordCloud;

namespace CardDesign
{
    /// <summary>
    /// Interaction logic for CloudWindow.xaml
    /// </summary>
    public partial class Cloud_Window : Window
    {
        WordCloudView wordCloud;
        MainWindow mainWindow;

        public WordCloudView WordCloud
        {
            get
            {
                return wordCloud;
            }

            set
            {
                wordCloud = value;
            }
        }

        public Cloud_Window(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            WordCloud = new WordCloudView(this.Width, this.Height);
            Closing += MainWindow_Closing;
            this.Container.Children.Add(WordCloud);
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WordCloud.Quit();
        }
    }
}
