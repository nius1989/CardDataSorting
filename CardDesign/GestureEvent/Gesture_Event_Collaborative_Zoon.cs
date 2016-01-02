using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Gesture_Event_Collaborative_Zoon:Gesture_Event
    {
        public static Gesture_Event_Collaborative_Zoon Detect(List<My_Point> points, Gesture_Controler controler)
        {
            List<My_Point> result = new List<My_Point>();
            Gesture_Event_Collaborative_Zoon colZoonEvent = null;
            foreach (My_Point p in points)
            {
                if (p.Sender is News_Card)
                {
                    if (STATICS.COLLABORATIVE_ZOON.Contains((int)p.CurrentPoint.Position.X, (int)p.CurrentPoint.Position.Y)) {
                        result.Add(p);
                        My_Point[] argPoints = result.ToArray();
                        object[] objects = new object[1];
                        objects[0] = p.Sender;
                        colZoonEvent = new Gesture_Event_Collaborative_Zoon();
                        colZoonEvent.Points = argPoints;
                        Gesture_List.addGesture(colZoonEvent);
                        Gesture_Collaborative_Zoon_Listener gestureListener = new Gesture_Collaborative_Zoon_Listener(controler, colZoonEvent);
                        colZoonEvent.Register(objects, argPoints);
                        foreach (My_Point p2 in result)
                        {
                            points.Remove(p2);
                        }
                        return colZoonEvent;
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
            if (myPoints != null && senders != null && checkValid(senders, myPoints))
            {
                Gesture_Event_Args gestureEventArgs = new Gesture_Event_Args();
                gestureEventArgs.GesturePoints = myPoints;
                gestureEventArgs.GestureObjects = senders;
                this.Status = GESTURESTATUS.CONTINUE;
                OnContinued(this, gestureEventArgs);
            }
            if (!checkValid(senders, myPoints))
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
            if (STATICS.COLLABORATIVE_ZOON.Contains((int)myPoints[0].CurrentPoint.Position.X, (int)myPoints[0].CurrentPoint.Position.Y))
            {
                return true;
            }
            return false;
        }
        protected override bool checkTerminate(object[] senders, My_Point[] myPoints)
        {
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
