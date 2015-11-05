using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Gesture_Event_Copying : Gesture_Event
    {
        Card card1;

        public Card Card1
        {
            get { return card1; }
            set { card1 = value; }
        }
        public static Gesture_Event_Copying Detect(List<My_Point> points, Gesture_Controler controler) {
            List<My_Point> result = new List<My_Point>();
            foreach (My_Point point in points)
            {
                if (point.Sender is Copy_Icon)
                {
                    result.Add(point);
                    Card card = (point.Sender as Copy_Icon).Card;
                    My_Point[] argPoints = result.ToArray();
                    object[] objects = new object[2];
                    objects[0] = card;
                    Gesture_Event_Copying copyEvent = new Gesture_Event_Copying();
                    copyEvent.Points = argPoints;
                    Gesture_List.addGesture(copyEvent);
                    Gesture_Copying_Listener gestureLinkingListener = new Gesture_Copying_Listener(controler, copyEvent);
                    copyEvent.Register(objects, argPoints);
                    foreach (My_Point p in result)
                    {
                        controler.NewGesturePoints.Remove(p);
                    }
                    return copyEvent;
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
                card1 = (Card)senders[0];
                Gesture_Event_Args gestureEventArgs = new Gesture_Event_Args();
                gestureEventArgs.GesturePoints = myPoints;
                gestureEventArgs.GestureObjects = senders;
                this.Status = GESTURESTATUS.REGISTER;
                OnRegistered(this, gestureEventArgs);
            }
        }
        public override void Continue(object[] senders, My_Point[] myPoints)
        {
            bool isValid = checkValid(senders, myPoints);
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
            if (checkTerminate(senders, myPoints))
            {
                Terminate(senders, myPoints);
            }
        }
        protected override bool checkTerminate(object[] senders, My_Point[] myPoints)
        {
            return !myPoints[0].IsLive && Calculator.CalDistance(myPoints[0].StartPoint, myPoints[0].CurrentPoint) >= STATICS.MIN_DISTANCE_FOR_MOVE;
        }
        protected override bool checkValid(object[] senders, My_Point[] myPoints)
        {
            return myPoints[0].IsLive || !myPoints[0].IsLive && Calculator.CalDistance(myPoints[0].StartPoint, myPoints[0].CurrentPoint) > STATICS.MIN_DISTANCE_FOR_MOVE;
        }
        public override void Terminate(object[] senders, My_Point[] myPoints)
        {
            if (myPoints != null && senders != null)
            {
                Gesture_Event_Args gestureEventArgs = new Gesture_Event_Args();
                gestureEventArgs.GesturePoints = myPoints;
                My_Point point = myPoints[0];
                gestureEventArgs.GestureObjects = this.Senders;
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
