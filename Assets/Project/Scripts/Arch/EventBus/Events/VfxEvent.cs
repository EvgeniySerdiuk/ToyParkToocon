using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;

namespace Project.Scripts.Arch.EventBus.Events
{
    public readonly struct VfxEvent : IEvent
    {
        public readonly Vector3 SpawnPosition;
        public readonly FieldByuUpgrade.ObjectType Type;
        public readonly int ObjectId;

        public VfxEvent(Vector3 spawnPosition, FieldByuUpgrade.ObjectType type, int id)
        {
            SpawnPosition = spawnPosition;
            Type = type;
            ObjectId = id;
        }
    }
}