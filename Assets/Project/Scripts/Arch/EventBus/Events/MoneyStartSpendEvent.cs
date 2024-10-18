using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;

namespace Project.Scripts.Arch.EventBus.Events
{
    public readonly struct MoneyStartSpendEvent : IEvent
    {
        public readonly Vector3 SpendPosition;
        public readonly Transform MoneySpenderTransform;
        
        public MoneyStartSpendEvent(Vector3 spendPosition, Transform moneySpenderTransform)
        {
            SpendPosition = spendPosition;
            MoneySpenderTransform = moneySpenderTransform;
        }
    }
}