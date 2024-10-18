using Newtonsoft.Json.Linq;
using Project.Scripts.Wallet;
using System;

public class PlayerExperience : ISaveable
{
    private Counter experienceCounter;
    private const int maxLevel = 4;

    public readonly int[] expToNextLevel = { 8, 16, 24 };
    public int CurrentLevel { get; private set; }
    public Counter ExperienceCounter => experienceCounter;

    public event Action UpgradeLevel;

    public PlayerExperience()
    {
        experienceCounter = new Counter();
        CurrentLevel = 1;
    }

    public void AddExp(int amountMoney)
    {
        if (CurrentLevel >= maxLevel) return;

        experienceCounter.TryChangeCountByVal(amountMoney);
            
        UpgradeLevel?.Invoke();
    }

    public void LevelUp()
    {
        CurrentLevel++;
        UpgradeLevel?.Invoke();
    }

    public JToken CaptureAsJToken()
    {
        return experienceCounter.CaptureAsJToken();
    }

    public void RestoreFromJToken(JToken state)
    {
        experienceCounter.RestoreFromJToken(state);
    }
}
