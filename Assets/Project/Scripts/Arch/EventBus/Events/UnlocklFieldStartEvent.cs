using Project.Scripts.Arch.EventBus.Interfaces;

namespace Project.Scripts.Arch.EventBus.Events
{
    public readonly struct UnlocklFieldStartEvent : IEvent
    {
        public readonly FieldByuUpgrade Field;
        public readonly IBuyable BoughtObject;

        public UnlocklFieldStartEvent(FieldByuUpgrade field, IBuyable buyable)
        {
            Field = field;
            BoughtObject = buyable;
        }
    }
}