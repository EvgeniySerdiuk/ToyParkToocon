using System;
using System.Collections.Generic;
using Project.Scripts.Arch.EventBus.Interfaces;

namespace Project.Scripts.Arch.EventBus
{
    public class EventBus : IEventBus
    {
        private Dictionary<Type, List<WeakReference<IBaseEventReceiver>>> receivers;
        private Dictionary<int, WeakReference<IBaseEventReceiver>> receiversHashToReferences;

        public EventBus()
        {
            receivers = new Dictionary<Type, List<WeakReference<IBaseEventReceiver>>>();
            receiversHashToReferences = new Dictionary<int, WeakReference<IBaseEventReceiver>>();
        }

        public void Register<T>(IEventReceiver<T> rec) where T : struct, IEvent
        {
            var type = GetType<T>();
            if(!receivers.ContainsKey(type))
                receivers.Add(type, new List<WeakReference<IBaseEventReceiver>>());
            
            WeakReference<IBaseEventReceiver> reference = new WeakReference<IBaseEventReceiver>(rec);
            
            receivers[type].Add(reference);
            int hash = GetHash(rec, type);
            receiversHashToReferences.Add(hash, reference);
        }

        public void Unregister<T>(IEventReceiver<T> rec) where T : struct, IEvent
        {
            var type = GetType<T>();
            int recHash = GetHash(rec, type);
            if(!receivers.ContainsKey(type) || !receiversHashToReferences.ContainsKey(recHash))
                return;
            
            WeakReference<IBaseEventReceiver> reference = receiversHashToReferences[recHash];
            receivers[type].Remove(reference);
            receiversHashToReferences.Remove(recHash);
        }
        
        public void Raise<T>(T @event) where T : struct, IEvent
        {
            var eventType = GetType<T>();
            if(!receivers.ContainsKey(eventType))
                return;
            
            foreach (var weakRef in receivers[eventType])
            {
                if (weakRef.TryGetTarget(out var rec))
                    ((IEventReceiver<T>) rec).OnEvent(@event);
            }
        }

        private Type GetType<T>()
        {
            return typeof(T);
        }

        private int GetHash<T>(IEventReceiver<T> rec, Type type) where T : struct, IEvent
        {
            string mixedString = new string(rec.ToString() + type.ToString());
            return mixedString.GetHashCode();
        }
    }
        
}