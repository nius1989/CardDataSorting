using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CardDesign
{
    class Gesture_Event_Cutting_Sorting:Gesture_Event
    {
        public static Gesture_Event_Cutting_Sorting Detect(List<My_Point> points,Gesture_Controler controler)
        {
            List<My_Point> result = new List<My_Point>();
            Gesture_Event_Cutting_Sorting cuttingEvent = null;
            if (Group_List.CardGroups.Count > 0 && controler.Control.MainWindow.SortingGestureLayer.GroupLinks.Count > 0)
            {
                Menu_Sort_Box[] keys = controler.Control.MainWindow.SortingGestureLayer.GroupLinks.Keys.ToArray();
                foreach (My_Point p in points)
                {
                    if (p.Sender is Card_Layer)
                    {
                        foreach (Menu_Sort_Box key in keys)
                        {
                            if (Group_List.CardGroups.ContainsKey(key))
                            {
                                Card[] cards = Group_List.CardGroups[key].ToArray();
                                foreach (Card c in cards)
                                {
                                    bool isIntersect = Calculator.DoLinesIntersect(new Point(key.CurrentPosition.X, key.CurrentPosition.Y),
                                        new Point(c.CurrentPosition.X, c.CurrentPosition.Y),
                                        new Point(p.StartPoint.Position.X, p.StartPoint.Position.Y),
                                        new Point(p.CurrentPoint.Position.X, p.CurrentPoint.Position.Y));
                                    if (isIntersect)
                                    {
                                        result.Add(p);
                                        My_Point[] argPoints = result.ToArray();
                                        object[] objects = new object[2];
                                        objects[0] = key;
                                        objects[1] = c;
                                        cuttingEvent = new Gesture_Event_Cutting_Sorting();
                                        cuttingEvent.Points = argPoints;
                                        Gesture_List.addGesture(cuttingEvent);
                                        Gesture_Cutting_Sorting_Listener gestureListener = new Gesture_Cutting_Sorting_Listener(controler, cuttingEvent);
                                        cuttingEvent.Register(objects, argPoints);
                                        foreach (My_Point p2 in result)
                                        {
                                            points.Remove(p2);
                                        }
                                        return cuttingEvent;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
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
                OnContinued(this, gestureEventArgs);
            }
            if (!checkValid(null, null))
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
            //DONG: TO DO check if the the gesture is still valid
            return true;
        }
        protected override bool checkTerminate(object[] senders, My_Point[] myPoints)
        {
            //DONG: TO DO check if the the gesture is terminated
            return !Points[0].IsLive;
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
