using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CardDesign
{
    public class Gesture_Event_Grouping:Gesture_Event
    {
        protected List<Card> cards = new List<Card>();
        Point[] previousPoint = new Point[4];
        Vector vector = new Vector();
        Vector vectorDistance = new Vector();

        public Vector Vector
        {
            get { return vector; }
        }

        public static Gesture_Event_Grouping Detect(List<My_Point> points,Gesture_Controler controler)
        {
            List<My_Point> result = new List<My_Point>();
            if (points.Count == 4)
            {
                foreach (My_Point p in points)
                {
                    if (p.Sender is Card_Layer)
                        result.Add(p);
                }
            }
            if (result.Count == 4)
            {
                My_Point[] argPoints = result.ToArray();
                Card[] cards = Card_List.CardList.ToArray();
                My_Point[] Newpoint = new My_Point[4];
                for (int i = 0; i < 4; i++)
                {
                    Newpoint[i] = argPoints[i];
                }
                double maxX_1 = Math.Max(Math.Min(Newpoint[0].CurrentPoint.Position.X, Newpoint[1].CurrentPoint.Position.X), Math.Min(Newpoint[2].CurrentPoint.Position.X, Newpoint[3].CurrentPoint.Position.X));
                double maxX_2 = Math.Min(Math.Max(Newpoint[0].CurrentPoint.Position.X, Newpoint[1].CurrentPoint.Position.X), Math.Max(Newpoint[2].CurrentPoint.Position.X, Newpoint[3].CurrentPoint.Position.X));
                double maxY_1 = Math.Max(Math.Min(Newpoint[0].CurrentPoint.Position.Y, Newpoint[1].CurrentPoint.Position.Y), Math.Min(Newpoint[2].CurrentPoint.Position.Y, Newpoint[3].CurrentPoint.Position.Y));
                double maxY_2 = Math.Min(Math.Max(Newpoint[0].CurrentPoint.Position.Y, Newpoint[1].CurrentPoint.Position.Y), Math.Max(Newpoint[2].CurrentPoint.Position.Y, Newpoint[3].CurrentPoint.Position.Y));
                double midX = (maxX_1 + maxX_2) / 2;
                double midY = (maxY_1 + maxY_2) / 2;

                My_Point[] orderedPoints = new My_Point[4];
                int count1 = 0;
                int count2 = 0;
                int count3 = 0;
                int count4 = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (Newpoint[i].CurrentPoint.Position.X > midX && Newpoint[i].CurrentPoint.Position.Y <= midY) { count1++; }
                    if (Newpoint[i].CurrentPoint.Position.X <= midX && Newpoint[i].CurrentPoint.Position.Y < midY) { count2++; }
                    if (Newpoint[i].CurrentPoint.Position.X < midX && Newpoint[i].CurrentPoint.Position.Y >= midY) { count3++; }
                    if (Newpoint[i].CurrentPoint.Position.X >= midX && Newpoint[i].CurrentPoint.Position.Y > midY) { count4++; }

                }
                if (count1 > 1 || count2 > 1 || count3 > 1 || count4 > 1)
                {
                    if (count1 > 1)
                    {
                        int countIndexRight1 = 0;
                        int countIndexRight2 = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (Newpoint[i].CurrentPoint.Position.X > midX && Newpoint[i].CurrentPoint.Position.Y <= midY)
                            {
                                countIndexRight1++;
                                if (countIndexRight1 > 1)
                                {
                                    if (Newpoint[i].CurrentPoint.Position.X < orderedPoints[0].CurrentPoint.Position.X)
                                    {
                                        orderedPoints[1] = argPoints[i];
                                    }
                                    else
                                    {
                                        orderedPoints[1] = orderedPoints[0];
                                        orderedPoints[0] = argPoints[i];
                                    }
                                    break;
                                }
                                orderedPoints[0] = argPoints[i];
                            }
                        }
                        double disRight = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (Newpoint[i].CurrentPoint.Position.X < midX && Newpoint[i].CurrentPoint.Position.Y >= midY)
                            {

                                countIndexRight2++;
                                if (countIndexRight2 > 1)
                                {
                                    double temp = Math.Pow((Newpoint[i].CurrentPoint.Position.X - orderedPoints[1].CurrentPoint.Position.X), 2) + Math.Pow((Newpoint[i].CurrentPoint.Position.Y - orderedPoints[1].CurrentPoint.Position.Y), 2);
                                    if (temp > disRight)
                                    {
                                        orderedPoints[3] = argPoints[i];
                                    }
                                    else
                                    {
                                        orderedPoints[3] = orderedPoints[2];
                                        orderedPoints[2] = argPoints[i];
                                    }
                                    break;
                                }
                                orderedPoints[2] = argPoints[i];
                                disRight = Math.Pow((Newpoint[i].CurrentPoint.Position.X - orderedPoints[1].CurrentPoint.Position.X), 2) + Math.Pow((Newpoint[i].CurrentPoint.Position.Y - orderedPoints[1].CurrentPoint.Position.Y), 2);
                            }
                        }
                    }
                    else if (count2 > 1)
                    {
                        int countIndexLeft1 = 0;
                        int countIndexLeft2 = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (Newpoint[i].CurrentPoint.Position.X <= midX && Newpoint[i].CurrentPoint.Position.Y < midY)
                            {

                                countIndexLeft1++;
                                if (countIndexLeft1 > 1)
                                {
                                    if (Newpoint[i].CurrentPoint.Position.X < orderedPoints[0].CurrentPoint.Position.X)
                                    {
                                        orderedPoints[1] = argPoints[i];
                                    }
                                    else
                                    {
                                        orderedPoints[1] = orderedPoints[0];
                                        orderedPoints[0] = argPoints[i];
                                    }
                                    break;
                                }
                                orderedPoints[0] = argPoints[i];
                            }
                        }
                        double disLeft = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (Newpoint[i].CurrentPoint.Position.X >= midX && Newpoint[i].CurrentPoint.Position.Y > midY)
                            {

                                countIndexLeft2++;
                                if (countIndexLeft2 > 1)
                                {
                                    double temp = Math.Pow((Newpoint[i].CurrentPoint.Position.X - orderedPoints[1].CurrentPoint.Position.X), 2) + Math.Pow((Newpoint[i].CurrentPoint.Position.Y - orderedPoints[1].CurrentPoint.Position.Y), 2);
                                    if (temp > disLeft)
                                    {
                                        orderedPoints[3] = argPoints[i];
                                    }
                                    else
                                    {
                                        orderedPoints[3] = orderedPoints[2];
                                        orderedPoints[2] = argPoints[i];
                                    }
                                    break;
                                }
                                orderedPoints[2] = argPoints[i];
                                disLeft = Math.Pow((Newpoint[i].CurrentPoint.Position.X - orderedPoints[1].CurrentPoint.Position.X), 2) + Math.Pow((Newpoint[i].CurrentPoint.Position.Y - orderedPoints[1].CurrentPoint.Position.Y), 2);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (Newpoint[i].CurrentPoint.Position.X > midX && Newpoint[i].CurrentPoint.Position.Y < midY)
                        {
                            orderedPoints[0] = argPoints[i];
                        }
                        else if (Newpoint[i].CurrentPoint.Position.X < midX && Newpoint[i].CurrentPoint.Position.Y < midY)
                        {
                            orderedPoints[1] = argPoints[i];
                        }
                        else if (Newpoint[i].CurrentPoint.Position.X < midX && Newpoint[i].CurrentPoint.Position.Y > midY)
                        {
                            orderedPoints[2] = argPoints[i];
                        }
                        else if (Newpoint[i].CurrentPoint.Position.X > midX && Newpoint[i].CurrentPoint.Position.Y > midY)
                        {
                            orderedPoints[3] = argPoints[i];
                        }
                    }
                }
                List<Card> objects = new List<Card>();
                lock (cards)
                {
                    foreach (Card c in cards)
                    {
                        if (Enclose_Helper.PNPoly(orderedPoints, c.CurrentPosition.X, c.CurrentPosition.Y))
                        {
                            objects.Add(c);
                        }
                    }
                }

                if (objects.Count > 0)
                {
                    Gesture_Event_Grouping gestureEvent = new Gesture_Event_Grouping();
                    gestureEvent.Points = orderedPoints;
                    gestureEvent.Senders = objects.ToArray();
                    Gesture_List.addGesture(gestureEvent);
                    Gesture_Grouping_Listener gestureListener = new Gesture_Grouping_Listener(controler, gestureEvent);
                    gestureEvent.Register(objects.ToArray(), orderedPoints);
                    foreach (My_Point p in result)
                    {
                        points.Remove(p);
                    }
                    return gestureEvent;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public bool IsGrouping() {
            return vectorDistance.Length < STATICS.MIN_DISTANCE_FOR_MOVE;
        }
        public override void Register(object[] senders, My_Point[] myPoints)
        {
            if (myPoints != null && senders != null)
            {
                Points = myPoints;
                Senders = senders;
                Gesture_Event_Args gestureEventArgs = new Gesture_Event_Args();
                gestureEventArgs.GesturePoints = myPoints;
                gestureEventArgs.GestureObjects = senders;
                this.Status = GESTURESTATUS.REGISTER;
                for (int i = 0; i < 4; i++)
                {
                    previousPoint[i] = new Point(myPoints[i].CurrentPoint.Position.X, myPoints[i].CurrentPoint.Position.Y);
                }
                OnRegistered(this, gestureEventArgs);
            }
        }
        public override void Continue(object[] senders, My_Point[] myPoints)
        {
            if (myPoints != null && senders != null && checkValid(null, null))
            {
                Gesture_Event_Args gestureEventArgs = new Gesture_Event_Args();
                gestureEventArgs.GesturePoints = myPoints;
                gestureEventArgs.GestureObjects = senders;
                this.Status = GESTURESTATUS.CONTINUE;
                double xAvg = 0;
                double yAvg = 0;
                for (int i = 0; i < 4; i++) {
                    xAvg += myPoints[i].CurrentPoint.Position.X - previousPoint[i].X;
                    yAvg += myPoints[i].CurrentPoint.Position.Y - previousPoint[i].Y;
                    previousPoint[i] = new Point(myPoints[i].CurrentPoint.Position.X, myPoints[i].CurrentPoint.Position.Y);
                }
                xAvg = xAvg / 4;
                yAvg = yAvg / 4;
                vector = new Vector(xAvg, yAvg);
                vectorDistance.X += xAvg;
                vectorDistance.Y += yAvg;
                OnContinued(this, gestureEventArgs);
            }
            if (!checkValid(null,null))
            {
                Fail();
                return;
            }
            if (checkTerminate(null, null))
            {
                Terminate(senders, myPoints);
            }
        }
        protected override bool checkValid(object[] senders, My_Point[] myPoints)
        {
            return true;
        }
        protected override bool checkTerminate(object[] senders, My_Point[] myPoints)
        {
            return !Points[0].IsLive || !Points[1].IsLive || !Points[2].IsLive || !Points[3].IsLive;
        }
        public override void Terminate(object[] senders, My_Point[] myPoints)
        {
            if (myPoints != null && senders != null)
            {
                Gesture_Event_Args gestureEventArgs = new Gesture_Event_Args();
                gestureEventArgs.GesturePoints = myPoints;
                gestureEventArgs.GestureObjects = senders;
                this.Status = GESTURESTATUS.TERMINATE;
                OnTerminated(this, gestureEventArgs);
            }
        }

        public override void Fail()
        {
            Gesture_Event_Args gestureEventArgs = new Gesture_Event_Args();
            gestureEventArgs.GesturePoints = Points;
            gestureEventArgs.GestureObjects = Senders;
            this.Status = GESTURESTATUS.FAIL;
            OnFailed(this, gestureEventArgs); 
        }
    }
}
