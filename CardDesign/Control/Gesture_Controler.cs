using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace CardDesign
{
    /// <summary>
    /// The controller class to detect the gesture.
    /// Fire a new thread.
    /// Periodically update all the gestures in the Gesture_List
    /// Periodically remove all the gestures which are terminated from the Gesture_List
    /// Periodically check the My_Point list
    /// If one or more points are detected as a gesture, put create a Gesture_Event variable and add it to the Gesture_List. Then remove these points from the list
    /// </summary>
    public class Gesture_Controler
    {
        Thread gestureDetectionThread = null;
        bool isRunning = false;
        List<My_Point> newGesturePoints = new List<My_Point>();
        Controlers control;

        public Controlers Control
        {
            get { return control; }
            set { control = value; }
        }
        public List<My_Point> NewGesturePoints
        {
            get { return newGesturePoints; }
            set { newGesturePoints = value; }
        }

        public Gesture_Controler(Controlers control)
        {
            this.control = control;
        }

        public void Start()
        {
            if (gestureDetectionThread != null)
            {
                isRunning = false;
                Thread.Sleep(100);
                gestureDetectionThread.Abort();
            }
            gestureDetectionThread = new Thread(new ThreadStart(processGesture));
            gestureDetectionThread.SetApartmentState(ApartmentState.STA);
            isRunning = true;
            gestureDetectionThread.Start();
        }

        public void quit() {
            if (gestureDetectionThread != null)
            {
                isRunning = false;
                Thread.Sleep(100);
                gestureDetectionThread.Abort();
                gestureDetectionThread = null;
            }
        }
        private void processGesture()
        {
            while (isRunning)
            {
                if (Touch_Controler.isTouched())
                {
                    Thread.Sleep(STATICS.GESTURE_REFRESH_RATE);
                    updateGesture();
                    terminateGesture();
                    detectGesture();
                }
            }
        }

        private void updateGesture()
        {
            newGesturePoints.Clear();
            My_Point[] myPoints = new My_Point[Point_List.TouchPointList.Count];
            myPoints = Point_List.TouchPointList.Values.ToArray();
            foreach (My_Point point in myPoints)
            {
                if (point.Life > STATICS.MIN_GESTURE_LIFE)
                {
                    bool inUse = false;
                    Gesture_Event[] gestures = Gesture_List.GestureList.ToArray();
                    foreach (Gesture_Event gesture in gestures)
                    {
                        if ((gesture.Status == GESTURESTATUS.CONTINUE || gesture.Status == GESTURESTATUS.REGISTER) && gesture.ContainPoint(point))
                        {
                            inUse = true;
                            break;
                        }
                    }
                    if (!inUse)
                    {
                        newGesturePoints.Add(point);
                    }
                }
            }
            lock (Gesture_List.GestureList)
            {
                foreach (Gesture_Event gesture in Gesture_List.GestureList.ToArray())
                {
                    gesture.Continue(gesture.Senders, gesture.Points);
                }
            }
        }

        private void terminateGesture()
        {
            List<Gesture_Event> waitToRemove = new List<Gesture_Event>();
            lock (Gesture_List.GestureList)
            {
                foreach (Gesture_Event gesture in Gesture_List.GestureList)
                {
                    if (gesture.Status == GESTURESTATUS.TERMINATE || gesture.Status == GESTURESTATUS.FAIL)
                    {
                        waitToRemove.Add(gesture);
                    }
                }

                foreach (Gesture_Event gesture in waitToRemove)
                {
                    foreach (My_Point point in gesture.Points)
                    {
                        control.MainWindow.GestureIndicatorLayer.Remove(point.ID);
                        Gesture_List.removeGesture(gesture);
                    }
                }
            }
            waitToRemove.Clear();
        }



        private void detectGesture()
        {
            if (newGesturePoints.Count > 0)
            {
                // the code that you want to measure comes here
                Gesture_Event_Linking.Detect(newGesturePoints, this);
                //Gesture_Event_Copying.Detect(newGesturePoints, this);
                Gesture_Event_Sorting.Detect(newGesturePoints, this);
                //Gesture_Event_Emphasizing.Detect(newGesturePoints, this);
                Gesture_Event_Grouping.Detect(newGesturePoints,this);
                //Gesture_Event_Sorting_Group.Detect(newGesturePoints, this);
                //Gesture_Event_Showing.Detect(newGesturePoints, this);
                Gesture_Event_Showing_Sorting.Detect(newGesturePoints,this);
                Gesture_Event_Cutting_Sorting.Detect(newGesturePoints, this);
                Gesture_Event_Cutting_Link.Detect(newGesturePoints, this);
                Gesture_Event_Deleting_Bin.Detect(newGesturePoints, this);



            }
        }        
    }
}
