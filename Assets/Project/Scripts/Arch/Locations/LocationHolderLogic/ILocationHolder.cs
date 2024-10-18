
using Project.Scripts.InteractiveBuildings;
using Project.Scripts.InteractiveBuildings.ResourceFieldsRegister;

namespace Project.Scripts.LocationHolder
{
    public interface ILocationHolder : IAvailableBuildingsHandler
    {
        public SpawnClients GetClientSpawner();
        public InteractiveBuildingRegister GetInteractiveBuildingRegister();
        public ResourceFieldRegister GetResourceFieldRegister();
    }
}