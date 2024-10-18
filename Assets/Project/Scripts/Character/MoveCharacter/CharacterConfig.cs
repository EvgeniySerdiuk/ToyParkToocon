using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "CharacterConfig")]
public class CharacterConfig : ScriptableObject, ISaveable
{
    [Header("Inventory")]
    [SerializeField] private int currentInventoryCapacity;
    [SerializeField] private int maxInventoryCapacity;
    [Space]
    [SerializeField] private float currentSpeedInteraction;
    [SerializeField] private float minSpeedInteraction;
    [Header("Move")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed;
    [Space]
    [SerializeField] private float rotationSpeed;
    [Header("Level")]
    [SerializeField] private int[] expToNextLevel; 
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private int upgradePoints;

    public int MaxInventoryCapacity => maxInventoryCapacity;
    public float MaxSpeed => maxSpeed;
    public float MinSpeedInteraction => minSpeedInteraction;
    public float CurrentSpeed => currentSpeed;
    public float RotationSpeed => rotationSpeed;
    public int CurrentInventoryCapacity => currentInventoryCapacity;
    public float CurrentSpeedInteraction => currentSpeedInteraction;
    public int CurrentLevel => currentLevel;
    public int UpgradePoint => upgradePoints;

    public event Action levelUp;

    private int[] upgradeHistory = new int[3];

    public JToken CaptureAsJToken()
    {
        JToken[] save = new[] 
        { 
            JToken.FromObject(upgradePoints),
            JToken.FromObject(upgradeHistory)
        };
        return JToken.FromObject(save);
    }

    public void RestoreFromJToken(JToken state)
    {
        currentInventoryCapacity = 3;
        currentSpeedInteraction = 2.1f;
        currentSpeed = 2.2f;

        JToken[] save = state.ToObject<JToken[]>();
        upgradePoints = save[0].ToObject<int>();
        upgradeHistory = save[1].ToObject<int[]>();
        
        for (int i = 0; i < upgradeHistory[0]; i++)
        {
            currentSpeed += 0.1f;
        }
        for (int i = 0; i < upgradeHistory[1]; i++)
        {
            currentInventoryCapacity++;
        }
        for (int i = 0; i < upgradeHistory[2]; i++)
        {
            currentSpeedInteraction -= 0.1f;
        }
    }
    
    public void ResetExp()
    {
        currentLevel = 0;
        upgradePoints = 0;
    }
    
    public void UpgradeLevel()
    {
        if(currentLevel < maxLevel) 
        {
            currentLevel++;
            upgradePoints++;
            levelUp?.Invoke();
        }
    }

    public void UpgradeLevelUI()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
    }
    
    public void UpgradeMoveSpeed()
    {
        if (currentSpeed < maxSpeed && upgradePoints > 0)
        {
            upgradePoints--;
            currentSpeed += 0.1f;
            upgradeHistory[0]++;
        }
    }

    public void UpgradeInventory()
    {
        if(currentInventoryCapacity < maxInventoryCapacity && upgradePoints > 0)
        {
            upgradePoints--;
            currentInventoryCapacity++;
            upgradeHistory[1]++;
        }
    }

    public void UpgradeInteractionSpeed()
    {
        if(currentSpeedInteraction > minSpeedInteraction && upgradePoints > 0)
        {
            upgradePoints--;
            currentSpeedInteraction -= 0.1f;
            upgradeHistory[2]++;
        }
    }

    public int GiveAmountExpForNexLevel()
    {
        if(currentLevel >= maxLevel) return 0;
        return expToNextLevel[currentLevel];
    }
}
