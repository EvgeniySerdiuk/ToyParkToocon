using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBarSlot : MonoBehaviour
{
    [SerializeField] private Image upBar;
    [SerializeField] private Image downBar;

    private List<Image> progressBar = new List<Image>(); 

    public void Init(int amountLevel,int currentLevel)
    {
        for (int i = 0; i < amountLevel; i++)
        {
           var obj = Instantiate(downBar,transform);
           if (i < currentLevel) obj.sprite = upBar.sprite;
           progressBar.Add(obj);
        }
    }

    public void UpgradeBar(int currentLevel) 
    {
        progressBar[currentLevel - 1].sprite = upBar.sprite;
    }
}
