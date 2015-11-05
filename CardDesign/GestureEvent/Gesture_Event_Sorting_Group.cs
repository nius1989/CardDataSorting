using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Gesture_Event_Sorting_Group:Gesture_Event
    {

        public static Gesture_Event_Sorting_Group Detect(List<My_Point> points,Gesture_Controler controler)
        {
            List<My_Point> result = new List<My_Point>();
            Gesture_Event_Grouping resturedCluster = null;
            Gesture_Event_Sorting_Group sortingGroupEvent = null;
            foreach (My_Point p in points)
            {
                if (p.Sender is Menu_Sort_Box)
                {
                    foreach (Gesture_Event gesture in Gesture_List.GestureList)
                    {
                        if (gesture is Gesture_Event_Grouping)
                        {
                            Gesture_Event_Grouping cluster = gesture as Gesture_Event_Grouping;
                            if (cluster.IsGrouping())
                            {
                                My_Point[] clusterPoints = cluster.Points;
                                if (Enclose_Helper.PNPoly(clusterPoints, p.CurrentPoint.Position.X, p.CurrentPoint.Position.Y))
                                {
                                    resturedCluster = cluster;
                                    result.Add(p);
                                    foreach (My_Point clusterPoint in clusterPoints)
                                    {
                                        result.Add(clusterPoint);
                                    }
                                    My_Point[] argPoints = result.ToArray();
                                    object[] objects = new object[cluster.Senders.Length + 1];
                                    objects[0] = cluster.Points[0].Sender;
                                    for (int i = 0; i < cluster.Senders.Length; i++)
                                    {
                                        objects[i + 1] = cluster.Senders[i];
                                    }
                                    sortingGroupEvent = new Gesture_Event_Sorting_Group();
                                    sortingGroupEvent.Points = cluster.Points;
                                    Gesture_List.addGesture(sortingGroupEvent);
                                    Gesture_SortingGroup_Listener listener = new Gesture_SortingGroup_Listener(controler, sortingGroupEvent);
                                    sortingGroupEvent.Register(objects, argPoints);
                                }
                            }
                        }
                    }
                }
            }
            if (resturedCluster != null)
            {
                Gesture_List.removeGesture(resturedCluster);
                controler.Control.MainWindow.GroupingGestureLayer.Remove(resturedCluster);
                foreach (My_Point p in result)
                {
                    points.Remove(p);
                }
                return sortingGroupEvent;
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
            return !Points[0].IsLive || !Points[1].IsLive || !Points[2].IsLive || !Points[3].IsLive || !Points[4].IsLive;
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
