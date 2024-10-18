using Project.Scripts.Arch.Building;
using System;
using UnityEngine;

[Serializable]
public class TakeResourceTask : Task
{
    [SerializeField] private ResourceType takeResourceType;
    [SerializeField] private LocationHolderr locationHolder;

    private InteractionField resourceField;

    public override Vector3 ObjectPoint => resourceField.transform.position;

    public override void Complete()
    {
        resourceField.IsFinishFilling -= Complete;
        base.Complete();
    }

    public override void StartTask()
    {
        resourceField = locationHolder.GetResourceFieldRegister().GetFieldOfResourceType(takeResourceType);
        resourceField.IsFinishFilling += Complete;
    }
}
