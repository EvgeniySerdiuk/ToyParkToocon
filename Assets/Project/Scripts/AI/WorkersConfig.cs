using UnityEngine;

[CreateAssetMenu(fileName = "NewWorkersConfig", menuName = "WorkersConfig")]
public class WorkersConfig : ScriptableObject
{
    [SerializeField] private int currentLevel;
    private int maxLevel = 3;

    [SerializeField] private int[] upgradePrice;
    [SerializeField] private int[] expForUpgrade;
    [SerializeField] private int[] inventoryCapacity;
    [SerializeField] private float[] moveSpeed;

    public int CurrentUpgradePrice { get; private set; }
    public int CurrentInventoryCapacity { get; private set; }
    public float CurrentMoveSpeed { get; private set; }
    public int AmountExp { get; private set; }

    public bool MaxLevel {get { return currentLevel >= maxLevel; } }

    public int Currentlevel {get { return currentLevel; } }
    public void ConfigInitialize()
    {
        //Проверяем есть ли сохранения этого конфига.
        //Если есть подтягиваем уровень оттуда. вычитаем 1 и вызываем апгрейд.
    }

    public void Upgrade()
    {
        if (currentLevel >= maxLevel) return;
        currentLevel++;
        CurrentMoveSpeed = moveSpeed[currentLevel - 1];
        CurrentInventoryCapacity = inventoryCapacity[currentLevel - 1];
        CurrentUpgradePrice = upgradePrice[currentLevel - 1];
        AmountExp = expForUpgrade[currentLevel - 1];
    }
}
