using Project.Scripts.AI.Workers;
using Project.Scripts.InteractiveBuildings;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Arch.Factory.Worker
{
    public interface IWorkerWithInventory
    {
        public void Config(IAvailableBuildingsHandler availableBuilings, WorkersAnimator animator, NavMeshAgent agent,
            Vector3 spawnPoint, Characteristics characteristics, UIUpgradeFactory uiUpgradeFactory,
            InventoryWorker inventoryWorker);
    }
}