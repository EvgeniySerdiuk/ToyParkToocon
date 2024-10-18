using System;
using System.Collections;
using Project.Scripts.AI.Workers;
using Project.Scripts.Arch.Factory.Worker;
using Project.Scripts.InteractiveBuildings;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Arch.Workers
{
    [RequireComponent(typeof(InventoryWorker))]
    public class Docker : BaseWorker, IWorkerWithInventory
    {
        private InventoryWorker inventoryWorker;
        private Vector3 currentBuidlingPos;
        private float timer;
        
        public void Config(IAvailableBuildingsHandler availableBuilings, WorkersAnimator animator, NavMeshAgent agent,
            Vector3 spawnPoint, Characteristics characteristics, UIUpgradeFactory uiUpgradeFactory, InventoryWorker inventoryWorker)
        {
            base.Config(availableBuilings, animator, agent, spawnPoint, characteristics, uiUpgradeFactory);
            this.inventoryWorker = inventoryWorker;
            inventoryWorker.OnInventoryFullEvent += MoveToAttractionWithFullInventory;
            this.inventoryWorker.OnInventoryEmptyEvent += FullfilledInteractiveBuilding;
            characteristics.GetCharacteristic(CharacteristicType.MoveSpeed).changeValue += ChangeValueCharacteristic;
            agent.speed = characteristics.Value(CharacteristicType.MoveSpeed);
        }

        private void Update()
        {
            if (Time.time >= timer)
            {
                timer = Time.time + 2f;
                CheckInventory();
            }
        }

        private void ChangeValueCharacteristic()
        {
            agent.speed = characteristics.Value(CharacteristicType.MoveSpeed);
        }

        public override void Upgrade()
        {
            uiUpgradeFactory.GetUIUpgrade(characteristics);
        }

        protected override void InterventionFrequencyEnd(IInteractiveBuilding building, Vector3 pos)
        {
            CheckAvailableBuilding();
        }

        protected override void CheckAvailableBuilding()
        {
            //Сейчас работаем
            if (currentBuilding != null || !inventoryWorker.IsEmpty)
                return;
            //Не работаем и нет пустых аттракционов
            if (!IsAvailableBuildings(out var b, out var pos))
            {
                MoveToPosition(spawnPoint);
                return;
            }
            //Есть пустые аттракционы
            currentBuilding = b;
            currentBuidlingPos = pos;
            InteractionField resourceField = availableBuilings.GetRecieveField(b);
            MoveToPosition(resourceField.transform.position);
        }

        private bool IsAvailableBuildings(out IInteractiveBuilding b, out Vector3 pos)
        {
            b = null;
            pos = Vector3.zero;
            if(availableBuilings.TryGetBuildingWithInterventionFrequencyEnd(out var att, out var attPos, BuildingType.Attraction))
            {
                b = att;
                pos = attPos;
                return true;
            }
            if(availableBuilings.TryGetBuildingWithInterventionFrequencyEnd(out var food, out var foodPos, BuildingType.FoodTrack))
            {
                b = food;
                pos = foodPos;
                return true;
            }

            return false;
        }
        
        private void MoveToAttractionWithFullInventory()
        {
            SetTargetBuilding(currentBuilding, currentBuidlingPos);
        }

        protected void FullfilledInteractiveBuilding()
        {
            ResetTargetBuilding();
            currentBuidlingPos = Vector3.zero;
            CheckAvailableBuilding();
        }

        protected override void Add(IInteractiveBuilding building, Vector3 pos)
        {
        }

        protected override void Remove(IInteractiveBuilding building)
        {
        }

        private void CheckInventory()
        {
            if(inventoryWorker.IsFullInventory)
                MoveToAttractionWithFullInventory();
        }
    }
}