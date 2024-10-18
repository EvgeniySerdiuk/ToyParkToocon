using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BuyObjTask : Task
{
    [SerializeField] private FieldByuUpgrade fieldBuy;
    public FieldByuUpgrade FieldBuy => fieldBuy;
    
    public override Vector3 ObjectPoint => fieldBuy == null ? GetPoingInFrontOfCamera() : fieldBuy.transform.position;

    public override void StartTask()
    {
        if(fieldBuy.UpgradableObj == null)
        {
            fieldBuy.InstantiateObj += Complete;
        }
        else
        {
            DelayForNextTask = 0;
            base.Complete();
        }
    }

    public override void Complete()
    {
        fieldBuy.InstantiateObj -= Complete;
        base.Complete();
    }

    private Vector3 GetPoingInFrontOfCamera()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1.0f));
    }
}
