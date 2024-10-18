using System;
using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;

namespace Project.Scripts.Arch.EventBus.Events
{
    public readonly struct MoveCameraEvent : IEvent
    {
        public readonly Vector3 TargetPosition;
        public readonly float FinishCallbackDelay;
        public readonly Action Callback;

        public MoveCameraEvent(Vector3 targetPosition, float finishCallbackDelay, Action callback)
        {
            TargetPosition = targetPosition;
            FinishCallbackDelay = finishCallbackDelay;
            Callback = callback;
        }
    }
}