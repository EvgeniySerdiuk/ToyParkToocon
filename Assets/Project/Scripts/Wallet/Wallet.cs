using Newtonsoft.Json.Linq;
using Project.Scripts.Wallet;

public class Wallet : ISaveable
{
    private Counter walletCounter;
    public Counter WalletCounter => walletCounter;

    public Wallet(bool firstTime)
    {
        walletCounter = new Counter();
        if (firstTime)
            walletCounter.SetCount(3000);
    }

    public void AddMoney(int amountMoney)
    {
        walletCounter.TryChangeCountByVal(amountMoney);
    }

    public void RemoveMoney(int amountMoney) 
    {
        walletCounter.TryChangeCountByVal(-amountMoney);
    }

    public JToken CaptureAsJToken()
    {
        return walletCounter.CaptureAsJToken();
    }

    public void RestoreFromJToken(JToken state)
    {
        walletCounter.RestoreFromJToken(state);
    }

    public void CheatMoney()
    {
        AddMoney(50000);
    }
}
