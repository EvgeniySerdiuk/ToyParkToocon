using System.Collections;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.Interfaces;
using UnityEngine;

namespace Project.Scripts.Effects.VFX
{
    public class VfxHandler : IEventReceiver<VfxEvent>
    {
        private VfxStorage storage;
        private ICoroutineRunner runner;
        
        public VfxHandler(IEventBus bus, VfxStorage storage, ICoroutineRunner runner)
        {
            bus.Register(this);
            this.storage = storage;
            this.runner = runner;
        }

        public void OnEvent(VfxEvent @event)
        {
            SpawnVfx(@event.Type, @event.ObjectId, @event.SpawnPosition);
        }
        
        private void SpawnVfx(FieldByuUpgrade.ObjectType type, int objectId, Vector3 spawnPosition)
        {
            ParticleRoot particleRoot = GetParticle(type, objectId);
            if(particleRoot == null)
                return;

            particleRoot.transform.position = spawnPosition;
            runner.StartCoroutine(PlayAndDeleteByEnd(particleRoot));
        }

        private ParticleRoot GetParticle(FieldByuUpgrade.ObjectType type, int id)
        {
            ParticleRoot p = null;
            if (type == FieldByuUpgrade.ObjectType.Building && storage.BuildingsBuyVfx.Length > id)
                p = Object.Instantiate(storage.BuildingsBuyVfx[id]);
            else if (type == FieldByuUpgrade.ObjectType.Worker && storage.WorkersBuyVfx.Length > id)
                p = Object.Instantiate(storage.WorkersBuyVfx[id]);
            return p;
        }

        private IEnumerator PlayAndDeleteByEnd(ParticleRoot root)
        {
            root.Particle.Play();
            yield return new WaitForSeconds(5f);
            Object.Destroy(root.gameObject);
        }
    }
}