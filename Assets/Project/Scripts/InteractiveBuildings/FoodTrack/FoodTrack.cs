using System;
using System.Collections;
using System.Linq;
using Project.Scripts.Arch.EventBus.Events;
using TMPro;
using UnityEngine;

public class FoodTrack : InteractiveBuilding
{
    [SerializeField] private TextMeshProUGUI foodCounter;

    [Header("FoodDealler")]
    [SerializeField] private InteractionField foodRemoveField;
    [SerializeField] private CasherField workField;

    public override InteractionField RemoveResourceField => foodRemoveField;

    private void Start()
    {
        foodRemoveField.IsFinishFilling += AddFoodToTrack;
        foodCounter.text = interventionFrequency.ToString();
        workField.gameObject.SetActive(true);
        workField.IsFinishFilling += Work;
        characteristics.GetCharacteristic(CharacteristicType.TicketCost).changeValue += ChangeValueCharacteristic;
        characteristics.Init();
    }

    public override void StartWorking()
    {             
        OnUnoccupiedEventInvoke(workField.gameObject.transform.position);
        InterventionFrequencyEnded();
        //characteristics.Init();
    }
    
    protected override void InterventionFrequencyEnded()
    {
        workField.SetActive(false);
        mainCollider.enabled = false;
        OnInterventionFrequencyEndEventInvoke(foodRemoveField.transform.position);
    }
    
    protected override void Work()
    {
        if (currentClient == null || isOccupied) return;
        if(workField.IsPlayerOnField)
            bus.Raise(new VibrateEvent());
        OnOccupiedEventInvoke();
        StopWork();
    }

    public override void Upgrade()
    {
        upgradeFactory.GetUIUpgrade(characteristics);
    }

    private void ChangeValueCharacteristic()
    {
        try
        {
            var first = upgradeObjects.First(x => x.activeSelf == !isActivate);
            first.gameObject.SetActive(isActivate);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(currentClient != null) { workField.SetActive(true); }
    }

    protected override void StopWork()
    {
        queueReserved.Remove(currentClient);
        currentClient.MoveNextBuilding();
        currentClient = null;
        ChangeInterventionFrequencyByVal(-1);
        foodCounter.text = interventionFrequency.ToString();
        CollectIncome((int)characteristics.Value(CharacteristicType.TicketCost));

        if (queueReserved.Count > 0)
        {
            SortQueue();
        }

        workField.SetActive(false);
        if (interventionFrequency > 0)
        {
          mainCollider.enabled = true;
        }
    }

    private void AddFoodToTrack()
    {
        ChangeInterventionFrequencyByVal(10);
        foodCounter.text = interventionFrequency.ToString();
        if (interventionFrequency > 0)
        {
            mainCollider.enabled = true;
            if (currentClient == null) 
                return;
            workField.SetActive(true);
        }
    }

    protected override IEnumerator WorkCoroutine()
    {
        yield return null;
    }
}

