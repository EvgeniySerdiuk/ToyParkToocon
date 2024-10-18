using UnityEngine;

public class MultyLocationReceiveField : ReceiveField
{
    [SerializeField] private int[] locationsId;
    
    protected override void Awake()
    {
        for (int i = 0; i < locationsId.Length; i++)
        {
            locationId = locationsId[i];
            base.Awake();
        }
    }
}