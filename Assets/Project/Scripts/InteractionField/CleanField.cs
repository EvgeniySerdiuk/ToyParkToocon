using System.Runtime.CompilerServices;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.Interfaces;
using UnityEngine;
using Zenject;

public class CleanField : InteractionField
{
    [SerializeField] private AudioClip cleanAudioClip;
    
    private IEventBus bus;
    private float workSpeed;
    
    [Inject]
    private void Init(IEventBus bus)
    {
        this.bus = bus;
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        CheckWorker(other);
    }

    private void OnTriggerStay(Collider other)
    {
        currentTime += Time.deltaTime;
        FillingProgressBar(currentTime, workSpeed);

        if (progressBar.fillAmount == 1)
        {
            FinishFiling();
            ResetProgressBar();
            if (IsPlayerOnField && player != null)
            {
                player.PlaySound(cleanAudioClip);
                //bus.Raise(new VibrateEvent());
            }
            playerOnField = false;
            player = null;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        ResetProgressBar();
    }

    private void CheckWorker(Collider collider)
    {
        if (collider.TryGetComponent(out IUpgradable upgradable))
        {
            workSpeed = upgradable.Characteristics.Value(CharacteristicType.WorkSpeedWorker);
        }
    }
}
