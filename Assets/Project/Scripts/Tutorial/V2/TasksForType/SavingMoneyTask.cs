using System;
using UnityEngine;
using Zenject;

[Serializable]
public class SavingMoneyTask : Task
{
    [SerializeField] private int amountSavableMoney;
    [SerializeField] private UIWalletView walletView;
    public override Vector3 ObjectPoint => Vector3.zero;
    private ICounter wallet;

    public override void StartTask()
    {
        wallet = walletView.Wallet.WalletCounter;
        wallet.OnCountChangeEvent += Complete;
        Complete();
    }

    public override void Complete()
    {
        if(wallet.Count >= amountSavableMoney)
        {
            wallet.OnCountChangeEvent -= Complete;
            DelayForNextTask = 0;
            base.Complete();
        }
    }
}
