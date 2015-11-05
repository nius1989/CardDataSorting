using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    public class Gesture_Event_Showing:Gesture_Event
    {
        public static Gesture_Event_Showing Detect(List<My_Point> points,Gesture_Controler controler)
        {
            List<My_Point> result = new List<My_Point>();
            if (points.Count > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].Life > STATICS.MIN_SHOW_LIFE && points[i].Sender is Card)
                    {
                        result.Add(points[i]);
                        for (int m = 0; m < points.Count; m++)
                        {
                            if (i != m && points[i].Sender == points[m].Sender && points[m].Life > STATICS.MIN_SHOW_LIFE)
                            {
                                result.Add(points[m]);
                            }
                        }
                        if (result.Count > 2)
                        {
                            My_Point[] argPoints = result.ToArray();
                            object[] objects = new object[1];
                            objects[0] = argPoints[0].Sender;
                            Gesture_Event_Showing showEvent = new Gesture_Event_Showing();
                            showEvent.Points = argPoints;
                            Gesture_List.addGesture(showEvent);
                            Gesture_Showing_Listener gestureListener = new Gesture_Showing_Listener(controler, showEvent);
                            showEvent.Register(objects, argPoints);
                            return showEvent;
                        }
                        else
                        {
                            result.Clear();
                        }
                    }
                }
            }
            return null;
        }
        public override void Register(object[] senders, My_Point[] myPoints)
        {
            if (myPoints != null)
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
            bool isValid = checkValid(null, null);
            if (myPoints != null && senders != null && isValid)
            {
                Gesture_Event_Args gestureEventArgs = new Gesture_Event_Args();
                gestureEventArgs.GesturePoints = myPoints;
                gestureEventArgs.GestureObjects = senders;
                this.Status = GESTURESTATUS.CONTINUE;
                OnContinued(this, gestureEventArgs);
            }
            if (!isValid)
            {
                Fail();
                return;
            }
            if (checkTerminate(null, null))
            {
                Terminate(senders, myPoints);
            }
        }
        protected override bool checkTerminate(object[] senders, My_Point[] myPoints)
        {
            return !Points[0].IsLive || !Points[1].IsLive || !Points[2].IsLive;
        }
        protected override bool checkValid(object[] senders, My_Point[] myPoints)
        {
            return Calculator.CalDistance(Points[0].StartPoint, Points[0].CurrentPoint) < STATICS.MIN_DISTANCE_FOR_MOVE
                && Calculator.CalDistance(Points[1].StartPoint, Points[1].CurrentPoint) < STATICS.MIN_DISTANCE_FOR_MOVE
                && Calculator.CalDistance(Points[2].StartPoint, Points[2].CurrentPoint) < STATICS.MIN_DISTANCE_FOR_MOVE;
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
