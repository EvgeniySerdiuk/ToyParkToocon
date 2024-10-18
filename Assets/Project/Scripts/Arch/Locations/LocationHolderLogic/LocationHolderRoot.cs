namespace Project.Scripts.LocationHolder
{
    public class LocationHolderRoot : ILocationHoldersRoot
    {
        private ILocationHolder[] holders;
        
        public LocationHolderRoot(int locationsCount = 5)
        {
            holders = new ILocationHolder[locationsCount];
        }
        
        public ILocationHolder GetLocationHolder(int locationId)
        {
            return holders[locationId];
        }

        public void RegisterLocationHolder(ILocationHolder holder, int locationId)
        {
            holders[locationId] = holder;
        }
    }
}