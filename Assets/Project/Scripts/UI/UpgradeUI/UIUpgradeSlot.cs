using System;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeSlot : MonoBehaviour
{
    [Header("Text slot")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI middleText;
    [SerializeField] private TextMeshProUGUI costUpgrade;
    [SerializeField] private TextMeshProUGUI amountValueUpgrade;
    [SerializeField] private Button button;

    [Header("Images slot")]
    [SerializeField] private Image icon;

    [Header("Slot Text")]
    [SerializeField] private UpgradeSlotTextConfig textConfig;

    public Characteristic Characteristic => characteristic;
    public event Action OnUpgradeEvent;
    
    private UIProgressBarSlot progressBar;
    private Characteristic characteristic;
    private SlotText slotText;
    private ICounter wallet;
    private IEventBus bus;
    
    public void Init(Characteristic characteristic, ICounter wallet, IEventBus bus)
    {
        this.characteristic = characteristic;
        this.wallet = wallet;
        this.bus = bus;
        progressBar = GetComponentInChildren<UIProgressBarSlot>();
        Construct();
    }

    private void Construct()
    {
        progressBar.Init(characteristic.value.Length, characteristic.Level);
        slotText = textConfig.GetText(characteristic.Type);
        headerText.text = slotText.TitleText;
        icon.sprite = slotText.Icon;
        UpgradeTextField();
        AddListenerButton();
        CheckLevel();
        wallet.OnCountChangeEvent += CheckLevel;
    }

    private void Upgrade()
    {
        wallet.TryChangeCountByVal(-characteristic.UpgradeCost[characteristic.Level - 1]);
        progressBar.UpgradeBar(characteristic.Level);
        UpgradeTextField();       
        CheckLevel();
        bus.Raise(new SaveEvent());
        OnUpgradeEvent?.Invoke();
    }

    private void UpgradeTextField()
    {
        middleText.text = characteristic.Value + slotText.AbbreviationText;

        if (characteristic.Level >= characteristic.value.Length)
        {
            costUpgrade.text = "MAX";
            amountValueUpgrade.text = string.Empty;
            return;
        }

        float value = characteristic.value[characteristic.Level] - characteristic.Value;
        float roundedValue = (float)Math.Round(value, 1);
        amountValueUpgrade.text = value > 0 ? "+" + roundedValue : roundedValue.ToString();
        costUpgrade.text = characteristic.UpgradeCost[characteristic.Level].ToString();
    }

    private void AddListenerButton()
    {
        button.onClick.AddListener(characteristic.Upgrade);
        button.onClick.AddListener(Upgrade);
    }

    private void OnDisable()
    {
        wallet.OnCountChangeEvent -= CheckLevel;
        button.onClick.RemoveAllListeners();
    }

    private void CheckLevel()
    {
        if (characteristic.Level >= characteristic.value.Length || wallet.Count < characteristic.UpgradeCost[characteristic.Level])
        {
            button.interactable = false;
        }
    }
}
