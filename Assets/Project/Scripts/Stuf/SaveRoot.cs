using System.Collections;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;
using Zenject;

public class SaveRoot : MonoBehaviour, IEventReceiver<SaveEvent>
{
    [SerializeField] private int walletSaveCooldown;
    
    private SavingWrapper wrapper;
    private IEventBus bus;
    
    [Inject]
    private void Init(SavingWrapper wrapper, IEventBus bus)
    {
        this.wrapper = wrapper;
        this.bus = bus;
        bus.Register(this);
    }

    private void OnDestroy()
    {
        bus.Unregister(this);
        if(wrapper.IsSaveExist())
            wrapper.Save();
    }

    private void Start()
    {
        StartCoroutine(WalletSaveAsync());
    }

    
    private IEnumerator WalletSaveAsync()
    {
        while (true)
        {
            wrapper.SaveWallet();
            yield return new WaitForSeconds(walletSaveCooldown);
        }
    }

    public void OnEvent(SaveEvent @event)
    {
        wrapper.Save();
    }
}
