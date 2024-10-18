using Project.Scripts.Arch.Factory.Worker;
using Project.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SaveableEntity))]
public class FieldByuUpgrade : MonoBehaviour, ISaveable
{
    [Header("Field Data")]
    [SerializeField] private Image delayImage;
    [SerializeField] private Image progressImage;
    [SerializeField] private Image staticBG;
    [SerializeField] private Sprite lockSprite;
    [SerializeField] private Sprite buySprite;
    [SerializeField] private Sprite upgradeSprite;
    [SerializeField] private TextMeshProUGUI textField;
    [Space]
    [SerializeField] private float delayTime;

    [Header("Object")]
    [SerializeField] private ObjectType objectType;
    [SerializeField] private GameObject preViewImageObj;
    [SerializeField] private int locationId;
    [SerializeField] private int idObject;
    [SerializeField] private int idCharacteristic;
    [SerializeField] private int price;
    [SerializeField] private Transform spawnPoint;

    [Header("Exp")]
    [SerializeField] private Experience expPrefab;
    [SerializeField] private bool giveExp;
    [SerializeField] private int expForOpen;

    private float stepMoneyRate;
    private int amountMoneyRate = 10;

    private BuildingsFactory buildingsFactory;
    private IWorkerFactory workerFactory;
    private IUpgradable upgradableObj;
    private IBuyable buyable;
    private ICounter wallet;
    private IEventBus bus;
    private SaveableEntity saveableEntity;
    
    private int paidMoney;
    private int currentLevel;
    private Collider mainCollider;
    private bool firstTimeActivated = true;
    private bool stageCompleted = false;
    private float time;
    
    public Transform SpawnPoint => spawnPoint;
    public int ObjectId => idObject;
    public ObjectType ObjType => objectType;
    public IUpgradable UpgradableObj => upgradableObj;
    public IBuyable Buyable => buyable;
    public event Action InstantiateObj;

    [Inject]
    private void Init(Wallet wallet, BuildingsFactory buildingsFactory, IWorkerFactory workerFactory, IEventBus bus)
    {
        this.wallet = wallet.WalletCounter;
        this.buildingsFactory = buildingsFactory;
        this.workerFactory = workerFactory;
        this.bus = bus;
    }

    private void Awake()
    {
        mainCollider = GetComponent<Collider>();
        saveableEntity = GetComponent<SaveableEntity>();
        textField.text = price.ToString();
        staticBG.sprite = buySprite;
    }

    private void Start()
    {
        if(upgradableObj != null)
            upgradableObj.Characteristics.CheckLevel();
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        UseField(other);
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        ResetDelay();
    }

    private void UseField(Collider other)
    {
        switch (currentLevel)
        {
            case 0:
                StartCoroutine(ByuObj(other));
                break;
            case 1:
                StartCoroutine(UpgradeObj());
                break;
        }
    }

    private IEnumerator StartDelay()
    {
        float currentTime = 0;

        while (currentTime <= delayTime)
        {
            currentTime += Time.deltaTime;
            delayImage.fillAmount = currentTime / delayTime;
            yield return null;
        }
    }

    private IEnumerator ByuObj(Collider other)
    {
        yield return StartDelay();

        if (wallet.Count < 10) yield break;

        InteractEvents(other.transform);
        stepMoneyRate = 3.5f;

        while (paidMoney < price)
        {
            if (wallet.Count <= 0)
            {
                yield break;
            }

            if (wallet.Count < amountMoneyRate)
            {
                InteractEventsEnd();
            }

            if(stepMoneyRate> 1.5f)
            {
                stepMoneyRate -= Time.deltaTime;
            }

            amountMoneyRate = Mathf.RoundToInt(price / stepMoneyRate * Time.deltaTime);

            if (paidMoney + amountMoneyRate > price)
            {
                amountMoneyRate = price - paidMoney;
            }

            if(amountMoneyRate > wallet.Count && wallet.Count != 0)
            {
                amountMoneyRate = wallet.Count;
            }

            DebitMoney();

            yield return null;
        }

        Buy();
        GiveOutExperience();
        SaveProgress();
    }

    private void DebitMoney()
    {
        if (wallet.TryChangeCountByVal(-amountMoneyRate))
        {
            paidMoney += amountMoneyRate;
            textField.text = (price - paidMoney).ToString();
            progressImage.fillAmount = (float)paidMoney / price;
        }
    }
    
    private void Buy()
    {
        switch (objectType)
        {
            case ObjectType.Building:
                var building =  buildingsFactory.GetBuilding(spawnPoint, locationId, idObject, idCharacteristic);
                upgradableObj = building;
                buyable = building;
                break;
            case ObjectType.Worker:
                var worker = workerFactory.GetWorker(idObject, idCharacteristic, locationId, spawnPoint.position);
                upgradableObj = worker;
                buyable = worker;
                break;
        }

        UpgradeField();
        InstantiateObj?.Invoke();
        StopAllCoroutines();
        InteractEventsEnd();
        SaveProgress();
        upgradableObj.Characteristics.AllCharacteristicsImproved += DestroyField;
    }

