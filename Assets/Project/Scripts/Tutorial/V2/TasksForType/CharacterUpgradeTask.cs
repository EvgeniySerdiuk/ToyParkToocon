using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterUpgradeTask : Task
{
    [SerializeField] private CharacterUpgradeField fieldUpgrade;
    public override Vector3 ObjectPoint => fieldUpgrade.transform.position;

    public override void Complete()
    {
        fieldUpgrade.UpgradableObj.Characteristics.UpgradableCharacteristics.ToList().ForEach(characteristic => characteristic.changeValue -= Complete);
        base.Complete();
    }

    public override void StartTask()
    {
        fieldUpgrade.UpgradableObj.Characteristics.UpgradableCharacteristics.ToList().ForEach(characteristic => characteristic.changeValue += Complete);
    }
}
