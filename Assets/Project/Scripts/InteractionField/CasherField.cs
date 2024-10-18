using Project.Scripts.Interfaces;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CasherField : InteractionField
{
    protected AudioSource audioSource;
    protected bool isActive;
    protected float workSpeed;
    private Collider mainCollider;

    private void Awake()
    {
        mainCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        CheckWorker(other);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if(!isActive) 
            return;
        currentTime += Time.deltaTime;
        FillingProgressBar(currentTime, workSpeed);

        if (progressBar.fillAmount == 1)
        {
            FinishFiling();
            ResetProgressBar();
            currentTime = 0;
            if (IsPlayerOnField && player != null) 
                audioSource.PlayOneShot(audioSource.clip);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        ResetProgressBar();
    }

    public void SetActive(bool value)
    {
        isActive = value;

        if (value)
        {
            ActivateField();
            // mainCollider.enabled = true;
        }
        else
        {
            ResetProgressBar();
            DeactivateField();
            // mainCollider.enabled = false;
        }
    }

    private void CheckWorker(Collider collider)
    {
        if(collider.TryGetComponent(out IUpgradable upgradable))
        {
            workSpeed = upgradable.Characteristics.Value(CharacteristicType.WorkSpeedWorker);
        }
    }
}
