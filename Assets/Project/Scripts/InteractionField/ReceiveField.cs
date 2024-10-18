using System;
using System.Collections;
using Project.Scripts.Animations;
using Project.Scripts.Arch.Building;
using Project.Scripts.InteractiveBuildings.ResourceFieldsRegister;
using Project.Scripts.LocationHolder;
using UnityEngine;
using Zenject;

public class ReceiveField : InteractionField
{
    [SerializeField] protected GameObject timerView;
    [SerializeField] protected bool alwaysShowTimer;
    [SerializeField] protected Transform spawnPoint;
    [Header("Object")] 
    [SerializeField] protected int locationId;
    [SerializeField] private ResourceType resourceType;
    [SerializeField] protected BaseAnimatedItem interactableObject;
    [SerializeField] protected AudioSource audioSource;
    
    protected bool InventoryFull;
    protected bool fieldIsOccupied;

    protected ILocationHoldersRoot root;
    private ItemParabola parabola;
    protected event Action isFinish;
    
    //Recieve филды должны регистрироваться в локейшн холдер руте для работы грузчиков
    [Inject]
    protected virtual void Init(ILocationHoldersRoot root, ItemParabola parabola)
    {
        this.root = root;
        this.parabola = parabola;
    }

    //нужен поздний лаун в Awake потому что все ILocationHolder`ы регестрируются в [Inject]
    protected virtual void Awake()
    {
        ILocationHolder holder = root.GetLocationHolder(locationId);
        ResourceFieldRegister fieldRegister = holder.GetResourceFieldRegister();
        fieldRegister.RegisterResourceRecieveField(this, resourceType);
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (fieldIsOccupied) return;
        base.OnTriggerEnter(other);
        InventoryFull = CheckInventory(inventory);

        if (InventoryFull == false)
        {
            fieldIsOccupied = true;
            timerView.SetActive(true);
            StartCoroutine(TestStey(other)); 
        }
    }

    protected virtual void OnEnable()
    {
        inventory = null;
        fieldIsOccupied = false;
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }
    
    protected virtual void OnTriggerStay(Collider other)
    {
        if (fieldIsOccupied) return;
        inventory = other.GetComponent<Inventory>();
        InventoryFull = CheckInventory(inventory);

        if(InventoryFull == false)
        {
            ActivateField();
            fieldIsOccupied = true;
            timerView.SetActive(true);
            StartCoroutine(TestStey(other));
        }
    }

    protected virtual IEnumerator TestStey(Collider other)
    {
        while (InventoryFull == false) 
        {
            currentTime += Time.deltaTime;

            if (currentTime> 0.2f)
            {
                UseItem(other);
                FinishFiling();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual void UseItem(Collider other)
    {
        var item = CreateItem();
        inventory.AddItem(item.gameObject);
        InventoryFull = CheckInventory(inventory);
        
        if (InventoryFull == true)
        {
            timerView.SetActive(false);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        ResetProgressBar();
        inventory = null;
        fieldIsOccupied = false;

        if (alwaysShowTimer == false)
        {
            timerView.SetActive(false);
        }

        StopAllCoroutines();
    }

    protected override void FinishFiling()
    {
        ResetProgressBar();
        if (IsPlayerOnField && player != null)
        {
            player.PlaySound(audioSource.clip); 
            player = null;
        }
            
        InvokeFinishFilling();
    }

    protected virtual bool CheckInventory(Inventory inventory)
    {
        return inventory.IsFullInventory;
    }

    private GameObject CreateItem()
    {
        var animatedObject = Instantiate(interactableObject, transform);
        animatedObject.Config(parabola);
        Transform t = animatedObject.transform;
        t.parent = spawnPoint;
        t.localPosition = Vector3.zero;
        t.rotation = new Quaternion(0f,0f,0f, 0f);
        t.parent = null;
        
        return animatedObject.gameObject;
    }
}
