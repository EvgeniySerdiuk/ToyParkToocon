using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProjectLoad : MonoBehaviour
{
    [SerializeField] private Image bar;

    void Awake()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        float curentTime = 0;

        while (curentTime < 10.5f)
        {
            curentTime += Time.deltaTime;
            yield return null;
            bar.fillAmount =  curentTime / 10;
        }
    }
}
