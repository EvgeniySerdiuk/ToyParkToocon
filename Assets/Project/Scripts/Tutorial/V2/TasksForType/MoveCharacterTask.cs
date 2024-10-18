using System;
using UnityEngine;

[Serializable]
public class MoveCharacterTask : Task
{
    [SerializeField] private MoveTaskAnim character;
    [SerializeField] private GameObject taskView;
    public override Vector3 ObjectPoint => Vector3.zero;

    public override void StartTask()
    {
        taskView.SetActive(true);
        character.AnimCompliete += Complete;
    }

    public override void Complete()
    {
        character.AnimCompliete -= Complete;
        GameObject.Destroy(taskView);
        base.Complete();
    }
}
