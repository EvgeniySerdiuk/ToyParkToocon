using Project.Scripts.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUpgradeField : MonoBehaviour
{
    [Header("Field Data")]
    [SerializeField] private Image delayImage;
    [Space]
    [SerializeField] private float delayTime;

    [SerializeField] private GameObject upgradableObject;

    private IUpgradable upgradableObj;
    public IUpgradable UpgradableObj => upgradableObj;

    private void Awake()
    {
        upgradableObj = upgradableObject.GetComponent<IUpgradable>();
        upgradableObj.Characteristics.AllCharacteristicsImproved += DestroyField;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(UpgradeObj());
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        ResetDelay();
    }

    IEnumerator StartDelay()
    {
        float currentTime = 0;

        while (currentTime <= delayTime)
        {
            currentTime += Time.deltaTime;
            delayImage.fillAmount = currentTime / delayTime;
            yield return null;
        }
    }

    private IEnumerator UpgradeObj()
    {
        yield return StartDelay();
        upgradableObj.Upgrade();
        
    }

    private void ResetDelay()
    {
        delayImage.fillAmount = 0;
    }

    private void DestroyField()
    {
        upgradableObj.Characteristics.AllCharacteristicsImproved -= DestroyField;
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
