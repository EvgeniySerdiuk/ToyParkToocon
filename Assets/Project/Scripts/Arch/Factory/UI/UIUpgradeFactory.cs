using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;

public class UIUpgradeFactory : MonoBehaviour
{
    private UIUpgradeView upgradeView;
    private UIUpgradeSlot upgradeSlot;
    private ICounter wallet;
    private IEventBus bus;
    public UIUpgradeFactory(UIUpgradeView viewPrefab, UIUpgradeSlot upgradeSlotPrefab, ICounter wallet, IEventBus bus)
    {
        upgradeView = viewPrefab;
        upgradeSlot = upgradeSlotPrefab;
        this.wallet = wallet;
        this.bus = bus;
    }

    public void GetUIUpgrade(Characteristics characteristics, bool characterCharacteristics = false)
    {
        var obj = Instantiate(upgradeView);
        obj.Init(characteristics.UpgradableCharacteristics, upgradeSlot, characteristics.HeaderText,
            characteristics.HeaderImage, characteristics.HeaderNameText, wallet, bus);
        
        if(characterCharacteristics)
            obj.CharacterCharacteristics();
    }
}
