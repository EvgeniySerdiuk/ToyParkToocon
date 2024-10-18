namespace Project.Scripts.Arch.EventBus.Interfaces
{
    public interface IEventReceiver<T> : IBaseEventReceiver where T :struct, IEvent
    {
        public void OnEvent(T @event);
    }
}