using System;
using UnityEngine;

[Serializable]
public class BuyLocationTask : Task
{
    [SerializeField] private BuyLocation buyLocationField;
    public override Vector3 ObjectPoint => buyLocationField.transform.position;

    public override void StartTask()
    {
        buyLocationField.LocationBuying += Complete;
        if (buyLocationField.LocationBought)
        {
            Complete();
        }
    }

    public override void Complete()
    {
        buyLocationField.LocationBuying -= Complete;
        base.Complete();
    }
}
