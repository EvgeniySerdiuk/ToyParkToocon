using Newtonsoft.Json.Linq;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Arch.EventBus.GameEventOperator
{
    [RequireComponent(typeof(SaveableEntity))]
    public class GameEventOperatorView : MonoBehaviour, ICoroutineRunner, ISaveable
    {
        [SerializeField] private BaseEventTrigger[] triggersInWorkOrder;
        
        private IEventOperator op;
        private int currentTrigger;
        
        [Inject]
        private void Init(IEventBus bus)
        {
            op = new GameEventsOperator(bus, this);
        }

        private void Start()
        {
            SetTrigger();
        }

        private void SetTrigger()
        {
            if(currentTrigger >= triggersInWorkOrder.Length)
                return;
            IEventTrigger trigger = triggersInWorkOrder[currentTrigger];
            trigger.OnEventTriggerEnd += TriggerChange;
            op.RegisterEventTrigger(trigger);
        }

        private void TriggerChange(IEventTrigger trigger)
        {
            trigger.OnEventTriggerEnd -= TriggerChange;
            currentTrigger++;
            SetTrigger();
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(currentTrigger);
        }

        public void RestoreFromJToken(JToken state)
        {
            currentTrigger = state.ToObject<int>();
        }
    }
}