using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Gesture_Collaborative_Zoon_Listener:Gesture_Listener
    {
        public Gesture_Collaborative_Zoon_Listener(Gesture_Controler gestureControler, Gesture_Event_Collaborative_Zoon gestureEvent)
            : base(gestureControler, gestureEvent)
        {

        }

        public override void TerminateGesture(object sender, Gesture_Event_Args gEventArgs)
        {
            News_Card card = gEventArgs.GestureObjects[0] as News_Card;
            Shared_Card_List.UpdateSharedCards();
            gestureControler.Control.CloudControler.UpdateMatrix();
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
            News_Card card = sender as News_Card;
            Shared_Card_List.UpdateSharedCards();
            gestureControler.Control.CloudControler.UpdateMatrix();
            base.FailGesture(sender, gEventArgs);
        }
    }
}
