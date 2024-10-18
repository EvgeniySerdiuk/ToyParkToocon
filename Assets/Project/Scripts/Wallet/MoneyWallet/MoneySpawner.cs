using Project.Scripts.Animations;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;

    public class MoneySpawner
    {
        private ObjectsPool<Money> moneyPool;
        private IEventBus eventBus;
        private AudioClip moneyAudio;
        private ItemParabola parabola;
        
        public MoneySpawner(Money moneyPrefab, Transform moneyTransform, IEventBus eventBus, AudioClip moneyAudio, ItemParabola parabola)
        {
            moneyPool = new ObjectsPool<Money>(new []{moneyPrefab}, 10, moneyTransform);
            this.eventBus = eventBus;
            this.moneyAudio = moneyAudio;
            this.parabola = parabola;
        }

        public Money GetMoney(Transform spawnPoint)
        {
           var money =  moneyPool.Get();
           money.Config(parabola, Return);
           money.transform.position = spawnPoint.position;
           money.transform.SetParent(spawnPoint);
           return money;
        }

        private void Return(Money money, Vector3 playerPosition)
        {
            eventBus.Raise(new MoneyAddEvent(money.transform.position, money.AmountMoney, moneyAudio));
            money.ParabolaAnimate(playerPosition, 1f, callback: () => money.gameObject.SetActive(false));
        }
    }
