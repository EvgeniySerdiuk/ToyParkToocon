using Project.Scripts.Arch.Workers;
using Project.Scripts.InteractiveBuildings;
using UnityEngine;

public class Casher : BaseWorker
{
    public override void Upgrade()
    {
        uiUpgradeFactory.GetUIUpgrade(characteristics);
    }

    protected override void InterventionFrequencyEnd(IInteractiveBuilding buidling, Vector3 pos)
    {
        if(currentBuilding != buidling)
            return;
        ResetTargetBuilding();
        CheckAvailableBuilding();
    }

    protected override void Add(IInteractiveBuilding building, Vector3 pos) 
    { 
        if (state != WorkerState.Idle 
            || currentBuilding != null) 
            return;
        CheckAvailableBuilding();
    }
    
    protected override void Remove(IInteractiveBuilding building)
    { 
        if (currentBuilding != building) 
            return;
        ResetTargetBuilding(); 
        CheckAvailableBuilding();
    }

    protected override void CheckAvailableBuilding()
    {
        if (availableBuilings.TryGetFreeBuildingOfType(out IInteractiveBuilding b, out Vector3 pos, BuildingType.Attraction)) 
        { 
            SetTargetBuilding(b, pos);
        }
    }
}