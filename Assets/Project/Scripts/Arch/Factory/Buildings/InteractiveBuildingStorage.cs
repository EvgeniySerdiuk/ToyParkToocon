using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractiveBuildingStorage", menuName = "Storage/InteractiveBuildingStorage", order = 1)]
public class InteractiveBuildingStorage : ScriptableObject
{
    private const string SheetId = "1IPzKH2PMSMj9OYGerKctDElEUGVB7kO4YFkckuHy1bg";
    public InteractiveBuilding[] AvailableBuildings;
    public Characteristics[] Characteristics;


    [Button]
    public void DownloadCharacteristics()
    {
        foreach (var Char in Characteristics)
        {
            Char.DownloadData(SheetId);
            ReadGoogleSheets.SetDirty(this);
        }
    }
}
