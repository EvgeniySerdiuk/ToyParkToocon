using Zenject;
using Project.Scripts;
using UnityEngine;
using Project.Scripts.SceneManagement;
using Project.Scripts.Interfaces;
using UnityEngine.Audio;
using System.Collections;
using Project.Scripts.Animations;
using Project.Scripts.Arch.EventBus;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.Arch.Factory.Worker;
using Project.Scripts.Effects.VFX;
using Project.Scripts.LocationHolder;

namespace Assets.Project.Scripts.Installers
{
    public class ProjectBootstrapp : MonoInstaller, ICoroutineRunner
    {
        [SerializeField] private SavingStrategy savingStrategy;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioClip moneyTakeAudio;
        [SerializeField] private Money moneyPrefab;
        [SerializeField] private Transform moneyTransform;
        [SerializeField] private InteractiveBuildingStorage interactiveBuildingStorage;
        [SerializeField] private WorkersStorage workersStorage;
        [SerializeField] private Transform workerParentTransform;
        
        [Header("UIUpgradeFactory")]
        [SerializeField] private UIUpgradeView upgradeView;
        [SerializeField] private UIUpgradeSlot upgradeSlot;

        [Header("ParabolaAnimationCurve")] 
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private BaseAnimatedItem moneyBaseAnimatedPrefab;
        [SerializeField] private Transform animatedMoneyParent;
        [SerializeField] private int gapBetweenMoneySpawn;

        [Header("VFX")] 
        [SerializeField] private VfxStorage vfxStorage;
        [SerializeField] private long baseVibrationTimeInMilliseconds = 100L;
        [SerializeField] private float gapBetweenVibrationsOnMoneySpend = 0.25f;
        
        private SavingWrapper savingWrapper;
        private SceneChanger sceneChanger;
        private ILocationHoldersRoot locationHolderRoot;
        private IEventBus eventBus;
        private ItemParabola parabola;
        private MoneyFlowAnimator moneyFlowAnimator;
        private VfxHandler vfxHandler;
        private Wallet wallet;
        private VibrationHandler vibrationHandler;
        
        public override void InstallBindings()
        {
            BindSavingSystem();
            BindSceneChanger();
            BindAudioMixer();
            BindLocationHolderRoot();
            BindEventBus();
            BindParabolaAnimator();
            BindOtherServices();
            BindFactories();
            Config();
            gameObject.SetActive(true);
            StartCoroutine(Timer());
        }

        private void BindSavingSystem()
        {
            SavingSystem savingSystem = new SavingSystem(savingStrategy);
            savingWrapper = new SavingWrapper(savingSystem);
            Container.Bind<SavingWrapper>().FromInstance(savingWrapper).AsSingle().NonLazy();
        }

        private void BindSceneChanger()
        {
            sceneChanger = new SceneChanger(savingWrapper, this);
            Container.Bind<SceneChanger>().FromInstance(sceneChanger).AsSingle().NonLazy();
        }

        private void BindAudioMixer()
        {
            AudioMixerHandler mixer = new AudioMixerHandler(audioMixer);
            Container.Bind<AudioMixerHandler>().FromInstance(mixer).AsSingle();
        }

        private void BindLocationHolderRoot()
        {
            locationHolderRoot = new LocationHolderRoot(4);
            Container.Bind<ILocationHoldersRoot>().FromInstance(locationHolderRoot).AsSingle().NonLazy();
        }

        private void BindEventBus()
        {
            eventBus = new EventBus();
            Container.Bind<IEventBus>().FromInstance(eventBus).AsSingle().NonLazy();
        }

        private void BindParabolaAnimator()
        {
            parabola = new ItemParabola(this, curve);
            Container.Bind<ItemParabola>().FromInstance(parabola).AsSingle().NonLazy();
        }
        
        private void BindFactories()
        {
            var moneySpawner = new MoneySpawner(moneyPrefab, moneyTransform, eventBus, moneyTakeAudio, parabola);
            Container.Bind<MoneySpawner>().FromInstance(moneySpawner).AsSingle().NonLazy();
            //то что выше просто засунуть в билдинг фактори
            UIUpgradeFactory UiuFactory = new UIUpgradeFactory(upgradeView, upgradeSlot,wallet.WalletCounter, eventBus);
            Container.Bind<UIUpgradeFactory>().FromInstance(UiuFactory).AsSingle().NonLazy();

            WorkerFactory wFactory = new WorkerFactory(workersStorage, locationHolderRoot, UiuFactory, eventBus, workerParentTransform);
            Container.Bind<IWorkerFactory>().FromInstance(wFactory).AsSingle().NonLazy();
            
            BuildingsFactory bFactory = new BuildingsFactory(interactiveBuildingStorage, locationHolderRoot, moneySpawner, UiuFactory, eventBus);
            Container.Bind<BuildingsFactory>().FromInstance(bFactory).AsSingle().NonLazy();
        }

        private void BindOtherServices()
        {
            wallet = new Wallet(!savingWrapper.IsSaveExist());
            Container.Bind<Wallet>().FromInstance(wallet).AsSingle().NonLazy();

            //Container.Bind<ADSManager>().FromInstance(new ADSManager(wallet)).AsSingle().NonLazy();

            var exp = new PlayerExperience();
            Container.Bind<PlayerExperience>().FromInstance(exp).AsSingle().NonLazy();

            GlobalEvents events = new GlobalEvents(savingWrapper, exp);
            Container.Bind<GlobalEvents>().FromInstance(events);

            Container.Bind<LastBreathWrapper>().FromNew().AsSingle();
        }

        private void Config()
        {
            moneyFlowAnimator = new MoneyFlowAnimator(eventBus, moneyBaseAnimatedPrefab, animatedMoneyParent, this, gapBetweenMoneySpawn, parabola);
            vfxHandler = new VfxHandler(eventBus, vfxStorage, this);
            vibrationHandler = new VibrationHandler(eventBus, this, baseVibrationTimeInMilliseconds, gapBetweenVibrationsOnMoneySpend);
        }
        
        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(1);
            sceneChanger.LoadGameSceneAndLoadData();
        }
    }
}