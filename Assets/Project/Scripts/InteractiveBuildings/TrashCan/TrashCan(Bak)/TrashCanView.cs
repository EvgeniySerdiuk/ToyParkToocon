using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Project.Scripts.InteractiveBuildings;
using Project.Scripts.LocationHolder;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class TrashCanView : MonoBehaviour, IInteractiveBuilding
{
    [SerializeField] private InteractionField interactionField;
    [SerializeField] private int locationId = 1;
    [SerializeField] private GarbageCanView garbageCan;
    
    [Space]
    [Header("Item info")]
    [SerializeField] private int amountMoney;
    [SerializeField] private Transform[] spawnMoneyPoints;

    public event Action<IInteractiveBuilding> OnOccupiedEvent;
    public event Action<IInteractiveBuilding, Vector3> OnUnoccupiedEvent;
    public event Action<IInteractiveBuilding, Vector3> OnInterventionFrequencyEndEvent;
    public event Action<IInteractiveBuilding> OnInterventionFrequencyRestoredEvent;
    public BuildingType BuildingType => BuildingType.TrashCan;
    public string Name => gameObject.name;
    public bool IsClean => garbageCan.IsClear;
    public InteractionField RemoveResourceField => interactionField;

    private MoneySpawner moneySpawner;
    private ILocationHoldersRoot root;
    private bool clean;
    
    [Inject]
    private void Init(MoneySpawner spawner, ILocationHoldersRoot root)
    {
        moneySpawner = spawner;
        this.root = root;
    }

    private void Start()
    {
        StartCoroutine(DelayRegister());
    }

    private IEnumerator DelayRegister()
    {
        yield return new WaitForSeconds(1f);
        root.GetLocationHolder(locationId).GetInteractiveBuildingRegister().Register(this, BuildingType.TrashCan);
        if(!garbageCan.IsClear)
            OnUnoccupiedEvent?.Invoke(this, interactionField.transform.position);
    }

    public void StartWorking()
    {
    }
    
    private void OnEnable()
    {
        interactionField.IsFinishFilling += CreateMoney;
        interactionField.IsFinishFilling += OnGarbageClear;
        garbageCan.OnGarbageSpawnEvent += OnGarbageSpawn;
        garbageCan.OnGarbageCleanEvent += OnGarbageGetEvent;
    }

    private void OnDisable()
    {
        interactionField.IsFinishFilling -= CreateMoney;
        interactionField.IsFinishFilling -= OnGarbageClear;
        garbageCan.OnGarbageSpawnEvent -= OnGarbageSpawn;
        garbageCan.OnGarbageCleanEvent += OnGarbageGetEvent;
    }

    private void CreateMoney()
    {
        for (int i = 0; i < amountMoney; i++)
        {
            moneySpawner.GetMoney(spawnMoneyPoints[Random.Range(0, spawnMoneyPoints.Length)]);
        }
    }

    private void OnGarbageSpawn(InteractionField field)
    {
        clean = false;
        OnUnoccupiedEvent?.Invoke(this, interactionField.transform.position);
    }

    private void OnGarbageClear()
    {
        CleanEvents();
    }

    private void OnGarbageGetEvent(InteractionField garbageField)
    {
        CleanEvents();
    }

    private void CleanEvents()
    {
        //если не появилось нового мусора
        if (!clean && garbageCan.IsClear)
        {
            clean = true;
            OnOccupiedEvent?.Invoke(this);
        }
    }
    
    #region DontNeedToUse
    public Characteristics Characteristics => null;

    public void Upgrade()
    {
    }
    public bool TryCheckPlaceInQueue()
    {
        return false;
    }
    public Vector3 ToGetInQueue(Client client) 
    { 
        return Vector3.zero;
    }    
    public JToken CaptureAsJToken() 
    { 
        return null;
    }

    public void RestoreFromJToken(JToken state)
    {
    }
    #endregion
}
