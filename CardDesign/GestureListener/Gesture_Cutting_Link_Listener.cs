using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Gesture_Cutting_Link_Listener:Gesture_Listener
    {
        public Gesture_Cutting_Link_Listener(Gesture_Controler gestureControler, Gesture_Event_Cutting_Link gestureEvent)
            : base(gestureControler, gestureEvent)
        {
            
        }
         public override void TerminateGesture(object sender, Gesture_Event_Args gEventArgs)
        {
            Gesture_Event_Linking link = gEventArgs.GestureObjects[0] as Gesture_Event_Linking;
            Link_List.RemoveLink(link);
            gestureControler.Control.MainWindow.LinkingGestureLayer.Remove(link);
            base.TerminateGesture(sender, gEventArgs);
        }

        public override void ContinueGesture(object sender, Gesture_Event_Args gEventArgs)
        {
            base.ContinueGesture(sender, gEventArgs);
        }

        public override void RegisterGesture(object sender, Gesture_Event_Args gEventArgs)
        {
            base.RegisterGesture(sender, gEventArgs);
        }

        public override void FailGesture(object sender, Gesture_Event_Args gEventArgs)
        {           
            base.FailGesture(sender, gEventArgs);
        }
    }
}
