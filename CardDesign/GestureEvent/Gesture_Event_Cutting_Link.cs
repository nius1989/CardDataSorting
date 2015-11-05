using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CardDesign
{
    class Gesture_Event_Cutting_Link:Gesture_Event
    {
        public static Gesture_Event_Cutting_Link Detect(List<My_Point> points,Gesture_Controler controler)
        {
            List<My_Point> result = new List<My_Point>();
            Gesture_Event_Cutting_Link cuttingEvent = null;
            if (Link_List.CardLinks.Count > 0 && controler.Control.MainWindow.LinkingGestureLayer.CardLinks.Count > 0)
            {
                Gesture_Event_Linking[] links = Link_List.CardLinks.ToArray();
                foreach (My_Point p in points)
                {
                    if (p.Sender is Card_Layer)
                    {
                        foreach (Gesture_Event_Linking link in links)
                        {
                            if (link.Card1 != null && link.Card2 != null)
                            {
                                bool isIntersect = Calculator.DoLinesIntersect(new Point(link.Card1.CurrentPosition.X, link.Card1.CurrentPosition.Y),
                                    new Point(link.Card2.CurrentPosition.X, link.Card2.CurrentPosition.Y),
                                    new Point(p.StartPoint.Position.X, p.StartPoint.Position.Y),
                                    new Point(p.CurrentPoint.Position.X, p.CurrentPoint.Position.Y));
                                if (isIntersect)
                                {
                                    result.Add(p);
                                    My_Point[] argPoints = result.ToArray();
                                    object[] objects = new object[2];
                                    objects[0] = link;
                                    cuttingEvent = new Gesture_Event_Cutting_Link();
                                    cuttingEvent.Points = argPoints;
                                    Gesture_List.addGesture(cuttingEvent);
                                    Gesture_Cutting_Link_Listener gestureListener = new Gesture_Cutting_Link_Listener(controler, cuttingEvent);
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
