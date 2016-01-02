using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WordCloud
{
    class Graph_Generator
    {
        Thread uiUpdateThread;
        double[][] matrix;
        String[] keywords;
        List<Point> pointList;//one to one correspond to keywards
        double energy = 0;
        double C = 1;
        int progress;
        double step = 0.1;
        bool isRunning = true;
        Controler ctrler;
        double maxConnectionValue = 0;
        Vector adjustmentVector = new Vector();
        double maxConnectionLength = 0.6;

        public Graph_Generator(Controler controler) {
            this.ctrler = controler;
        }

        public Point[] GetPoints(double width, double height)
        {
            if (pointList != null)
            {
                Point[] plist = pointList.ToArray();
                for (int i = 0; i < plist.Length; i++)
                {
                    plist[i].X *= width;
                    plist[i].Y *= height;
                }
                return plist;
            }
            else {
                return null;
            }
        }
        //<summary> update the connection matrix, if key words are selected, display them<summary>
        public void UpdateGraph(string[] newKeys, double[][] mtx)
        {
            if (newKeys != null && newKeys.Length > 0)
            {
                matrix = mtx;
                //Find the max connection value;
                foreach (double[] row in mtx)
                {
                    double rowMax = row.Max();
                    if (maxConnectionValue < rowMax)
                    {
                        maxConnectionValue = rowMax;
                    }
                }

                Random rand = new Random();
                //Initialize the random graph
                if (pointList == null)
                {
                    pointList = new List<Point>();
                    keywords = newKeys;
                    for (int i = 0; i < newKeys.Length; i++)
                    {
                        double rx = (rand.NextDouble() - 0.5) / 1000;
                        double ry = (rand.NextDouble() - 0.5) / 1000;
                        pointList.Add(new Point(0.5 + rx, 0.5 + ry));
                    }
                }
                else {
                    List<Point> tempList = new List<Point>();
                    foreach (string newkey in newKeys) {
                        if (keywords.Contains(newkey))
                        {
                            int index = Array.IndexOf(keywords, newkey);
                            tempList.Add(pointList[index]);
                        }
                        else {
                            double rx = (rand.NextDouble() - 0.5) / 1000;
                            double ry = (rand.NextDouble() - 0.5) / 1000;
                            tempList.Add(new Point(0.5 + rx, 0.5 + ry));
                        }
                    }
                    pointList.Clear();
                    pointList = tempList;
                    keywords = newKeys;
                }
                step = 0.05;
                isRunning = true;
            }
            else
            {
                ctrler.UpdateNodes();
                isRunning = false;
            }
        }

        public void StartGenGraph() {
            uiUpdateThread = new Thread(new ThreadStart(UpdateThread));
            uiUpdateThread.Start();
        }

        public void Quit() {
            uiUpdateThread.Abort();
        }
        private void UpdateThread()
        {
            step = 0.05;
            energy = double.MaxValue;
            while (true)
            {
                if (isRunning)
                {
                    UpdateUI();
                    ctrler.UpdateNodes();
                }
                Thread.Sleep(40);
            }
        }

        private void UpdateUI()
        {
            lock (pointList)
            {
                double energy0 = energy;
                energy = 0;
                for (int i = 0; i < pointList.Count; i++)
                {
                    Vector f = new Vector(0, 0);
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        if (i != j && matrix[i][j] > 0)
                        {
                            f += CalAtrration(i, j);
                        }
                    }
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        if (i != j)
                            f += CalRepel(i, j);
                    }
                    pointList[i] += step * (f / f.Length);
                    energy += Math.Pow(f.Length, 2);
                }
                step = Update_StepLength(step, energy, energy0);
                AdjustCenter();
            }
        }

        private double Update_StepLength(double step, double energy, double energy0)
        {
            if (energy < energy0)
            {
                progress = progress + 1;
                if (progress >= 5)
                {
                    progress = 0;
                    step = step / 0.99;
                }
            }
            else {
                progress = 0;
                step = 0.99 * step;
            }
            return step;
        }

        private Vector CalRepel(int i, int j)
        {
            double fr = CalRep(i, j);
            return (fr / Dist(i, j)) * (pointList[j] - pointList[i]);
        }

        private double CalRep(int i, int j)
        {
            double atrc = 1;
            if (matrix[i][j] != 0)
                atrc = (-1 * C * Math.Pow(GetNaturalLength(matrix[i][j], maxConnectionLength), 2)) / Dist(i, j);
            else
                atrc = (-1 * C * Math.Pow(GetNaturalLength(maxConnectionValue, maxConnectionLength), 2)) / Dist(i, j);
            return atrc;
        }

        private Vector CalAtrration(int i, int j)
        {
            double fa = CalAtrc(i, j);
            return (fa / Dist(i, j)) * (pointList[j] - pointList[i]);
        }

        private double CalAtrc(int i, int j)
        {
            if (matrix[i][j] != 0)
            {
                double atrc = 1;
                atrc = Math.Pow(Dist(i, j), 2) / GetNaturalLength(matrix[i][j], maxConnectionLength);
                return atrc;
            }
            else {
                return 0;
            }
        }
        private void AdjustCenter()
        {
            double ctrX = 0, ctrY = 0;
            foreach (Point p in pointList)
            {
                ctrX += p.X;
                ctrY += p.Y;
            }
            ctrX /= pointList.Count;
            ctrY /= pointList.Count;
            adjustmentVector = new Point(0.5, 0.5) - new Point(ctrX, ctrY);
            foreach (Point p in pointList) { 
            
            }
            for (int i = 0; i < pointList.Count; i++)
            {
                pointList[i] = new Point(pointList[i].X + adjustmentVector.X, pointList[i].Y + adjustmentVector.Y);
            }
        }

        private double GetNaturalLength(double rank, double max) {
            double a = max / maxConnectionValue;
            return a * rank;
        }
        private double Dist(int i, int j)
        {
            double dist = Math.Sqrt(Math.Pow(pointList[i].X - pointList[j].X, 2) +
                Math.Pow(pointList[i].Y - pointList[j].Y, 2));
            if (dist == 0)
            {
                dist = double.Epsilon;
            }
            return dist;
        }
    }
}
