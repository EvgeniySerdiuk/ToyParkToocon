using Project.Scripts.AI.Workers;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.Arch.Workers;
using Project.Scripts.InteractiveBuildings;
using Project.Scripts.LocationHolder;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Arch.Factory.Worker
{
    public class WorkerFactory : IWorkerFactory
    {
        private BaseWorker[] workers;
        private Characteristics[] characteristics;
        private ILocationHoldersRoot holderRoot;
        private UIUpgradeFactory upgradeFactory;
        private IEventBus bus;
        private Transform workerParentTransform;
        
        public WorkerFactory(WorkersStorage workersStorage, ILocationHoldersRoot root, UIUpgradeFactory uIUpgradeFactory, IEventBus bus, Transform workerParentTransform)
        {
            workers = workersStorage.Storage;
            characteristics = workersStorage.Characteristics;
            holderRoot = root;
            upgradeFactory = uIUpgradeFactory;
            this.bus = bus;
            this.workerParentTransform = workerParentTransform;
        }
        
        public BaseWorker GetWorker(int workerId,int characteristicId, int locationId, Vector3 spawnPosition)
        {
            BaseWorker worker = GameObject.Instantiate(workers[workerId], spawnPosition, Quaternion.identity);
            Animator animator = worker.gameObject.GetComponent<Animator>();
            WorkersAnimator workersAnimator = new WorkersAnimator(animator);
            NavMeshAgent agent = worker.GetComponent<NavMeshAgent>();
            IAvailableBuildingsHandler buildingsHandler = holderRoot.GetLocationHolder(locationId);
            
            ConfigWorker(worker, buildingsHandler, workersAnimator, agent, spawnPosition, characteristics,
                characteristicId, upgradeFactory);
            bus.Raise(new VfxEvent(spawnPosition, FieldByuUpgrade.ObjectType.Worker, workerId));
            worker.transform.parent = workerParentTransform;
            return worker;
        }

        private void ConfigWorker(BaseWorker worker, IAvailableBuildingsHandler buildingsHandler, WorkersAnimator workersAnimator, NavMeshAgent agent,
            Vector3 spawnPosition, Characteristics[] characteristics, int characteristicId, UIUpgradeFactory upgradeFactory)
        {
            if (worker is IWorkerWithInventory workerWithInventory)
            {
                InventoryWorker inventoryWorker = worker.GetComponent<InventoryWorker>();
                Animator animator = worker.GetComponentInChildren<Animator>();
                WorkersAnimator dockerAnimator = new WorkersAnimator(animator);
                workerWithInventory.Config(buildingsHandler, dockerAnimator, agent, spawnPosition, characteristics[characteristicId],upgradeFactory, inventoryWorker);
                return;
            }
            
            worker.Config(buildingsHandler, workersAnimator, agent, spawnPosition, characteristics[characteristicId],upgradeFactory);
        }
    }
}