using Project.Scripts.Arch.Workers;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWorker : Inventory
{
    public bool IsEmpty {get { return inventorySlots.Count <= 0;} }

    public event Action OnInventoryFullEvent;
    public event Action OnInventoryEmptyEvent;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        itemsLocalPosition = new Dictionary<GameObject, Vector3>();
        inventoryCapacity = GetComponent<BaseWorker>().Characteristics.GetCharacteristic(CharacteristicType.InventoryCapacityWorker);
    }

    protected override void Add(GameObject item, Vector3 position)
    {
        base.Add(item, position);
        if(CheckInventoryCapacity())
            OnInventoryFullEvent?.Invoke();
    }

    protected override bool CheckInventoryCapacity()
    {
        float capacity = 1;
        if(inventoryCapacity != null && inventoryCapacity.Value != null)
            capacity = inventoryCapacity.Value;
        return inventorySlots.Count >= (int) capacity;
    }

    public override bool CheckCountSlots()
    {
        if(inventorySlots.Count < 1)
        {
            OnInventoryEmptyEvent?.Invoke();
            return true;
        }

        return false;
    }
}
