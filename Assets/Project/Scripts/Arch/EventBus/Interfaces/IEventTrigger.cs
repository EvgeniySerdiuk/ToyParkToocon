using System;

namespace Project.Scripts.Arch.EventBus.Interfaces
{
    public interface IEventTrigger
    {
        public event Action<EventOperatorData> OnTriggerEvent;
        public event Action<FieldByuUpgrade> OnBuyEvent;
        public event Action<IEventTrigger> OnEventTriggerEnd;
    }
}