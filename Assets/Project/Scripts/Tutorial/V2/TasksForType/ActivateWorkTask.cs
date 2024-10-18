using Project.Scripts.InteractiveBuildings;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ActivateWorkTask : Task
{
    [SerializeField] private BuildingType buildingType;
    [SerializeField] private string buildingName;

    [SerializeField] private LocationHolderr locationHolder;

    [SerializeField] private int manyTimesActivate;

    private IInteractiveBuilding interactiveBuilding;

    public override Vector3 ObjectPoint => interactiveBuilding.RemoveResourceField.transform.position;

    public override void Complete()
    {
        manyTimesActivate--;

        if(manyTimesActivate == 0)
        {
            interactiveBuilding.OnOccupiedEvent -= (IInteractiveBuilding) => Complete();
            base.Complete();
        }
    }

    public override void StartTask()
    {
        var buildings = locationHolder.GetInteractiveBuildingRegister().GetBuildingsInLocation()[(int)buildingType];

        interactiveBuilding = buildings.FirstOrDefault(x => x.Name == buildingName + "(Clone)");

        if (interactiveBuilding != null)
        {
            interactiveBuilding.OnOccupiedEvent += (IInteractiveBuilding) => Complete();
        }
    }
}
