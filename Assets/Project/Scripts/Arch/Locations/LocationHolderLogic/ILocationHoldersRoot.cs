
namespace Project.Scripts.LocationHolder
{
    public interface ILocationHoldersRoot
    {
    public ILocationHolder GetLocationHolder(int locationId);
    public void RegisterLocationHolder(ILocationHolder holder, int locationId);
    }
}