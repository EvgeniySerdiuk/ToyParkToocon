using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIExperienceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountExpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image filedimage;

    private PlayerExperience playerExperience;

    [Inject]
    private void Init(PlayerExperience experience)
    {
        playerExperience = experience;
    }

    private void OnEnable()
    {
        playerExperience.UpgradeLevel += ChangeValue;
    }
    private void OnDisable()
    {
        playerExperience.UpgradeLevel -= ChangeValue;
    }

    private void Start()
    {
        ChangeValue();
    }

    private void ChangeValue()
    {
        amountExpText.text = $"{playerExperience.ExperienceCounter.Count}/{playerExperience.expToNextLevel[playerExperience.CurrentLevel-1]}";
        levelText.text = $"{playerExperience.CurrentLevel}";
        filedimage.fillAmount = (float)playerExperience.ExperienceCounter.Count / (float)playerExperience.expToNextLevel[playerExperience.CurrentLevel-1];
    }
}
