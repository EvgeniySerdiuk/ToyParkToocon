using System;
using Project.Scripts.Interfaces;
using UnityEngine;

namespace Project.Scripts.InteractiveBuildings
{
    public interface IInteractiveBuilding : IUpgradable, IBuyable
    {
        public event Action<IInteractiveBuilding> OnOccupiedEvent;
        public event Action<IInteractiveBuilding, Vector3> OnUnoccupiedEvent;
        public event Action<IInteractiveBuilding, Vector3> OnInterventionFrequencyEndEvent;
        public event Action<IInteractiveBuilding> OnInterventionFrequencyRestoredEvent;
        public string Name { get; }
        public bool IsClean { get; }
        
        public InteractionField RemoveResourceField { get; }
        public BuildingType BuildingType { get; }
        public bool TryCheckPlaceInQueue();
        public Vector3 ToGetInQueue(Client client);
    }
}