using System;
using System.Collections;
using System.Linq;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Attractions.Animators;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Attraction : InteractiveBuilding
{
    [SerializeField] private Transform finishPosition;
    [SerializeField] private GameObject[] peopleInAttraction;
    [SerializeField] private TextMeshProUGUI ticketsCounter;

    [SerializeField] private InteractionField ticketsField;
    [SerializeField] private CasherField workField;

    private AttractionAnimatorManager animator;
    private SpawnClients spawnClients;

    private int capacityAttraction;
    private int ticketCost;
    private int workSpeed;

    public override InteractionField RemoveResourceField => ticketsField;

    public void SetSpawnClietns(SpawnClients spawnClients)
    {
        this.spawnClients = spawnClients;
    }

    protected override void Awake()
    {
        base.Awake();
        animator = new AttractionAnimatorManager(GetComponentInChildren<Animator>());
    }

    private void Start()
    {
        SubscriptionEvent();
        characteristics.Init();
        Construct();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(isOccupied == false && interventionFrequency > 0)
        {
          workField.SetActive(true);
        }
    }

    public override void StartWorking()
    {
        if (queueReserved.Count < queuePoints.Length) 
            SpawnClients();
        InterventionFrequencyEnded();
    }

    private void SubscriptionEvent()
    {
        ticketsField.IsFinishFilling += AddTickets;
        workField.IsFinishFilling += Work;
        characteristics.GetCharacteristic(CharacteristicType.CapacityBuilding).changeValue += ChangeViewForUpgrade;
        characteristics.GetCharacteristic(CharacteristicType.WorkSpeedBuilding).changeValue += ChangeValueCharacteristic;
        characteristics.GetCharacteristic(CharacteristicType.TicketCost).changeValue += ChangeValueCharacteristic;
    }
    
    private void Construct()
    {
        isOccupied = false;
        mainCollider.enabled = true;
        workField.gameObject.SetActive(true);
        ticketsCounter.text = interventionFrequency.ToString();
        capacityAttraction = (int) characteristics.Value(CharacteristicType.CapacityBuilding);
        ticketCost = (int) characteristics.Value(CharacteristicType.TicketCost);
        workSpeed = (int) characteristics.Value(CharacteristicType.WorkSpeedBuilding);
    }

    protected override void Work()
    {
        if (currentClient == null || isOccupied) return;
        if(workField.IsPlayerOnField)
            bus.Raise(new VibrateEvent());  
        StopAllCoroutines();
        queueReserved.Remove(currentClient);
        currentQueue.Enqueue(currentClient);
        currentClient.gameObject.SetActive(false);
        currentClient = null;

        SortQueue();
        RemoveTicket();
        CollectIncome(ticketCost);
        EnablePeopleInAttraction(true);
        workField.SetActive(false);
        StartCoroutine(StartWork());
        mainCollider.enabled = true;
        spawnClients.SpawnClientsInLocation(1);
    }

    private IEnumerator StartWork()
    {
        if (currentQueue.Count < capacityAttraction)
        {
            yield return new WaitForSeconds(7);
        }

        isOccupied = true;
        workField.SetActive(false);
        StartCoroutine(WorkCoroutine());
        animator.SetPerformanceAnimation();
        OnOccupiedEventInvoke();
        AnimationClient();
    }

    protected override void StopWork()
    {
        StartCoroutine(SpawnClientFinish());
        animator.SetIdleAnimation();
    }

    protected override IEnumerator WorkCoroutine()
    {
        yield return new WaitForSeconds(workSpeed);
        StopWork();
    }

    protected override void InterventionFrequencyEnded()
    {
        workField.SetActive(false);
        OnInterventionFrequencyEndEventInvoke(ticketsField.transform.position);
    }

    public override void Upgrade()
    {
        upgradeFactory.GetUIUpgrade(characteristics);
    }

    private void ChangeViewForUpgrade()
    {
        upgradeObjects.First(x => x.activeSelf == !isActivate).gameObject.SetActive(isActivate);
        ChangeValueCharacteristic();
    }

    private void ChangeValueCharacteristic()
    {
        capacityAttraction = (int)characteristics.Value(CharacteristicType.CapacityBuilding);
        ticketCost = (int)characteristics.Value(CharacteristicType.TicketCost);
        workSpeed = (int)characteristics.Value(CharacteristicType.WorkSpeedBuilding);
    }

    private void AnimationClient()
    {
        foreach (var people in peopleInAttraction)
        {
            if (people.activeSelf)
            {
                people.GetComponent<Animator>().SetTrigger("Perform");
            }
        }
    }

    private void AddTickets()
    {
        ChangeInterventionFrequencyByVal(10);
        if (interventionFrequency > 0 && isOccupied == false) 
            workField.SetActive(true);
        OnUnoccupiedEventInvoke(workField.transform.position);
        ticketsCounter.text = interventionFrequency.ToString();
    }

    private void RemoveTicket()
    {
        amountVisit++;
        ChangeInterventionFrequencyByVal(-1);
        ticketsCounter.text = interventionFrequency.ToString();
    }

    private void EnablePeopleInAttraction(bool value)
    {
        var people = peopleInAttraction.FirstOrDefault(x => x.activeSelf == !value);
        if (people != null) people.SetActive(value);
    }

    private void SpawnClients()
    {
        spawnClients.SpawnClientsInLocation(capacity);
    }

    private IEnumerator SpawnClientFinish()
    {
        var amountClients  = currentQueue.Count;

        for (int i = 0; i < amountClients; i++)
        {
            if (currentQueue.TryDequeue(out var client))
            {
                amountVisit--;
                EnablePeopleInAttraction(false);
                client.transform.position = finishPosition.position;
                client.gameObject.SetActive(true);
                client.MoveNextBuilding();
                yield return new WaitForSeconds(1f);
            }
        }

        isOccupied = false;
        //if we have tickets and building is not working
        if (interventionFrequency > 0)
        {
            OnUnoccupiedEventInvoke(workField.transform.position);
            workField.SetActive(true);
        }
 
        SortQueue();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}