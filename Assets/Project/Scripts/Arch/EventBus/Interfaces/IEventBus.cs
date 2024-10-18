namespace Project.Scripts.Arch.EventBus.Interfaces
{
    public interface IEventBus
    {
        public void Raise<T>(T @event) where T : struct, IEvent;
        public void Register<T>(IEventReceiver<T> receiver) where T : struct, IEvent;
        public void Unregister<T>(IEventReceiver<T> receiver) where T : struct, IEvent;
    }
}