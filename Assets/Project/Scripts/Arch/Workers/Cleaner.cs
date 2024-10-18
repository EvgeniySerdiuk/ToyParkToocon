using Project.Scripts.AI.Workers;
using Project.Scripts.Arch.Factory.Worker;
using Project.Scripts.Arch.Workers;
using Project.Scripts.InteractiveBuildings;
using UnityEngine;
using UnityEngine.AI;

public class Cleaner : BaseWorker, IWorkerWithInventory
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
    }

    protected override void Add(IInteractiveBuilding building, Vector3 pos)
    {
        if(!IsValidBuildingType(building.BuildingType) || currentBuilding != null)
            return;
        CheckAvailableBuilding();
    }

    private bool IsValidBuildingType(BuildingType type)
    {
        return type == BuildingType.Toilet || type == BuildingType.TrashCan;
    }
    
    protected override void Remove(IInteractiveBuilding building)
    {
        if(currentBuilding != building || !inventoryWorker.IsEmpty)
            return;
        FullfilledInteractiveBuilding();
    }

    protected override void CheckAvailableBuilding()
    {       
        //Если зависли на туалете, но он уже почищен
        if (currentBuilding != null && currentBuilding.BuildingType == BuildingType.Toilet && currentBuilding.IsClean)
        {
            FullfilledInteractiveBuilding();
            return;
        }
        //Сейчас работаем или есть работа
        if (currentBuilding != null || !inventoryWorker.IsEmpty || CleanAvailableAttraction())
            return;
        //Не работаем и нет пустых аттракционов
        MoveToPosition(spawnPoint);
    }

    private bool CleanAvailableAttraction()
    {
        if(availableBuilings.TryGetFreeBuildingOfType(out var building, out var pos, BuildingType.Toilet))
        {
            SetTargetBuilding(building, pos);
            return true;
        }
        if (availableBuilings.TryGetFreeBuildingOfType(out var can, out var position, BuildingType.TrashCan))
        {
            //Есть пустые аттракционы
            currentBuilding = can;
            currentBuidlingPos = position;
            InteractionField resourceField = availableBuilings.GetRecieveField(can);
            MoveToPosition(resourceField.transform.position);
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
    
    private void CheckInventory()
    {
        if(inventoryWorker.IsFullInventory)
            MoveToAttractionWithFullInventory();
    }
}