    private void UpgradeField()
    {
        currentLevel = 1;
        staticBG.sprite = upgradeSprite;
        Destroy(textField);
        Destroy(progressImage.gameObject);
        Destroy(preViewImageObj);
    }
    
    private IEnumerator UpgradeObj()
    {
        yield return StartDelay();
        upgradableObj.Upgrade();
    }

    public void LockField()
    {
        mainCollider.enabled = false;
        staticBG.sprite = lockSprite;
        textField.text = $"NEED {expForOpen} EXP";
    }

    public void BuyField()
    {
        mainCollider.enabled = true;
        staticBG.sprite = buySprite;
        textField.text = price.ToString();
    }

    public void AppMetricaActivate()
    {
        if(!firstTimeActivated)
            return;
        firstTimeActivated = false;
        AppMetricaStageStart();
    }
    
    private void GiveOutExperience()
    {
        if (giveExp == false) return;

        Instantiate(expPrefab, transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);
    }

    private void ResetDelay()
    {
        delayImage.fillAmount = 0;
        InteractEventsEnd();
    }

    private void DestroyField()
    {
        upgradableObj.Characteristics.AllCharacteristicsImproved -= DestroyField;
        StopAllCoroutines();
        saveableEntity.DeleteThis();
        if (!stageCompleted)
        {
            stageCompleted = true;
            AppMetricaStageEnd();
            SaveProgress();
        }
        
        saveableEntity.Delete();
    }

    public enum ObjectType
    {
        Building,
        Worker
    }

    private void SaveProgress()
    {
        bus.Raise(new SaveEvent());
    }
    
    private void InteractEvents(Transform t)
    {
        bus.Raise(new MoneyStartSpendEvent(transform.position, t));
        bus.Raise(new LongVibrateEventStart());
    }

    private void InteractEventsEnd()
    {
        bus.Raise(new MoneyEndSpendEvent());
        bus.Raise(new LongVibrateEventEnd());
    }

    #region AppMetrica

    private void AppMetricaStageStart()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        int workerOffset = objectType == ObjectType.Worker ? 7 : 0;
        int stage_count = idObject + workerOffset;
        parameters.Add("stage_count", stage_count);
        string stage_name = locationId.ToString() + "_" + gameObject.transform.parent.gameObject.name;
        parameters.Add("stage_name", stage_name);

        AppMetrica.Instance.ReportEvent("stage_start", parameters);
    }

    private void AppMetricaStageEnd()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        int workerOffset = objectType == ObjectType.Worker ? 7 : 0;
        int stage_count = idObject + workerOffset;
        parameters.Add("stage_count", stage_count);
        string stage_name = locationId.ToString() + "_" + gameObject.transform.parent.gameObject.name;
        parameters.Add("stage_name", stage_name);
        parameters.Add("time", (int) time);
        
        AppMetrica.Instance.ReportEvent("stage_finish", parameters);
    }
    
    #endregion
    
    #region Save
    public JToken CaptureAsJToken()
    {
        JToken[] token = new JToken[5];
        token[0] = JToken.FromObject(paidMoney);
        token[1] = JToken.FromObject(currentLevel);
        token[2] = JToken.FromObject(firstTimeActivated);
        token[3] = JToken.FromObject(time);
        token[4] = JToken.FromObject(stageCompleted);
        return JToken.FromObject(token);
    }

    public void RestoreFromJToken(JToken state)
    {
        JToken[] array = state.ToObject<JToken[]>();
        paidMoney =  array[0].ToObject<int>();
        currentLevel = array[1].ToObject<int>();
        firstTimeActivated = array[2].ToObject<bool>();
        time = array[3].ToObject<float>();
        stageCompleted = array[4].ToObject<bool>();
        if(buyable != null)
            return;
        if (currentLevel > 0)
            BuyWithoutSave();
    }
    
    private void BuyWithoutSave()
    {
        switch (objectType)
        {
            case ObjectType.Building:
                var building =  buildingsFactory.GetBuilding(spawnPoint, locationId, idObject, idCharacteristic);
                upgradableObj = building;
                buyable = building;
                break;
            case ObjectType.Worker:
                var worker = workerFactory.GetWorker(idObject, idCharacteristic, locationId, spawnPoint.position);
                upgradableObj = worker;
                buyable = worker;
                break;
        }

        UpgradeField();
        InstantiateObj?.Invoke();
        StopAllCoroutines();
        InteractEventsEnd();
        upgradableObj.Characteristics.AllCharacteristicsImproved += DestroyField;
    }
    #endregion
}
