using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSSC;
namespace GSSC.Signal
{
    [System.Serializable]
    public class SignalSend
    {
        public SignalTag SignalTag;

        public void SendAction() 
            => ActionManager.Instance.InvokeAction(SignalTag.Group, SignalTag.Tag);
        public void SendAction(bool ActionBool)
            => ActionManager.Instance.InvokeAction(SignalTag.Group, SignalTag.Tag, ActionBool: ActionBool);
        public void SendAction(int ActionInt) 
            => ActionManager.Instance.InvokeAction(SignalTag.Group, SignalTag.Tag, ActionInt: ActionInt);
        public void SendAction(float ActionFloat) 
            => ActionManager.Instance.InvokeAction(SignalTag.Group, SignalTag.Tag, ActionFloat: ActionFloat);
        public void SendAction(string ActionString) 
            => ActionManager.Instance.InvokeAction(SignalTag.Group, SignalTag.Tag, ActionString: ActionString);
        public void SendAction(object ActionObect)
            => ActionManager.Instance.InvokeAction(SignalTag.Group, SignalTag.Tag, ActionObject: ActionObect);
    }
}