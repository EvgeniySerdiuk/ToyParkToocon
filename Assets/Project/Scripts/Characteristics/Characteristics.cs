using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Characteristics
{
    public string Name;
    public string gidId;
    public string HeaderText;
    public string HeaderNameText { get {  return Name.Split('_')[0];}}
    public Sprite HeaderImage;

    [SerializeField] private List<Characteristic> upgradableCharacteristics;

    public Characteristic[] UpgradableCharacteristics => upgradableCharacteristics.ToArray();
    public event Action AllCharacteristicsImproved;
    
    public void Init()
    {
        DownloadCharacteristics();

        foreach (var characteristic in upgradableCharacteristics)
        {
            characteristic.changeValue += CheckLevel;
            characteristic.changeValue += SaveCharacteristicks;
        }
    }


    public void DownloadData(string sheetId)
    {
        ReadGoogleSheets.FillData<Characteristic>(sheetId,gidId, calBack: list =>
        {
            upgradableCharacteristics = list;
        });
    }

    public float Value(CharacteristicType type)
    {
        return upgradableCharacteristics.First(x => x.Type == type).Value;
    }

    public Characteristic GetCharacteristic(CharacteristicType type)
    {
        return UpgradableCharacteristics.FirstOrDefault(x => x.Type == type);
    }

    public void CheckLevel()
    {
        foreach (var characteristic in upgradableCharacteristics)
        {
            if(characteristic.Level < characteristic.value.Length)
            {
                return;
            }
        }

        AllCharacteristicsImproved?.Invoke();
    }

    private void SaveCharacteristicks()
    {
        Debug.Log("���������");
        var path = Path.Combine(Application.persistentDataPath, gidId + Name + ".json");

        try
        {
            using (var textWriter = File.CreateText(path))
            {
                using (var writer = new JsonTextWriter(textWriter))
                {
                    JsonSerializer serializer = new();
                    serializer.Serialize(writer, upgradableCharacteristics);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }

    private void DownloadCharacteristics()
    {
        foreach (var item in upgradableCharacteristics)
        {
            item.currentLevel = 1;
        }

        var path = Path.Combine(Application.persistentDataPath, gidId + Name + ".json");

        if(File.Exists(path))
        {
            using (var textReader = File.OpenText(path))
            {
                using (var reader = new JsonTextReader(textReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var obj = serializer.Deserialize<List<Characteristic>>(reader);

                    for (int i = 0; i < obj.Count; i++)
                    {
                        for (int j = 0; upgradableCharacteristics[i].currentLevel < obj[i].currentLevel; j++)
                        {
                            upgradableCharacteristics[i].Upgrade();
                        }                    
                    }
                }
            }
        }
    }
}
