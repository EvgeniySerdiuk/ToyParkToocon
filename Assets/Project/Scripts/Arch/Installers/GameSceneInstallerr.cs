using UnityEngine;
using Zenject;

public class GameSceneInstallerr : MonoInstaller
{
    [SerializeField] private UnlockLocations unlockLocations;
    [SerializeField] private InteractionAudioPlayer interactionAudioPlayer;

    public override void InstallBindings()
    {
        Container.Bind<UnlockLocations>().FromInstance(unlockLocations).AsSingle();
        Container.Bind<InteractionAudioPlayer>().FromInstance(interactionAudioPlayer).AsSingle().NonLazy();
    }
}
