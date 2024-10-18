using System;
using Project.Scripts.LocationHolder;
using UnityEngine;

public class GarbageCanView : MonoBehaviour
{
    [Header("Interactable Item")]
    [SerializeField] private Transform itemInstantiatePosition;
    [SerializeField] private GameObject garbagePrefab;
    [SerializeField] private InteractionField interactionField;
    
    private TrashCan can;
    private GameObject interactableItem;

    public bool IsClear => can.IsClear;
    
    public event Action<InteractionField> OnGarbageSpawnEvent;
    public event Action<InteractionField> OnGarbageCleanEvent;
    
    private ILocationHoldersRoot root;

    private void Awake()
    {
        can = new TrashCan(this, 50,70);
    }

    private void OnEnable()
    {
        interactionField.IsFinishFilling += can.Clear;
        can.OnTrashCanFillEvent += FillCan;
        can.OnTrashCanClearEvent += ClearCan;
    }

    private void OnDisable()
    {
        interactionField.IsFinishFilling -= can.Clear;
        can.OnTrashCanFillEvent -= FillCan;
        can.OnTrashCanClearEvent -= ClearCan;
    }

    private void FillCan()
    {
        interactableItem = Instantiate(garbagePrefab, itemInstantiatePosition);
        interactableItem.transform.position = itemInstantiatePosition.position;
        ActiveAndDeactive(true);
    }

    private void ClearCan()
    {
        ActiveAndDeactive(false);
        Destroy(interactableItem);
    }

    private void ActiveAndDeactive(bool isActive)
    {
        interactionField.gameObject.SetActive(isActive);
        if(isActive)
            OnGarbageSpawnEvent?.Invoke(interactionField);
        else
            OnGarbageCleanEvent?.Invoke(interactionField);
    }
}
