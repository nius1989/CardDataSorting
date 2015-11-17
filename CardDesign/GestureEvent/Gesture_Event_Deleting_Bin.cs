using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Gesture_Event_Deleting_Bin : Gesture_Event
    {

        public static Gesture_Event_Deleting_Bin Detect(List<My_Point> points, Gesture_Controler controler)
        {
            List<My_Point> result = new List<My_Point>();
            Gesture_Event_Deleting_Bin deletingEvent = null;
            foreach (My_Point p in points)
            {
                if (!result.Contains(p) && p.Sender is Menu_Sort_Box)
                {
                    Menu_Sort_Box category = p.Sender as Menu_Sort_Box;
                    foreach (Menu_Container mc in controler.Control.MainWindow.MenuLayer.MenuBars)
                    {
                        if (mc != null && category != null && Math.Sqrt(Math.Pow((category.CurrentPosition.X - mc.RecycleButton.XCoord), 2) +
                            Math.Pow((category.CurrentPosition.Y - mc.RecycleButton.YCoord), 2))
                            < 50)
                        {
                            foreach (My_Point p2 in points)
                            {
                                if (p.Sender == p2.Sender && !result.Contains(p2))
                                    result.Add(p2);
                            }
                            My_Point[] argPoints = result.ToArray();
                            object[] objects = new object[2];
                            objects[0] = category;
                            objects[1] = mc.RecycleButton;
                            deletingEvent = new Gesture_Event_Deleting_Bin();
                            Gesture_List.addGesture(deletingEvent);
                            Gesture_Deleting_Bin_Listener gestureListener = new Gesture_Deleting_Bin_Listener(controler, deletingEvent);
                            deletingEvent.Register(objects, argPoints);
                        }
                    }
                    
                }
            }
            return deletingEvent;
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
            foreach (My_Point p in myPoints)
            {
                if (!p.IsLive)
                {
                    return true;
                }
            }
            return false; 
        }

        protected override bool checkValid(object[] senders, My_Point[] myPoints)
        {
            Menu_Sort_Box categoryBox = senders[0] as Menu_Sort_Box;
            Menu_Recycle_Bin recycleBox = senders[1] as Menu_Recycle_Bin;

            if (recycleBox != null && categoryBox != null && Math.Sqrt(Math.Pow((categoryBox.CurrentPosition.X - recycleBox.XCoord), 2) +
                Math.Pow((categoryBox.CurrentPosition.Y - recycleBox.YCoord), 2))
                < 50)
            {
                return true;
            }
            return false;
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
