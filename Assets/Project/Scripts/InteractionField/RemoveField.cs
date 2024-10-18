using System.Collections;
using Project.Scripts.Animations;
using Project.Scripts.LocationHolder;
using UnityEngine;

public class RemoveField: ReceiveField
{
    private bool haveItem;
    private Collider thisCollider;

    private GameObject InteractableObject => interactableObject.gameObject;
    
    protected override void Init(ILocationHoldersRoot root, ItemParabola parabola)
    {
        //тут была логика, которая должна работать только в ресив филде, но не в ремув филде
    }

    protected override void Awake()
    {
        //тут была логика, которая должна работать только в ресив филде, но не в ремув филде
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (fieldIsOccupied) return;
        inventory = other.GetComponent<Inventory>();

        if (interactableObject != null)
        {
            ActivateField();

            if (inventory.CheckItem(InteractableObject) == false)
            {
                fieldIsOccupied = false;
                return;
            }
        }

        fieldIsOccupied = true;
        StartCoroutine(TestStey(other));
        haveItem = true;
        if (thisCollider == null) 
            thisCollider = GetComponent<Collider>();           
    }

    protected override IEnumerator TestStey(Collider other)
    {
        while (haveItem == true)
        {
            currentTime += Time.deltaTime;

            if (currentTime> 0.2f)
            {
                UseItem(other);
                FinishFiling();
                haveItem = inventory.CheckItem(InteractableObject);
            }

            yield return new WaitForEndOfFrame();
        }

        fieldIsOccupied = false;
        haveItem = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    protected override void FinishFiling()
    {
        base.FinishFiling();

        if (inventory.CheckItem(InteractableObject) == false)
        {
            thisCollider.enabled = false;
            haveItem = false;
            fieldIsOccupied = false;
            StopAllCoroutines();
            thisCollider.enabled = true;
        }
    }

    protected override void UseItem(Collider other)
    {
        GameObject obj;

        if (interactableObject == null)
        {
             obj = inventory.RemoveItem();
        }
        else
        {
             haveItem = inventory.CheckItem(InteractableObject);
             obj = inventory.RemoveItem(InteractableObject);
        }

        if (obj != null)
        {
            if (obj.TryGetComponent<BaseAnimatedItem>(out var animatedItem))
            {
                animatedItem.transform.SetParent(transform);
                animatedItem.ParabolaAnimate(spawnPoint.position,0, callback: () => Destroy(animatedItem.gameObject));
            }
            else
                Destroy(obj);
        }

        InventoryFull = CheckInventory(inventory);
        haveItem = inventory.CheckItem(InteractableObject);

        if (InventoryFull && alwaysShowTimer == false )
        {
            timerView.SetActive(false);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Inventory>().Equals(inventory))
        {
            fieldIsOccupied = false;
            StopAllCoroutines();
            ResetProgressBar();          
        }
    }

    protected override bool CheckInventory(Inventory inventory)
    {
        return inventory.IsEmptyInventory;
    }  
}
