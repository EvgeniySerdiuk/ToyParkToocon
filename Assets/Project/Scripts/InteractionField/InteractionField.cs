using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionField : MonoBehaviour
{
    [Header("ProgressBar")]
    [SerializeField] protected Image progressBar;

    [Header("Field View")]
    [SerializeField] private Image[] field;
    [SerializeField] private TextMeshProUGUI[] text;

    public bool IsPlayerOnField => playerOnField;
    
    public event Action IsFinishFilling;
    protected Inventory inventory;

    private Color currentColor = new(0.8f, 0.8f, 0.8f, 1);
    protected float currentTime;
    protected bool playerOnField;
    protected PlayerTag player;
    
    private void Start()
    {
        DeactivateField();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        inventory = other.GetComponent<Inventory>();
        if (other.TryGetComponent<PlayerTag>(out var isPlayer))
        {
            playerOnField = true;
            player = isPlayer;
        }
            
        ActivateField();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        DeactivateField();
        if (other.TryGetComponent<PlayerTag>(out var tag))
        {
            playerOnField = false;
            player = null;
        }
    }

    protected void FillingProgressBar()
    {
        currentTime += Time.deltaTime;
        progressBar.fillAmount = currentTime / 1;
    }

    protected void FillingProgressBar(float current,float max)
    {
        progressBar.fillAmount = current / max;
    }

    protected virtual void FinishFiling()
    {
        InvokeFinishFilling();
    }

    protected void InvokeFinishFilling()
    {
        IsFinishFilling?.Invoke();
    }
    
    protected void ResetProgressBar()
    {
        progressBar.fillAmount = 0;
        currentTime = 0;
    }

    protected void ActivateField()
    {
        if (field.Length > 0)
        {
            foreach (var item in field)
            {
                item.color = Color.white;
            }
        }

        if(text.Length > 0)
        {
            foreach (var item in text)
            {
                item.color = Color.white;
            }
        }
    }

    protected void DeactivateField()
    {
        if (field.Length > 0)
        {
            foreach (var item in field)
            {
                item.color = currentColor;
            }
        }

        if (text.Length > 0)
        {
            foreach (var item in text)
            {
                item.color = currentColor;
            }
        }
    }
}
