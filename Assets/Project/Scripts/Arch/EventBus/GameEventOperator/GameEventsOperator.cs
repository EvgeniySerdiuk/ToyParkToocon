using System;
using System.Collections;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.Interfaces;
using UnityEngine;

namespace Project.Scripts.Arch.EventBus.GameEventOperator
{
    public class GameEventsOperator : IEventOperator
    {
        private Camera camera;
        private IEventBus bus;
        private ICoroutineRunner runner;
        private IEventTrigger trigger;

        private EventOperatorData operatorData;
        
        public GameEventsOperator(IEventBus bus, ICoroutineRunner runner)
        {
            this.bus = bus;
            this.runner = runner;
            camera = Camera.main;
        }

        public void RegisterEventTrigger(IEventTrigger eventTrigger)
        {
            if (trigger != null)
                trigger.OnTriggerEvent -= HandleEvent;
            trigger = eventTrigger;
            trigger.OnTriggerEvent += HandleEvent;
        }

        private void HandleEvent(EventOperatorData data)
        {
            operatorData = data;
            if(IsBuyFieldEvent())
                OnBuyableEvent();
            else
                MoveToNextField();
        }

       #region BuyFieldEvent
       private void OnBuyableEvent()
       {
           FieldByuUpgrade field = operatorData.BuyField;
           bus.Raise(new UnlocklFieldStartEvent(field, field.Buyable));
           runner.StartCoroutine(StartWithDelay(MoveToNextField));
        }

        private IEnumerator StartWithDelay(Action action)
        {
            yield return new WaitForSeconds(operatorData.CameraDelayBeforeStartMoving);
            action.Invoke();
        }

        private void MoveToNextField()
        {
            Action callback = null;
            if (IsBuyFieldEvent())
                callback = RaiseUnlockFieldEndEvent;
            if (!IsTransformInView(operatorData.NextTargetPosition))
            {
                bus.Raise(new MoveCameraEvent(operatorData.NextTargetPosition,
                                    operatorData.CameraDelayOnFieldActivation, callback));
            }
            else
            {
                callback?.Invoke();
            }
        }
    
        private void RaiseUnlockFieldEndEvent()
        {
            bus.Raise(new UnlockFieldEndEvent());
        }
        #endregion

        private bool IsBuyFieldEvent()
        {
            return operatorData.BuyField != null;
        }
        
        private bool IsTransformInView(Vector3 globalPosition)
        {
            Vector3 position = camera.WorldToViewportPoint(globalPosition);
            bool x = position.x >= 0 && position.x <= 1;
            bool y = position.y >= 0 && position.y <= 1;
            return x && y && position.z > 0;
        }
    }
}