using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CardDesign
{
    class Gesture_Event_Emphasizing : Gesture_Event
    {

        double startSize = 0;

        public double StartSize
        {
            get { return startSize; }
            set { startSize = value; }
        }

        public static Gesture_Event_Emphasizing Detect(List<My_Point> points,Gesture_Controler controler)
        {
            List<My_Point> result = new List<My_Point>();
            Gesture_Event_Emphasizing emphasizeEvent = null;
            if (points.Count > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    if (Calculator.CalDistance(points[i].StartPoint, points[i].CurrentPoint) < STATICS.MIN_DISTANCE_FOR_MOVE && points[i].Life > STATICS.MIN_LONG_PRESS_LIFE && points[i].Sender is Card)
                    {
                        bool moreThan1 = false;
                        if (points.Count > 1)
                        {
                            for (int m = 0; m < points.Count; m++)
                            {
                                if (i != m && points[i].Sender == points[m].Sender)
                                {
                                    moreThan1 = true;
                                    break;
                                }
                            }
                        }
                        if (!moreThan1)
                        {
                            result.Add(points[i]);
                            My_Point[] argPoints = result.ToArray();
                            object[] objects = new object[argPoints.Length];
                            objects[0] = argPoints[0].Sender;
                            emphasizeEvent = new Gesture_Event_Emphasizing();
                            emphasizeEvent.Points = argPoints;
                            Gesture_List.addGesture(emphasizeEvent);
                            Gesture_Emphasizing_Listener gestureListener = new Gesture_Emphasizing_Listener(controler, emphasizeEvent);
                            emphasizeEvent.Register(objects, argPoints);
                            foreach (My_Point p in result)
                            {
                                points.Remove(p);
                            }
                            return emphasizeEvent;
                        }
                    }
                }
            }
            return null;
        }
        public double GetSize()
        {
            TouchPoint point0 = Points[0].CurrentPoint;
            double size0 = point0.Size.Height * point0.Size.Width;
            return size0;
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
                startSize = GetSize();
                OnRegistered(this, gestureEventArgs);
            }
        }
        public override void Continue(object[] senders, My_Point[] myPoints)
        {
            bool isValid=checkValid(null,null);
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
            return !Points[0].IsLive;
        }
        protected override bool checkValid(object[] senders, My_Point[] myPoints)
        {
            return Calculator.CalDistance(Points[0].StartPoint, Points[0].CurrentPoint) < STATICS.MIN_DISTANCE_FOR_MOVE;
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
