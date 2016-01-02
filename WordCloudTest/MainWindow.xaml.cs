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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordCloud;

namespace WordCloudTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WordCloudView wordCloud;
        public MainWindow()
        {
            InitializeComponent();

            this.Width = 1600;
            this.Height = 900;
            wordCloud = new WordCloudView(this.Width,this.Height);
            Closing += MainWindow_Closing;
            this.Container.Children.Add(wordCloud);
            this.Left = 0;
            this.Top = 0;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wordCloud.Quit();
        }
    }
}
