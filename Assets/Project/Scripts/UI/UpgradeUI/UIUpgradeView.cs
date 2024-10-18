using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeView : MonoBehaviour, IEventReceiver<MoveCameraEvent>
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image headImage;
    [SerializeField] private TextMeshProUGUI headText;
    private UIUpgradeSlot slotPrefab;
    private GridLayoutGroup slotsContainer;
    private UIUpgradeSlot[] upgradeSlots;
    private ICounter wallet;
    private IEventBus bus;
    
    public void Init(Characteristic[] characteristics, UIUpgradeSlot upgradeSlotPrefab, string HeaderText, Sprite headImage, string headerNameText,ICounter wallet, IEventBus bus)
    {
        slotsContainer = GetComponentInChildren<GridLayoutGroup>();
        upgradeSlots = new UIUpgradeSlot[characteristics.Length];
        slotPrefab = upgradeSlotPrefab;
        text.text = HeaderText;
        this.headImage.sprite = headImage;
        this.headText.text = headerNameText;
        this.wallet = wallet;
        this.bus = bus;
        bus.Register(this);
        CreateSlots(characteristics);
    }

    public void CharacterCharacteristics()
    {
        foreach (var slot in upgradeSlots)
        {
            slot.OnUpgradeEvent += AppMetricaReportEvent;
        }
    }
    
    private void OnEnable()
    {
        if (bus != null)
        {
            bus.Register(this);
            foreach (var slot in upgradeSlots)
            {
                slot.OnUpgradeEvent += AppMetricaReportEvent;
            }
        }
            
    }

    private void OnDisable()
    {
        if (bus != null)
        {
            bus.Unregister(this);
            foreach (var slot in upgradeSlots)
            {
                slot.OnUpgradeEvent -= AppMetricaReportEvent;
            }
        }
    }

    private void AppMetricaReportEvent()
    {
        int startLevel = -3;
        foreach (var slot in upgradeSlots)
        {
            startLevel += slot.Characteristic.currentLevel;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("level", startLevel);

        AppMetrica.Instance.ReportEvent("level_up", parameters);
    }
    
    private void CreateSlots(Characteristic[] characteristics)
    {
        SettingSizeContainer();

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i] = Instantiate(slotPrefab, slotsContainer.transform);
            upgradeSlots[i].Init(characteristics[i],wallet,bus);
        }
    }

    private void SettingSizeContainer()
    {
        var containerRect = slotsContainer.GetComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(300 * upgradeSlots.Length, 600);
    }

    public void CloseUpgradeView()
    {
        Destroy(this.gameObject);
    }

    public void OnEvent(MoveCameraEvent @event)
    {
        StartCoroutine(LateClose());
    }

    private IEnumerator LateClose()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = false; 
        yield return new WaitForSeconds(1f);
        canvas.enabled = true;
        CloseUpgradeView();
    }
}
