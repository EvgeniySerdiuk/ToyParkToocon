using Project.Scripts.InteractiveBuildings;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class RemoveResourceTask : Task
{
    [SerializeField] private BuildingType buildingType;
    [SerializeField] private string buildingName;

    [SerializeField] private LocationHolderr locationHolder;

    private IInteractiveBuilding searchBuild;

    public override Vector3 ObjectPoint => searchBuild.RemoveResourceField.transform.position;


    public override void Complete()
    {
        searchBuild.RemoveResourceField.IsFinishFilling -= Complete;
        base.Complete();
    }

    public override void StartTask()
    {
        var buildings = locationHolder.GetInteractiveBuildingRegister().GetBuildingsInLocation()[(int)buildingType];
        var name = buildingName != "TrashCan" ? buildingName + "(Clone)" : buildingName;

        searchBuild = buildings.FirstOrDefault(x => x.Name == name);

        switch (searchBuild)
        {
            case Toilet toilet :
                if (toilet.NeedClean == false)
                {
                    DelayForNextTask = 0;
                    Complete();
                    return;
                }
                break;
        }

        searchBuild.RemoveResourceField.IsFinishFilling += Complete;
    }
}
