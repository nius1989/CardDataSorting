using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Gesture_Event_Sorting : Gesture_Event
    {
        public static Gesture_Event_Sorting Detect(List<My_Point> points, Gesture_Controler controler)
        {
            List<My_Point> result = new List<My_Point>();
            Gesture_Event_Sorting sortEvent = null;
            foreach (My_Point p in points)
            {
                if (!result.Contains(p) && p.Sender is Card && Calculator.CalDistance(p.StartPoint, p.CurrentPoint) >= STATICS.MIN_DISTANCE_FOR_MOVE)
                {
                    Card c = p.Sender as Card;
                    foreach (Menu_Sort_Box button in Group_List.GroupBox.Values)
                    {
                        if (c.Contain(button.CurrentPosition))
                        {
                            foreach (My_Point p2 in points)
                            {
                                if (p.Sender == p2.Sender && !result.Contains(p2))
                                    result.Add(p2);
                            }
                            My_Point[] argPoints = result.ToArray();
                            object[] objects = new object[2];
                            objects[0] = c;
                            objects[1] = button;
                            sortEvent = new Gesture_Event_Sorting();
                            sortEvent.Points = argPoints;
                            Gesture_List.addGesture(sortEvent);
                            Gesture_Sorting_Listener gestureListener = new Gesture_Sorting_Listener(controler, sortEvent);
                            sortEvent.Register(objects, argPoints);
                        }
                    }
                }
            }
            foreach (My_Point p in result)
            {
                points.Remove(p);
            }
            return sortEvent;
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
            foreach (My_Point p in myPoints) {
                if (!p.IsLive) {
                    return true;
                }
            }
            return false;
        }
        protected override bool checkValid(object[] senders, My_Point[] myPoints)
        {
            Card c = senders[0] as Card;
            Menu_Sort_Box b = senders[1] as Menu_Sort_Box;
            bool crit1 = c.Contain(b.CurrentPosition);
            bool isLeave = false;
            foreach (My_Point p in myPoints)
            {
                if (!p.IsLive)
                {
                    isLeave = true;
                    break;
                }
            }
            bool crit2 = isLeave && Group_List.ContainCard(b.GroupID, c);
            return crit1 && !crit2;
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
