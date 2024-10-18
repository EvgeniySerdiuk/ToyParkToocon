using System;
using Newtonsoft.Json.Linq;
using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;

namespace Project.Scripts.Arch.EventBus
{
    [RequireComponent(typeof(SaveableEntity))]
    public abstract class BaseEventTrigger : MonoBehaviour, IEventTrigger, ISaveable
    {
        [Header("Camera event settings")]
        [SerializeField] protected float cameraDelayOnFieldActivation = 1f;
        [SerializeField] protected float cameraDelayBeforeStartMoving = 1.5f;

        protected SaveableEntity saveableEntity;

        public event Action<EventOperatorData> OnTriggerEvent;
        public event Action<FieldByuUpgrade> OnBuyEvent;
        public event Action<IEventTrigger> OnEventTriggerEnd;

        private void Awake()
        {
            saveableEntity = gameObject.GetComponent<SaveableEntity>();
        }

        protected void Trigger(EventOperatorData data)
        {
            OnTriggerEvent?.Invoke(data);
        }

        protected void BuyEvent(FieldByuUpgrade field)
        {
            OnBuyEvent?.Invoke(field);
        }

        protected void TriggerEnd()
        {
            OnEventTriggerEnd?.Invoke(this);
        }

        public abstract JToken CaptureAsJToken();
        public abstract void RestoreFromJToken(JToken state);
    }
}