using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;

namespace Project.Scripts.Arch.EventBus.Events
{
    public readonly struct NewFieldEvent: IEvent
    {
        public readonly GameObject Field;

        public NewFieldEvent(GameObject field)
        {
            Field = field;
        }
    }
}