using System;
using System.Collections.Generic;
using System.Linq;

namespace ShuHai
{
    public class SequentialActions : SequentialEvent
    {
        public IEnumerable<Action> Callbacks => callbacks.Cast<Action>();

        public bool AddCallback(Action callback) => base.AddCallback(callback);
        public bool AddCallbackAfter(Action afterMe, Action callback) => base.AddCallbackAfter(afterMe, callback);
        public bool AddCallbackBefore(Action beforeMe, Action callback) => base.AddCallbackBefore(beforeMe, callback);
        public bool RemoveCallback(Action callback) => base.RemoveCallback(callback);

        public void Raise()
        {
            foreach (var callback in Callbacks)
                callback();
        }
    }

    public class SequentialActions<T> : SequentialEvent
    {
        public IEnumerable<Action<T>> Callbacks => callbacks.Cast<Action<T>>();

        public bool AddCallback(Action<T> callback) => base.AddCallback(callback);

        public bool AddCallbackAfter(Action<T> afterMe, Action<T> callback)
            => base.AddCallbackAfter(afterMe, callback);

        public bool AddCallbackBefore(Action<T> beforeMe, Action<T> callback)
            => base.AddCallbackBefore(beforeMe, callback);

        public bool RemoveCallback(Action<T> callback) => base.RemoveCallback(callback);

        public void Raise(T arg)
        {
            foreach (var callback in Callbacks)
                callback(arg);
        }
    }

    public class SequentialActions<T1, T2> : SequentialEvent
    {
        public IEnumerable<Action<T1, T2>> Callbacks => callbacks.Cast<Action<T1, T2>>();

        public bool AddCallback(Action<T1, T2> callback) => base.AddCallback(callback);

        public bool AddCallbackAfter(Action<T1, T2> afterMe, Action<T1, T2> callback)
            => base.AddCallbackAfter(afterMe, callback);

        public bool AddCallbackBefore(Action<T1, T2> beforeMe, Action<T1, T2> callback)
            => base.AddCallbackBefore(beforeMe, callback);

        public bool RemoveCallback(Action<T1, T2> callback) => base.RemoveCallback(callback);

        public void Raise(T1 arg1, T2 arg2)
        {
            foreach (var callback in Callbacks)
                callback(arg1, arg2);
        }
    }

    public class SequentialActions<T1, T2, T3> : SequentialEvent
    {
        public IEnumerable<Action<T1, T2, T3>> Callbacks => callbacks.Cast<Action<T1, T2, T3>>();

        public bool AddCallback(Action<T1, T2, T3> callback) => base.AddCallback(callback);

        public bool AddCallbackAfter(Action<T1, T2, T3> afterMe, Action<T1, T2, T3> callback)
            => base.AddCallbackAfter(afterMe, callback);

        public bool AddCallbackBefore(Action<T1, T2, T3> beforeMe, Action<T1, T2, T3> callback)
            => base.AddCallbackBefore(beforeMe, callback);

        public bool RemoveCallback(Action<T1, T2, T3> callback) => base.RemoveCallback(callback);

        public void Raise(T1 arg1, T2 arg2, T3 arg3)
        {
            foreach (var callback in Callbacks)
                callback(arg1, arg2, arg3);
        }
    }

    public class SequentialActions<T1, T2, T3, T4> : SequentialEvent
    {
        public IEnumerable<Action<T1, T2, T3, T4>> Callbacks => callbacks.Cast<Action<T1, T2, T3, T4>>();

        public bool AddCallback(Action<T1, T2, T3, T4> callback) => base.AddCallback(callback);

        public bool AddCallbackAfter(Action<T1, T2, T3, T4> afterMe, Action<T1, T2, T3, T4> callback)
            => base.AddCallbackAfter(afterMe, callback);

        public bool AddCallbackBefore(Action<T1, T2, T3, T4> beforeMe, Action<T1, T2, T3, T4> callback)
            => base.AddCallbackBefore(beforeMe, callback);

        public bool RemoveCallback(Action<T1, T2, T3, T4> callback) => base.RemoveCallback(callback);

        public void Raise(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            foreach (var callback in Callbacks)
                callback(arg1, arg2, arg3, arg4);
        }
    }
}