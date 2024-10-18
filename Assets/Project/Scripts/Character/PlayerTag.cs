using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class PlayerTag : MonoBehaviour, IEventReceiver<MoneyAddEvent>
{
    [SerializeField] private AudioClip dropSound;
    
    private PlayerExperience exp;
    private InteractionAudioPlayer audioPlayer;
    private Wallet wallet;
    private IEventBus bus;

    [Inject]
    private void Init(PlayerExperience exp, InteractionAudioPlayer audioPlayer, Wallet w, IEventBus bus)
    {
        this.exp = exp;
        this.audioPlayer = audioPlayer;
        wallet = w;
        this.bus = bus;
    }

    private void OnEnable()
    {
        bus.Register<MoneyAddEvent>(this);
    }

    private void OnDisable()
    {
        bus.Unregister<MoneyAddEvent>(this);
    }

    public void AddExp(int amount)
    {
        exp.AddExp(amount);
    }

    public void OnEvent(MoneyAddEvent @event)
    {
        wallet.AddMoney(@event.MoneyAmount);
        audioPlayer.PlayAudio(@event.MoneyAudioClip,0.7f);
    }


    public void PlaySound(AudioClip clip)
    {
        audioPlayer.PlayAudio(clip);
    }

    public void PlayDropSound()
    {
        PlaySound(dropSound);
    }
}
