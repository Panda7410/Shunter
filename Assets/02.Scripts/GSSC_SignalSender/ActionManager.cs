using System.Collections;
using System.Collections.Generic;
using System;

namespace GSSC
{
    public class ActionManager
    {
        static ActionManager s_instance;
        public static ActionManager Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new ActionManager();
                return s_instance;
            }
            set
            {
                if (value == null)
                    s_instance.ActionList.Clear();
                s_instance = value;
            }
        }
        private Dictionary<string, Dictionary<string, ActionSet>> ActionListList = new Dictionary<string, Dictionary<string, ActionSet>>();
        private Dictionary<string, ActionSet> ActionList = new Dictionary<string, ActionSet>();

        private Dictionary<string, ActionSet> GetActionListList(string GroupKey)
        {
            if (!ActionListList.ContainsKey(GroupKey))
                ActionListList.Add(GroupKey, new Dictionary<string, ActionSet>());
            return ActionListList[GroupKey];
        }
        private ActionSet GetActionSet(string Key)
        {
            if (!ActionList.ContainsKey(Key))
                ActionList.Add(Key, new ActionSet());
            return ActionList[Key];
        }
        private ActionSet GetActionSet(string Group, string Key)
        {
            if (!GetActionListList(Group).ContainsKey(Key))
                GetActionListList(Group).Add(Key, new ActionSet());
            return GetActionListList(Group)[Key];
        }
        public void InvokeAction(string Group, string Key)
            => GetActionSet(Group, Key).InvokeAction();
        public void InvokeAction(string Group, string Key, bool ActionBool)
            => GetActionSet(Group, Key).InvokeAction(ActionBool);
        public void InvokeAction(string Group, string Key, int ActionInt)
            => GetActionSet(Group, Key).InvokeAction(ActionInt);
        public void InvokeAction(string Group, string Key, float ActionFloat)
            => GetActionSet(Group, Key).InvokeAction(ActionFloat);
        public void InvokeAction(string Group, string Key, string ActionString)
            => GetActionSet(Group, Key).InvokeAction(ActionString);
        public void InvokeAction(string Group, string Key, object ActionObject)
            => GetActionSet(Group, Key).InvokeAction(ActionObject);

        public void AddAction(string Group, string Key, Action action)
            => GetActionSet(Group, Key).DefAtion += action;
        public void AddAction(string Group, string Key, Action<bool> BoolAction)
            => GetActionSet(Group, Key).BoolAction -= BoolAction;
        public void AddAction(string Group, string Key, Action<int> IntAction)
            => GetActionSet(Group, Key).IntAction += IntAction;
        public void AddAction(string Group, string Key, Action<float> FloatAction)
            => GetActionSet(Group, Key).FloatAtion += FloatAction;
        public void AddAction(string Group, string Key, Action<string> StringAction)
            => GetActionSet(Group, Key).StringAction += StringAction;
        public void AddAction(string Group, string Key, Action<object> ObjectAction)
            => GetActionSet(Group, Key).ObjectAction += ObjectAction;

        public void RemoveAction(string Group, string Key, Action action)
            => GetActionSet(Group, Key).DefAtion -= action;
        public void RemoveAction(string Group, string Key, Action<bool> BoolAction)
            => GetActionSet(Group, Key).BoolAction -= BoolAction;
        public void RemoveAction(string Group, string Key, Action<int> IntAction)
            => GetActionSet(Group, Key).IntAction -= IntAction;
        public void RemoveAction(string Group, string Key, Action<float> FloatAction)
            => GetActionSet(Group, Key).FloatAtion -= FloatAction;
        public void RemoveAction(string Group, string Key, Action<string> StringAction)
            => GetActionSet(Group, Key).StringAction -= StringAction;
        public void RemoveAction(string Group, string Key, Action<object> ObjectAction)
            => GetActionSet(Group, Key).ObjectAction -= ObjectAction;
        public List<string> GetKeys()
        {
            List<string> keys = new List<string>();
            keys.ForEach(t => keys.Add(t));
            return keys;
        }


        private class ActionSet
        {
            public Action DefAtion;
            public Action<bool> BoolAction;
            public Action<int> IntAction;
            public Action<float> FloatAtion;
            public Action<string> StringAction;
            public Action<object> ObjectAction;

            public void InvokeAction()
            => DefAtion?.Invoke();
            public void InvokeAction(bool ActionBool)
            => BoolAction?.Invoke(ActionBool);
            public void InvokeAction(int ActionInt)
            => IntAction?.Invoke(ActionInt);
            public void InvokeAction(float ActionFloat)
            => FloatAtion?.Invoke(ActionFloat);
            public void InvokeAction(string ActionString)
            => StringAction?.Invoke(ActionString);
            public void InvokeAction<T>(T ActionObject)
            => ObjectAction?.Invoke(ActionObject);

        }
    }
}