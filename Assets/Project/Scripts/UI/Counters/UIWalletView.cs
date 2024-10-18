using TMPro;
using UnityEngine;
using Zenject;

public class UIWalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountMoneyText;  

    public Wallet Wallet {get; private set;}


    [Inject]
    private void Init(Wallet wallet)
    {
        this.Wallet = wallet;
    }

    private void Start()
    {
        Wallet.WalletCounter.OnCountChangeEvent += ChangeValue;
        ChangeValue();
    }

    private void ChangeValue()
    {
        amountMoneyText.text = Wallet.WalletCounter.Count.ToString();
    }

    public void Cheat()
    {
        Wallet.AddMoney(5000000);
        ChangeValue();
    }
}
