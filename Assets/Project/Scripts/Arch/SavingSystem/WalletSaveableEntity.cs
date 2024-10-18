using System.Linq;

namespace Project.Scripts.SavingSystem
{
    public class WalletSaveableEntity : SaveableEntity
    {
        protected override void OnAwake()
        {
            saveables = GetComponents<ISaveable>().ToList();
            SavingWrapper.Wallet.Add(this);
        }
    }
}