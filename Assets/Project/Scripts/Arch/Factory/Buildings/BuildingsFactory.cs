using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.InteractiveBuildings;
using Project.Scripts.LocationHolder;
using UnityEngine;

public class BuildingsFactory
{
    private InteractiveBuildingStorage buildingStorage;
    private ILocationHoldersRoot holdersRoot;
    private MoneySpawner moneySpawner;
    private UIUpgradeFactory upgradeFactory;
    private IEventBus bus;
    
    public BuildingsFactory(InteractiveBuildingStorage buildingStorage, ILocationHoldersRoot holdersRoot, MoneySpawner moneySpawner, UIUpgradeFactory upgradeFactory, IEventBus bus) 
    {
        this.buildingStorage = buildingStorage;
        this.holdersRoot = holdersRoot;
        this.moneySpawner = moneySpawner;
        this.upgradeFactory = upgradeFactory;
        this.bus = bus;
    }

    public IInteractiveBuilding GetBuilding(Transform position, int locationId, int buildingId, int idCharacteristic)
    {
        var building = GameObject.Instantiate(buildingStorage.AvailableBuildings[buildingId], position.position, position.rotation);
        SetTransformAsParent(building.transform, position);
        
        ILocationHolder holder = holdersRoot.GetLocationHolder(locationId);
        holder.GetInteractiveBuildingRegister().Register(building, building.BuildingType);
        building.Config(moneySpawner,upgradeFactory, buildingStorage.Characteristics[idCharacteristic], bus);

        if(building is Attraction attraction)
        {
            attraction.SetSpawnClietns(holder.GetClientSpawner());
        }
        bus.Raise(new VfxEvent(position.position, FieldByuUpgrade.ObjectType.Building, buildingId));
        return building;
    }

    private void SetTransformAsParent(Transform childTransform, Transform parentTransform)
    {
        childTransform.parent = parentTransform;
        childTransform.localPosition = Vector3.zero;
        childTransform.localRotation = Quaternion.identity;
    }
}
