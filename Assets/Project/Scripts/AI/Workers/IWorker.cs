public interface IWorker
{
    public int UpgradePrice { get; }
    public int AmountExp { get; }
    public bool MaxLevel { get; }
    public void Upgrade();
}
