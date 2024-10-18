using Project.Scripts.Arch.Workers;
using Project.Scripts.InteractiveBuildings;
using UnityEngine;

public class FoodDealler : BaseWorker
{
    public override void Upgrade()
    {
        uiUpgradeFactory.GetUIUpgrade(characteristics);
    }

    protected override void Add(IInteractiveBuilding building, Vector3 pos)
    {
        if(building.BuildingType != BuildingType.FoodTrack 
           || currentBuilding != null)
            return;
        SetTargetBuilding(building, pos);
    }

    protected override void Remove(IInteractiveBuilding building)
    {
    }

    protected override void InterventionFrequencyEnd(IInteractiveBuilding buidling, Vector3 pos)
    {
    }
    
    protected override void CheckAvailableBuilding()
    {
        //if we food dealer already have building (food track) we return
        if(currentBuilding != null)
            return;
        //search first free food track
        if (availableBuilings.TryGetFreeBuildingOfType(out IInteractiveBuilding b, out Vector3 pos, BuildingType.FoodTrack))
        {
            SetTargetBuilding(b, pos);
        }
    }
}
