using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Toilet : InteractiveBuilding
{
    [SerializeField] private InteractionField interactionField;
    [SerializeField] private Transform workPoint;

    public event Action<InteractionField> IsNeedClean;
    public event Action<InteractionField> ISClean;
    public bool NeedClean;
    public InteractionField InteractionField => interactionField;
    public override InteractionField RemoveResourceField => interactionField;

    protected override void Awake()
    {
        base.Awake();
        isOccupied = false;
        mainCollider.enabled = true;
        amountVisit = (int)interventionFrequency;
    }

    private void Start()
    {
        characteristics.GetCharacteristic(CharacteristicType.PollutionFrequency).changeValue += ChangeValueCharacteristic;
        characteristics.Init();
        CleanToilet();
    }

    public override void StartWorking()
    {
        //CleanToilet();
    }

    public override void Upgrade()
    {
        upgradeFactory.GetUIUpgrade(characteristics);
    }

    private void ChangeValueCharacteristic()
    {
        upgradeObjects.First(x => x.activeSelf == !isActivate).gameObject.SetActive(isActivate);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Work();
    }

    protected override void Work()
    {
        if (currentClient == null || isOccupied) return;
        isOccupied = true;
        currentClient.Move(workPoint.position);
        queueReserved.Remove(currentClient);
        SortQueue();
        StartCoroutine(WorkCoroutine());
    }

    protected override void StopWork()
    {
        currentClient.MoveNextBuilding();
        CollectIncome((int)characteristics.Value(CharacteristicType.TicketCost));
        amountVisit++;
        currentClient = null;
        if (amountVisit >= characteristics.Value(CharacteristicType.PollutionFrequency))
        {
            CleanToilet(); 
            return;
        }
        
        isOccupied = false;
        mainCollider.enabled = true;
    }

    public void CleanToilet()
    {
        interactionField.IsFinishFilling += IsClean;
        NeedClean = true;
        IsNeedClean?.Invoke(interactionField);
        SetToiletState(true);
    }

    public void IsClean()
    {
        interactionField.IsFinishFilling -= IsClean;
        NeedClean = false;
        ISClean?.Invoke(interactionField);
        SetToiletState(false);
        isOccupied = false;
        mainCollider.enabled = true;
        amountVisit = 0;
    }

    private void SetToiletState(bool isActive)
    {
        if(isActive)
            OnUnoccupiedEventInvoke(interactionField.transform.position);
        else
            OnOccupiedEventInvoke();
        interactionField.gameObject.SetActive(isActive);
        mainCollider.enabled = !isActive;
        clean = isActive;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected override IEnumerator WorkCoroutine()
    {
        yield return new WaitForSeconds(characteristics.Value(CharacteristicType.WorkSpeedBuilding));
        StopWork();
    }
    
    protected override void InterventionFrequencyEnded()
    {
    }
}
