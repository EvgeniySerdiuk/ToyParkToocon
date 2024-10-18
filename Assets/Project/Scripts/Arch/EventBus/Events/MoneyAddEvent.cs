using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;

namespace Project.Scripts.Arch.EventBus.Events
{
    public readonly struct MoneyAddEvent : IEvent
    {
        public readonly Vector3 Position;
        public readonly int MoneyAmount;
        public readonly AudioClip MoneyAudioClip;
        
        public MoneyAddEvent(Vector3 position, int moneyAmount, AudioClip moneyAudioClip)
        {
            Position = position;
            MoneyAmount = moneyAmount;
            MoneyAudioClip = moneyAudioClip;
        }
    }
}