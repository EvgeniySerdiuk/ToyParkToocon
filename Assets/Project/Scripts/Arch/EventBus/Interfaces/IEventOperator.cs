namespace Project.Scripts.Arch.EventBus.Interfaces
{
    public interface IEventOperator 
    {
        public void RegisterEventTrigger(IEventTrigger eventTrigger);
    }
}