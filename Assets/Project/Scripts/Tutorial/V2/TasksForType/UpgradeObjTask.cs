using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class UpgradeObjTask : Task
{
    [SerializeField] private FieldByuUpgrade fieldUpgrade;
    public override Vector3 ObjectPoint => fieldUpgrade.transform.position;

    public override void Complete()
    {
        fieldUpgrade.UpgradableObj.Characteristics.UpgradableCharacteristics.ToList().ForEach(characteristic => characteristic.changeValue -= Complete);
        base.Complete();
    }

    public override void StartTask()
    {
        if(fieldUpgrade == null)
        {
            DelayForNextTask = 0;
            base.Complete();
            return;
        }

        fieldUpgrade.UpgradableObj.Characteristics.UpgradableCharacteristics.ToList().ForEach(characteristic => characteristic.changeValue += Complete);
    }
}
