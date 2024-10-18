using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.InteractiveBuildings;

public abstract class InteractiveBuilding : MonoBehaviour, IInteractiveBuilding
{
    // Длина очереди, вместимость, скорость работы, стоимость билета, частота вмешательства
    [Header("BUILDING iNFO")]
    [SerializeField] protected BuildingType buildingType; //тип интерактивного объекта
    [SerializeField] protected MoneyContainer moneyContainer;
    [Header("UPGRADE BUILDING")]
    [SerializeField] protected bool isActivate;
    [SerializeField] protected GameObject[] upgradeObjects;
    [Header("Queue")]
    [SerializeField] protected Transform[] queuePoints; // Точки очереди

    public bool isOccupied { get; protected set; }
    public event Action<IInteractiveBuilding> OnOccupiedEvent;
    public event Action<IInteractiveBuilding, Vector3> OnUnoccupiedEvent;
    public event Action<IInteractiveBuilding, Vector3> OnInterventionFrequencyEndEvent;
    public event Action<IInteractiveBuilding> OnInterventionFrequencyRestoredEvent;
    public BuildingType BuildingType => buildingType;
    public string Name => gameObject.name;
    public bool IsClean => clean;

    protected List<Client> queueReserved; // Лист в который попадаю клиенты которые идут к атракциону
    protected Queue<Client> currentQueue; // Лист в который попадают клиенты которые встали в очередь
    protected float interventionFrequency; // частота вмешательства.
    protected Client currentClient;
    protected MoneySpawner moneySpawner;
    protected int capacity; // вместимость очереди.
    protected int amountVisit;
    protected Collider mainCollider;

    protected UIUpgradeFactory upgradeFactory;
    protected Characteristics characteristics;
    protected IEventBus bus;
    protected bool clean;

    public Characteristics Characteristics => characteristics;
    public virtual InteractionField RemoveResourceField { get; }

    protected abstract void Work();
    protected abstract void StopWork();
    public abstract void StartWorking();
    public abstract void Upgrade();
    protected abstract IEnumerator WorkCoroutine();
    protected abstract void InterventionFrequencyEnded();

    public void Config(MoneySpawner moneySpawner, UIUpgradeFactory upgradeFactory, Characteristics characteristics, IEventBus bus)
    {
        this.moneySpawner = moneySpawner;
        this.upgradeFactory = upgradeFactory;
        this.characteristics = characteristics;
        this.bus = bus;
        queueReserved = new List<Client>();
        currentQueue = new Queue<Client>();
        StartCoroutine(StartWorkWithDelay());
    }

    private IEnumerator StartWorkWithDelay()
    {
        yield return new WaitForSeconds(5f);
        StartWorking();
    }

    protected virtual void Awake()
    {
        mainCollider = GetComponent<Collider>();
        capacity = queuePoints.Length;
    }

    protected void ChangeInterventionFrequencyByVal(int val)
    {
        if (val > 0)
        {
            OnInterventionFrequencyRestoredEvent?.Invoke(this);
            clean = true;
        }
        interventionFrequency += val;
        if (interventionFrequency < 1)
        {
            interventionFrequency = 0;
            clean = false;
            InterventionFrequencyEnded();
        }
    }

    protected void CollectIncome(int ticketCost)
    {
        var transformContainer = moneyContainer.GetPoint();

        if (transformContainer == null)
        {
            var money = moneyContainer.GetFirstMoney();
            money.SetAmountMoney(money.AmountMoney + ticketCost);
        }
        else
        {
            moneySpawner.GetMoney(transformContainer).SetAmountMoney(ticketCost);

        }

    }

    public bool TryCheckPlaceInQueue()
    {
        return queueReserved.Count < capacity;
    }

    public virtual Vector3 ToGetInQueue(Client client)
    {
        queueReserved.Add(client);
        var index = queueReserved.IndexOf(client);
        return queuePoints[index].position;
    }

    protected void SortQueue()
    {
        for (int i = 0; i < queueReserved.Count; i++)
        {
            queueReserved[i].Move(queuePoints[i].position);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (queueReserved.Count < 1)
            return;
        if (queueReserved[0].Equals(other.GetComponent<Client>()))
        {
            currentClient = queueReserved[0];
            mainCollider.enabled = false;
        }
    }

    protected void OnInterventionFrequencyEndEventInvoke(Vector3 pos)
    {
        OnInterventionFrequencyEndEvent?.Invoke(this, pos);
    }

    protected void OnOccupiedEventInvoke()
    {
        OnOccupiedEvent?.Invoke(this);
    }

    protected void OnUnoccupiedEventInvoke(Vector3 pos)
    {
        OnUnoccupiedEvent?.Invoke(this, pos);
    }
}
