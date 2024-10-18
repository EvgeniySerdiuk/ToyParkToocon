using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeSlotTextConfig", menuName = "Config/UI/SlotTextConfig")]
public class UpgradeSlotTextConfig : ScriptableObject
{
    [SerializeField] private List<SlotText> textForSlots;

    private const string sheetId = "1xPQmGq30a2YiF8sQUuc0RAm1wAac7ViDRR35ZE_krMI";
    private const string gidId = "0";

    [Button]
    public void DownloadTextForSlot()
    {
        ReadGoogleSheets.FillData<SlotText>(sheetId, gidId, list =>
        {
            textForSlots = list;
            ReadGoogleSheets.SetDirty(this);
        });
    }

    public SlotText GetText(CharacteristicType type)
    {
        return textForSlots.First(x => x.Type == type);
    }
}

[System.Serializable]
public class SlotText
{
    public string TitleText;
    public string AbbreviationText;
    public Sprite Icon;
    public CharacteristicType Type;
}
