using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Project.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

public class BuyField : InteractionField, ISaveable
{
    [SerializeField] private float timeDelay;
    [Header("Attraction")]
    [SerializeField] protected InteractiveBuilding[] interactivebuilding;
    [SerializeField] protected MonoBehaviour[] savableObjects;
    [SerializeField] protected float price;
    [SerializeField] protected TextMeshProUGUI textPrice;
    
    [Header("Expirience")]
    [SerializeField] protected int amountExpSpawn;
    [SerializeField] private Experience experiencePrefab;
    [SerializeField] private Transform[] pointsToSpawnPrefab;


    private ICounter wallet;
    protected float currentMoney;
    private float currentDelayTime;

    // flag to save system
    protected bool used;
    public bool Used => used;

    [Inject]
    private void Init(Wallet wallet)
    {
        this.wallet = wallet.WalletCounter;
    }    

    protected virtual void Start()
    {
        textPrice.text = price.ToString();
        DeactivateField();
    }

    private void OnTriggerStay(Collider other)
    {
        currentDelayTime += Time.deltaTime;
        if(timeDelay < currentDelayTime) 
        {           
            if (wallet.TryChangeCountByVal(-100))
            {
                currentMoney += 100;
                var amountMoney = (int)(price - currentMoney);
                textPrice.text = amountMoney.ToString();
                FillingProgressBar(currentMoney, price);
            }

            if(progressBar.fillAmount >= 1)
            {
                currentDelayTime = 0;
                currentMoney = 0;
                FinishFiling();
            }
        }       
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        currentDelayTime = 0;
    }

    protected void GiveExperience()
    {
        for (int i = 0; i < amountExpSpawn; i++)
        {
            Instantiate(experiencePrefab, pointsToSpawnPrefab[Random.Range(0, pointsToSpawnPrefab.Length)].position, Quaternion.identity);
        }
    }

    protected override void FinishFiling()
    {
        used = true;
        GiveExperience();
        Finish();
        GlobalEvents.InteractionFieldFinishFilling();
        InvokeFinishFilling();
    }

    protected virtual void Finish()
    {
        foreach (var building in interactivebuilding)
            building.gameObject.SetActive(true);
        //костыль
        transform.parent = null;
        transform.position = new Vector3(0,-100,0);
    }
    
    public virtual JToken CaptureAsJToken()
    {
        List<JToken> tokens = new List<JToken>(savableObjects.Length+2);
        tokens.Add(JToken.FromObject(used));
        tokens.Add(JToken.FromObject(gameObject.activeSelf));
        for (int i = 0; i < savableObjects.Length; i++)
            if (savableObjects[i] is ISaveable saveable)
                tokens.Add(saveable.CaptureAsJToken());
        
        return JToken.FromObject(tokens);
    }

    public virtual void RestoreFromJToken(JToken state)
    {
        var tokens = state.ToObject<List<JToken>>();
        used = tokens[0].ToObject<bool>();
        if (!used)
        {
            bool activated = tokens[1].ToObject<bool>();
            gameObject.SetActive(activated);
            return;
        }
        for (int i = 0; i < savableObjects.Length; i++)
            if (savableObjects[i] is ISaveable saveable)
                saveable.RestoreFromJToken(tokens[2 + i]);
        Finish();
    }
}
