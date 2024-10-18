using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Animations;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform inventoryView;
    public bool IsFullInventory { get { return CheckInventoryCapacity(); } }
    public bool IsEmptyInventory { get { return CheckCountSlots(); } }

    private CharacterCharacteristics characterCharacteristics;
    protected List<GameObject> inventorySlots = new List<GameObject>();
    protected Animator animator;
    protected Characteristic inventoryCapacity;
    protected Dictionary<GameObject, Vector3> itemsLocalPosition;

    private IEventBus bus;
    protected bool isPlayerInventory;
    protected PlayerTag player;
    
    [Inject]
    private void Init(IEventBus bus)
    {
        this.bus = bus;
        if (TryGetComponent<PlayerTag>(out var playerTag))
        {
            isPlayerInventory = true;
            player = playerTag;
        }
    }
    
    protected virtual void Start()
    {
        itemsLocalPosition = new Dictionary<GameObject, Vector3>();
        animator = GetComponentInChildren<Animator>();
        characterCharacteristics = GetComponent<CharacterCharacteristics>();
        inventoryCapacity = characterCharacteristics.Characteristics.GetCharacteristic(CharacteristicType.InventoryCapacityWorker);
    }

    public void AddItem(GameObject item)
    {
        if (CheckInventoryCapacity()) 
            return;
        Vector3 cachedPosition = item.transform.position;
        animator.SetLayerWeight(1, 1);
        item.transform.SetParent(inventoryView,false);
        item.transform.position = cachedPosition;
        Vector3 position = GetPosition();
        MoveItemToPosition(item, position);
        Add(item, position);
        DoNeedEvent();
    }

    protected virtual void Add(GameObject item, Vector3 position)
    {
        inventorySlots.Add(item);
        itemsLocalPosition.Add(item, position);
    }
    
    private Vector3 GetPosition()
    {
        if (inventorySlots.Count == 0)
            return Vector3.zero;
        
        var lastObj = inventorySlots.Last();
        var position = itemsLocalPosition[lastObj];
        position.y += lastObj.GetComponent<MeshRenderer>().bounds.size.y + 0.01f;
        return position;
    }
    
    private void MoveItemToPosition(GameObject item, Vector3 position)
    {
        if (item.TryGetComponent<BaseAnimatedItem>(out var animatedItem))
        {
            animatedItem.ParabolaAnimate(position);
        }
    }

    protected virtual bool CheckInventoryCapacity()
    {
        return inventorySlots.Count >= inventoryCapacity.Value;
    }

    public GameObject RemoveItem() 
    {
        if (CheckCountSlots()) 
            return null;
        DoNeedEvent();
        var obj = inventorySlots.Last();
        inventorySlots.Remove(obj);
        itemsLocalPosition.Remove(obj);
        return obj;
    }

    public GameObject RemoveItem(GameObject item)
    {
        if (CheckCountSlots()) 
            return null;

        var obj = inventorySlots.LastOrDefault(x => x.CompareTag(item.tag));

        if(obj == null)
        {
            return null;
        }

        DoNeedEvent();
        int index = inventorySlots.IndexOf(obj);
        inventorySlots.Remove(obj);
        SortItem(index);
        return obj;
    }

    public virtual bool CheckCountSlots()
    {
        if (inventorySlots.Count < 1) 
            animator.SetLayerWeight(1, 0);

        return inventorySlots.Count < 1;
    }

    private void SortItem(int index)
    {
        if(index == 0) 
            index = 1;
        for (int i = index; i < inventorySlots.Count; i++)
        {
            var newPos = inventorySlots[i - 1].transform.position;
            newPos.y += inventorySlots[i - 1].GetComponent<MeshRenderer>().bounds.size.y + 0.01f;
            inventorySlots[i].transform.position = newPos;
        }
    }

    public bool CheckItem(GameObject item)
    {
        if (inventorySlots.Count < 1) 
        { 
            animator.SetLayerWeight(1, 0); 
            return false; 
        }

        return inventorySlots.Any(x => x.CompareTag(item.tag));
    }

    private void DoNeedEvent()
    {
        if(!isPlayerInventory)
            return;
        bus.Raise(new VibrateEvent());
        player.PlayDropSound();
    }
}
