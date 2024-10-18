using Project.Scripts.Arch.Workers;
using UnityEngine;

namespace Project.Scripts.Arch.Factory.Worker
{
    public interface IWorkerFactory
    {
        public BaseWorker GetWorker(int workerId,int characteristicId, int locationId, Vector3 spawnPosition);
    }
}