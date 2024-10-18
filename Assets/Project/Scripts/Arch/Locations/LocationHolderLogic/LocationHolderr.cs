using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Arch.Building;
using Project.Scripts.InteractiveBuildings;
using Project.Scripts.InteractiveBuildings.ResourceFieldsRegister;
using Project.Scripts.Interfaces;
using Project.Scripts.LocationHolder;
using UnityEngine;
using Zenject;

public class LocationHolderr : MonoBehaviour, ILocationHolder, ICoroutineRunner
{
    [SerializeField] private int locationId;
    [SerializeField] private Transform clientsParentTransform;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] endPoints;
    [SerializeField] private Client[] clients;

    public event Action<IInteractiveBuilding, Vector3> OnInterventionFrequencyEndEvent;
    public event Action<IInteractiveBuilding, Vector3> OnAvailableBuildingAddEvent;
    public event Action<IInteractiveBuilding> OnAvailableBuildingRemoveEvent;
    
    private SpawnClients clientsSpawner;
    private InteractiveBuildingRegister buildingRegister;
    private ResourceFieldRegister resourceFieldRegister;
    private Dictionary<IInteractiveBuilding, Vector3> availableBuildings;
    private Dictionary<IInteractiveBuilding, Vector3> interventionFrequenceEndBuidlings;

    [Inject]
    private void Init(ILocationHoldersRoot locationHoldersRoot)
    {
        locationHoldersRoot.RegisterLocationHolder(this, locationId);        
        buildingRegister = new InteractiveBuildingRegister();
        buildingRegister.OnNewBuildingRegisterEvent += FollowInteractiveBuildingStateChanges;
        resourceFieldRegister = new ResourceFieldRegister();
        clientsSpawner =
            new SpawnClients(clientsParentTransform, spawnPoints, endPoints, clients, buildingRegister, this);

        availableBuildings = new Dictionary<IInteractiveBuilding, Vector3>();
        interventionFrequenceEndBuidlings = new Dictionary<IInteractiveBuilding, Vector3>();
    }
    
    public SpawnClients GetClientSpawner()
    {
        return clientsSpawner;
    }

    public InteractiveBuildingRegister GetInteractiveBuildingRegister()
    {
        return buildingRegister;
    }

    public ResourceFieldRegister GetResourceFieldRegister()
    {
        return resourceFieldRegister;
    }

    public bool TryGetBuildingWithInterventionFrequencyEnd(out IInteractiveBuilding b, out Vector3 pos)
    {
        b = null;
        pos = Vector3.zero;
        if (interventionFrequenceEndBuidlings.Count < 1)
            return false;
        IInteractiveBuilding[] buildings = GetBuildingsWithInterventionFrequencyEnd();


        for (int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i] != null)
            {
                b = buildings[i];
                pos = interventionFrequenceEndBuidlings[b];
                return true;
            }
        }

        return false;
    }

    public bool TryGetBuildingWithInterventionFrequencyEnd(out IInteractiveBuilding b, out Vector3 pos, BuildingType type)
    {
        b = null;
        pos = Vector3.zero;
        if (interventionFrequenceEndBuidlings.Count < 1)
            return false;
        IInteractiveBuilding[] buildings = GetBuildingsWithInterventionFrequencyEnd();
        int t = (int)type;
        if (buildings[t] != null)
        {
            b = buildings[t];
            pos = interventionFrequenceEndBuidlings[b];
            return true;
        }

        return false;
    }

    private IInteractiveBuilding[] GetBuildingsWithInterventionFrequencyEnd()
    {
        BuildingType maxBuildingsType = Enum.GetValues(typeof(BuildingType)).Cast<BuildingType>().Max();
        IInteractiveBuilding[] buildings = new IInteractiveBuilding[(int) maxBuildingsType+1];
        foreach (var pair in interventionFrequenceEndBuidlings)
            buildings[(int)pair.Key.BuildingType] = pair.Key;

        return buildings;
    }
    
    public InteractionField GetRecieveField(IInteractiveBuilding building)
    {
        ResourceType type = ResourceType.Food;
        switch (building.BuildingType)
        {
            case BuildingType.Attraction:
            {
                type = ResourceType.Ticket;
                break;
            }
            case BuildingType.TrashCan:
            {
                type = ResourceType.Garbage;
                break;
            }
        }
        
        return GetRecieveField(type);
    }
    
    public InteractionField GetRecieveField(ResourceType type)
    {
        return resourceFieldRegister.GetFieldOfResourceType(type);
    }

    public void AddAvailableBuilding(IInteractiveBuilding building, Vector3 pos)
    {
        if(availableBuildings.ContainsKey(building))
            return;
        availableBuildings.Add(building, pos);
        OnAvailableBuildingAddEvent?.Invoke(building, pos);
    }

    public void RemoveAvailableBuilding(IInteractiveBuilding building)
    {
        availableBuildings.Remove(building);
        OnAvailableBuildingRemoveEvent?.Invoke(building);
    }

    public bool TryGetClosestFreeBuilding(Vector3 sourcePosition, out IInteractiveBuilding building, out Vector3 pos)
    {
        building = null;
        pos = Vector3.zero;
        if (availableBuildings.Count < 1)
            return false;
        
        float currentMinDistance = float.MaxValue;
        foreach (var b in availableBuildings)
        {
            float distance = Vector3.Distance(sourcePosition, b.Value);
            if (distance < currentMinDistance)
            {
                building = b.Key;
                pos = b.Value;
                currentMinDistance = distance;
            }
        }
        
        return true;
    }

    public bool TryGetFreeBuildingOfType(out IInteractiveBuilding building, out Vector3 pos, BuildingType type)
    {
        building = null;
        pos = Vector3.zero;
        if (availableBuildings.Count < 1)
            return false;
        foreach (var b in availableBuildings)
        {
            if (b.Key.BuildingType == type)
            {
                building = b.Key;
                pos = b.Value;
                return true;
            }
        }

        return false;
    }

    private void FollowInteractiveBuildingStateChanges(IInteractiveBuilding building)
    {
        building.OnUnoccupiedEvent += AddAvailableBuilding;
        building.OnOccupiedEvent += RemoveAvailableBuilding;
        building.OnInterventionFrequencyEndEvent += InterventionFrequencyEnd;
        building.OnInterventionFrequencyRestoredEvent += InterventionFrequencyRestored;
    }
    
    private void InterventionFrequencyEnd(IInteractiveBuilding building, Vector3 pos)
    {
        if(!interventionFrequenceEndBuidlings.TryAdd(building, pos))
            Debug.Log("Trying to add already existing key in dict");
        OnInterventionFrequencyEndEvent?.Invoke(building, pos);
    }

    private void InterventionFrequencyRestored(IInteractiveBuilding b)
    {
        interventionFrequenceEndBuidlings.Remove(b);
    }
}
