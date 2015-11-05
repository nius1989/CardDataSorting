using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CardDesign
{
    /// <summary>
    /// A Controler to create My_Point from the TouchPoint
    /// </summary>
    public class Touch_Controler
    {
        Controlers control;
        public Touch_Controler(Controlers control) {
            this.control = control;
        }
        public static bool isTouched()
        {
            return Point_List.ListSize > 0;
        }
        public void TouchDown(object sdr, Type t, int touchDevice, TouchPoint touchPoint)
        {
            Point_List.AddPoint(sdr, t, touchDevice, touchPoint);
        }

        public My_Point TouchUp(TouchDevice touchDevice, TouchPoint touchPoint)
        {
            if (control.MainWindow.GestureIndicatorLayer.Contain(touchDevice.Id))
            {
                control.MainWindow.GestureIndicatorLayer.Remove(touchDevice.Id);
            }
            My_Point point = Point_List.ReleasePoint(touchDevice.Id);
            if (!isTouched())
            {
                control.MainWindow.GestureIndicatorLayer.Clear();
            }
            return point;
        }

        public void TouchMove(object sdr, Type t, TouchDevice touchDevice, TouchPoint touchPoint)
        {
            My_Point point = Point_List.UpdatePoint(sdr, t, touchDevice.Id, touchPoint);
            if (point != null && (t == typeof(Card_Layer)))
            {
                ///Show gesture indicator

                if (point.Life > STATICS.MIN_GESTURE_LIFE)
                {
                    FrameworkElement sender = point.Sender as FrameworkElement;

                    lock (Point_List.TouchPointList)
                    {
                        if (!control.MainWindow.GestureIndicatorLayer.Contain(touchDevice.Id))
                        {
                            control.MainWindow.GestureIndicatorLayer.Add(touchDevice.Id, touchPoint.Position);
                        }
                        else
                        {
                            control.MainWindow.GestureIndicatorLayer.Move(touchDevice.Id, touchPoint.Position);
                        }
                    }
                }
            }
        }
        public void ReleasePoint(int pointIndex) {
            Point_List.ReleasePoint(pointIndex);
        }
        public override string ToString()
        {
            string info = "";
            if (STATICS.DEBUG_MODE)
            {
                lock (Point_List.TouchPointList)
                {
                    foreach (KeyValuePair<int, My_Point> pair in Point_List.TouchPointList.ToList())
                    {
                        info += String.Join("\t", pair.Value.SenderType.ToString() + "\t");
                        info += pair.Key + "\t";
                        info += pair.Value.CurrentPoint.Position.X + "\t";
                        info += pair.Value.CurrentPoint.Position.Y + "\t";
                        info += pair.Value.Life + "\n";
                    }
                }
            }
            return info;
        }
    }
}
