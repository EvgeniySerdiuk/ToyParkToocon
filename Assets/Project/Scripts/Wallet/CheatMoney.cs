using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CheatMoney : MonoBehaviour
{
    Wallet wallet;

    [Inject]
    private void Init (Wallet wallet)
    {
        this.wallet = wallet;
    }

    public void Cheat()
    {
        wallet.CheatMoney();
    }
}
