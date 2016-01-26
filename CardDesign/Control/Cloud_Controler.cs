using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Cloud_Controler
    {
        Controlers control;
        double[][] matrix;//User Factor Matrix. row: 4 users, column: documents
        int newsCount = 0;
        WordCloud.Controler cloudControler;
        public Cloud_Controler(Controlers control) {
            this.control = control;
            this.matrix = new double[4][];
            newsCount = STATICS.NEWS_NUMBER;
            this.cloudControler = control.MainWindow.CloudWindow.WordCloud.Controler;
            for (int i = 0; i < matrix.Length; i++) {
                matrix[i] = new double[newsCount];
            }
        }
       
        public double[][] Matrix
        {
            get
            {
                return matrix;
            }

            set
            {
                matrix = value;
            }
        }
        public void SetNewsCount(int count) {
            this.newsCount = count;
        }
        public void UpdateMatrix()
        {

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                    matrix[i][j] = 0;
            }
            foreach (News_Card card in Shared_Card_List.ShardCards)
            {
                int index = Array.IndexOf(STATICS.USER_IDS, card.Owner);
                matrix[index][int.Parse(card.NewsID)] = (card.CurrentScale-STATICS.MIN_CARD_SCALE)/STATICS.MAX_CARD_SCALE;
            }
            cloudControler.UpdateMatrix(matrix);
        }
    }
}
