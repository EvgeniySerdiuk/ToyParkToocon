using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCharacterPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI currentMoveSpeedText;
    [SerializeField] private TextMeshProUGUI currentInteractiveSpeedText;
    [SerializeField] private TextMeshProUGUI currentCapacityInventory;
    [SerializeField] private Button[] buttons;
    private CharacterConfig characterConfig;

    private void Awake()
    {
        LoadConfig();
        int index = -1;
        foreach (var button in buttons) 
        {
            index++;
            if(button == null)
            {
                Debug.Log("Button " + index + " null ref");
                continue;
            }
            button.onClick.AddListener(CheckUpgradePoints);
        }

        characterConfig.levelUp += ShowPanel;

        panel.SetActive(false);
    }

    private void OnEnable()
    {
        CheckUpgradePoints();
    }

    private void ShowPanel()
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        CheckUpgradePoints();
    }

    public void OffShowPanel()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
    }

    private void LoadConfig()
    {
        characterConfig = Resources.Load<CharacterConfig>(ConfigsPath.CHARACTER_CONFIG);
    }

    private void CheckUpgradePoints()
    {
        if (currentMoveSpeedText != null)
            currentMoveSpeedText.text = $"{Math.Round(characterConfig.CurrentSpeed, 2)} км/ч";

        if (currentCapacityInventory != null)
            currentCapacityInventory.text = $"{characterConfig.CurrentInventoryCapacity} шт";

        if (currentInteractiveSpeedText != null)
            currentInteractiveSpeedText.text = $"{Math.Round(characterConfig.CurrentSpeedInteraction, 2)} сек";

        if (characterConfig.UpgradePoint < 1)
        {
            foreach (var btn in buttons)
            {
                if(btn != null)
                btn.interactable = false;
            }

            //OffShowPanel();
        }
        else
        {
            foreach (var btn in buttons)
            {
                if (btn != null)
                btn.interactable = true;
            }
        }

        //КОСТЫЛЬ
        if (characterConfig.CurrentSpeed >= characterConfig.MaxSpeed) { buttons[0].interactable = false; buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "МАКС"; }
        if (characterConfig.CurrentSpeedInteraction <= characterConfig.MinSpeedInteraction) { buttons[1].interactable = false; buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "МАКС"; }
        if (characterConfig.CurrentInventoryCapacity >= characterConfig.MaxInventoryCapacity) { buttons[2].interactable = false; buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "МАКС"; }
    }
}
