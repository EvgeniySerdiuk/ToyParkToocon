using System;

[System.Serializable]
public class Characteristic
{
    public string Name;
    public float[] value;
    public int[] UpgradeCost;
    public CharacteristicType Type;

    //[System.NonSerialized]
    public int currentLevel = 1;
    public int Level => currentLevel;
    public float Value { get { return value[currentLevel - 1]; } }

    public event Action changeValue;

    public void Upgrade()
    {
        if (currentLevel + 1 <= value.Length)
        {
            currentLevel++;
            changeValue?.Invoke();
        }
    }
}
