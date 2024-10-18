using NaughtyAttributes;
using System;
using UnityEngine;

[Serializable]
public class TutorialTask
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public TaskType taskType { get; private set; }

    [AllowNesting, ShowIf("taskType", TaskType.Buy),                 SerializeField] private BuyObjTask buyObjTask;
    [AllowNesting, ShowIf("taskType", TaskType.Upgrade),             SerializeField] private UpgradeObjTask upgradeObjTask;
    [AllowNesting, ShowIf("taskType", TaskType.TakeResource),        SerializeField] private TakeResourceTask takeResourceTask;
    [AllowNesting, ShowIf("taskType", TaskType.RemoveResource),      SerializeField] private RemoveResourceTask removeResourceTask;
    [AllowNesting, ShowIf("taskType", TaskType.ActivateWork),        SerializeField] private ActivateWorkTask activateWorkTask;
    [AllowNesting, ShowIf("taskType", TaskType.CleanToilet),         SerializeField] private RemoveResourceTask cleanToiletTask;
    [AllowNesting, ShowIf("taskType", TaskType.CharacterUpgrade),    SerializeField] private CharacterUpgradeTask characterUpgradeTask;
    [AllowNesting, ShowIf("taskType", TaskType.BuyLocation),         SerializeField] private BuyLocationTask buyLocationTask;
    [AllowNesting, ShowIf("taskType", TaskType.SavableMoney),        SerializeField] private SavingMoneyTask savingMoneyTask;
    [AllowNesting, ShowIf("taskType", TaskType.MoveCharacter),       SerializeField] private MoveCharacterTask moveCharacterTask;
    public Task Task { get; private set; }

    public void Init()
    {
        switch (taskType)
        {
            case TaskType.Buy:
                Task = buyObjTask;
                break;
            case TaskType.Upgrade:
                Task = upgradeObjTask;
                break;
            case TaskType.TakeResource:
                Task = takeResourceTask;
                break;
            case TaskType.CleanToilet:
                Task = cleanToiletTask;
                break;
            case TaskType.RemoveResource:
                Task = removeResourceTask;
                break;
            case TaskType.ActivateWork:
                Task = activateWorkTask;
                break;
            case TaskType.CharacterUpgrade:
                Task = characterUpgradeTask;
                break;
            case TaskType.BuyLocation:
                Task = buyLocationTask;
                break;
            case TaskType.SavableMoney:
                Task = savingMoneyTask;
                break;
            case TaskType.MoveCharacter:
                Task = moveCharacterTask;
                break;
        }
    }
}