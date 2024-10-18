namespace Project.Scripts.Interfaces
{
    public interface IUpgradable
    {
        public Characteristics Characteristics { get; }
        public void Upgrade();
    }
}