using System;
using System.Collections.Generic;

namespace ShuHai
{
    using DelegateList = LinkedList<Delegate>;
    using DelegateNode = LinkedListNode<Delegate>;

    /// <summary>
    ///     Represents an event that raises its callbacks in sequential order.
    /// </summary>
    public abstract class SequentialEvent
    {
        #region Callbacks

        public int CallbackCount => callbackList.Count;

        protected IEnumerable<Delegate> callbacks
        {
            get
            {
                // Don't use foreach since you can't modify the list during iterating.
                var node = callbackList.First;
                while (node != null)
                {
                    yield return node.Value;
                    node = node.Next;
                }
            }
        }

        protected bool AddCallback(Delegate callback)
        {
            Ensure.Argument.NotNull(callback, nameof(callback));

            if (callbackNodes.TryGetValue(callback, out _))
                return false;

            var node = callbackList.AddLast(callback);
            callbackNodes.Add(callback, node);
            return true;
        }

        protected bool AddCallbackAfter(Delegate afterMe, Delegate callback)
        {
            Ensure.Argument.NotNull(afterMe, nameof(afterMe));
            Ensure.Argument.NotNull(callback, nameof(callback));

            if (!callbackNodes.TryGetValue(afterMe, out var afterMeNode))
                return false;

            var node = callbackList.AddAfter(afterMeNode, callback);
            callbackNodes.Add(callback, node);
            return true;
        }

        protected bool AddCallbackBefore(Delegate beforeMe, Delegate callback)
        {
            Ensure.Argument.NotNull(beforeMe, nameof(beforeMe));
            Ensure.Argument.NotNull(callback, nameof(callback));

            if (!callbackNodes.TryGetValue(beforeMe, out var beforeMeNode))
                return false;

            var node = callbackList.AddBefore(beforeMeNode, callback);
            callbackNodes.Add(callback, node);
            return true;
        }

        protected bool RemoveCallback(Delegate callback)
        {
            if (!callbackNodes.TryGetValue(callback, out var node))
                return false;

            callbackNodes.Remove(callback);
            callbackList.Remove(node);
            return true;
        }

        public void ClearCallbacks()
        {
            callbackNodes.Clear();
            callbackList.Clear();
        }

        private readonly DelegateList callbackList = new DelegateList();
        private readonly Dictionary<Delegate, DelegateNode> callbackNodes = new Dictionary<Delegate, DelegateNode>();

        #endregion Callbacks
    }
}