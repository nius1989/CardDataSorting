using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Gesture_Deleting_Bin_Listener : Gesture_Listener
    {
        public Gesture_Deleting_Bin_Listener(Gesture_Controler gestureControler, Gesture_Event_Deleting_Bin gestureEvent)
            : base(gestureControler, gestureEvent)
        {

        }
        public override void TerminateGesture(object sender, Gesture_Event_Args gEventArgs)
        {
            Menu_Sort_Box box = gEventArgs.GestureObjects[0] as Menu_Sort_Box;
            gestureControler.Control.SortingBoxControler.DeleteGroup(box);
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
