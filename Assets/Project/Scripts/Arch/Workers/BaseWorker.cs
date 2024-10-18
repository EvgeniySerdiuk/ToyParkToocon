using System.Collections;
using Project.Scripts.AI.Workers;
using Project.Scripts.InteractiveBuildings;
using Project.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Arch.Workers
{
    public abstract class BaseWorker : MonoBehaviour, IUpgradable, IBuyable
    {
        protected IAvailableBuildingsHandler availableBuilings;
        protected WorkersAnimator animator;
        protected NavMeshAgent agent;
        protected Vector3 spawnPoint;
        protected IInteractiveBuilding currentBuilding;
        protected WorkerState state;
        protected UIUpgradeFactory uiUpgradeFactory;
        protected Characteristics characteristics;
        public Characteristics Characteristics => characteristics;

        public void Config(IAvailableBuildingsHandler availableBuilings, WorkersAnimator animator, 
            NavMeshAgent agent, Vector3 spawnPoint, Characteristics characteristics, UIUpgradeFactory uiUpgradeFactory)
        {
            this.availableBuilings = availableBuilings;
            this.animator = animator;
            this.agent = agent;
            spawnPoint.y = 0;
            this.spawnPoint = spawnPoint;
            this.characteristics = characteristics;
            this.uiUpgradeFactory = uiUpgradeFactory;
            characteristics.Init();
            SubscriptionOnEvents(availableBuilings);
            StartCoroutine(StartWorkWithDelay());
        }

        private void SubscriptionOnEvents(IAvailableBuildingsHandler availableBuilings)
        {
            availableBuilings.OnAvailableBuildingAddEvent += Add; 
            availableBuilings.OnAvailableBuildingRemoveEvent += Remove;
            availableBuilings.OnInterventionFrequencyEndEvent += InterventionFrequencyEnd;
        }

        public void StartWorking()
        {
            CheckAvailableBuilding();
        }

        private IEnumerator StartWorkWithDelay()
        {
            yield return new WaitForSeconds(2f);
            StartWorking();
        }
        
        private void OnDestroy() 
        { 
            availableBuilings.OnAvailableBuildingAddEvent -= Add; 
            availableBuilings.OnAvailableBuildingRemoveEvent -= Remove;
            availableBuilings.OnInterventionFrequencyEndEvent -= InterventionFrequencyEnd;
        }
        
        public abstract void Upgrade();
        //вызывается когда у интерактивного строения закончились продукты (билеты, еда в лавке)
        protected abstract void InterventionFrequencyEnd(IInteractiveBuilding buidling, Vector3 pos);
        //вызывается когда интерактивное строение не работает (только закончил работу)
        protected abstract void Add(IInteractiveBuilding building, Vector3 pos);
        //вызывается когда интерактивное строение работает (только начало работу)
        protected abstract void Remove(IInteractiveBuilding building);
        protected abstract void CheckAvailableBuilding();

        protected void SetTargetBuilding(IInteractiveBuilding b, Vector3 pos)
        {
            currentBuilding = b;
            MoveToPosition(pos);
        }

        protected void MoveToPosition(Vector3 pos)
        {
            agent.SetDestination(pos);
            SetState(WorkerState.Move);
            StartCoroutine(CheckDestination(pos));
        }
        
        protected void ResetTargetBuilding() 
        { 
            StopAllCoroutines();
            currentBuilding = null; 
            SetState(WorkerState.Idle);
        }
        
        private IEnumerator CheckDestination(Vector3 destination)
        { 
            while (Vector3.Distance(destination, transform.position) > 0.2f) 
                yield return null;
            
            if (currentBuilding != null)
            {
                SetState(WorkerState.Perform);
            }
            else
            {
                SetState(WorkerState.Idle);
            }
        }

        protected void SetState(WorkerState newState)
        {
            state = newState;
            
            if (state == WorkerState.Move)
            {
                agent.isStopped = false;
                animator.SetMoveAnimation();
                return;
            }

            agent.isStopped = true;
            animator.SetIdleAnimation();
        }
    }
}