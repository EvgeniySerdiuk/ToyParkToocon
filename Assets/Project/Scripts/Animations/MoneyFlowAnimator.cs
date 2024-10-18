using System.Collections;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.Interfaces;
using UnityEngine;

namespace Project.Scripts.Animations
{
    public class MoneyFlowAnimator : IEventReceiver<MoneyStartSpendEvent>, IEventReceiver<MoneyEndSpendEvent>
    {
        private ObjectsPool<BaseAnimatedItem> moneyPool;
        private ICoroutineRunner runner;
        private bool stop;
        private int gapBetweenMoneySpawn;
        private ItemParabola parabola;
        
        public MoneyFlowAnimator(IEventBus bus, BaseAnimatedItem moneyrefab, Transform parentTransform, ICoroutineRunner runner, int gapBetweenMoneySpawn, ItemParabola parabola)
        {
            bus.Register<MoneyStartSpendEvent>( this);
            bus.Register<MoneyEndSpendEvent>(this);
            moneyPool = new ObjectsPool<BaseAnimatedItem>(new []{moneyrefab}, 10, parentTransform);
            this.runner = runner;
            if (gapBetweenMoneySpawn < 1)
                gapBetweenMoneySpawn = 1;
            this.gapBetweenMoneySpawn = gapBetweenMoneySpawn;
            this.parabola = parabola;
        }

        public void OnEvent(MoneyStartSpendEvent @event)
        {
            stop = false;
            runner.StartCoroutine(AnimateFlow(@event.MoneySpenderTransform, @event.SpendPosition));
        }

        public void OnEvent(MoneyEndSpendEvent @event)
        {
            stop = true;
        }

        private IEnumerator AnimateFlow(Transform spenderTransform, Vector3 endPoint)
        {
            while (!stop)
            {
                BaseAnimatedItem animatedMoney = moneyPool.Get();
                animatedMoney.Config(parabola);
                animatedMoney.transform.position = spenderTransform.position + Vector3.up * 2;
                animatedMoney.ParabolaAnimate(endPoint, 1, callback: () => animatedMoney.gameObject.SetActive(false));
                yield return new WaitForSeconds(Time.deltaTime * gapBetweenMoneySpawn);
            }
        }
    }
}