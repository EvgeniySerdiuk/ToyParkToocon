using System;
using Project.Scripts.Arch.Building;
using UnityEngine;

namespace Project.Scripts.InteractiveBuildings
{
    public interface IAvailableBuildingsHandler
    {
        public event Action<IInteractiveBuilding, Vector3> OnInterventionFrequencyEndEvent;
        public event Action<IInteractiveBuilding, Vector3> OnAvailableBuildingAddEvent;
        public event Action<IInteractiveBuilding> OnAvailableBuildingRemoveEvent;
        
        public void AddAvailableBuilding(IInteractiveBuilding building, Vector3 pos);
        public void RemoveAvailableBuilding(IInteractiveBuilding building);
        public bool TryGetClosestFreeBuilding(Vector3 sourcePosition, out IInteractiveBuilding building, out Vector3 pos);
        public bool TryGetFreeBuildingOfType(out IInteractiveBuilding building, out Vector3 pos, BuildingType type);
        public bool TryGetBuildingWithInterventionFrequencyEnd(out IInteractiveBuilding building, out Vector3 pos);
        public bool TryGetBuildingWithInterventionFrequencyEnd(out IInteractiveBuilding building, out Vector3 pos, BuildingType type);
        public InteractionField GetRecieveField(ResourceType type);
        public InteractionField GetRecieveField(IInteractiveBuilding building);
    }
}
