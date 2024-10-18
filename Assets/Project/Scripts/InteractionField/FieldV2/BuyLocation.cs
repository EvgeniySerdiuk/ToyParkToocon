using System;
using System.Collections;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyLocation : MonoBehaviour
{
    [Header("Field Data")]
    [SerializeField] private Image delayImage;
    [SerializeField] private Image progressImage;
    [SerializeField] private TextMeshProUGUI priceText;
    [Space]
    [SerializeField] private float delayTime;

    [Header("Object")]
    [SerializeField] private int costLocation;
    [SerializeField] private GameObject wall;

    private float stepMoneyRate;
    private int amountMoneyRate = 10;
    private const float startMoneyRate = 0.2f;

    private int paidMoney;
    private ICounter wallet;
    private IEventBus bus;
    private bool activated;
    private PlayerExperience experience;
    private bool moneyFlowActive;
    
    public event Action LocationBuying;
    public event Action<BuyLocation> LocationBuy;
    public bool LocationBought { get; private set; }
    
    [Inject]
    public void Init(Wallet wallet, IEventBus bus, PlayerExperience playerExperience)
    {
        this.wallet = wallet.WalletCounter;
        this.bus = bus;
        experience = playerExperience;
    }

    private void Start()
    {
        if(activated)
            gameObject.SetActive(true);
    }

    protected virtual void OnEnable()
    {
        priceText.text = costLocation.ToString();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ByuObj(other));
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        ResetDelay();
        InteractEventsEnd();
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

        stepMoneyRate = 3.5f;

        while (paidMoney < costLocation)
        {
            if (wallet.Count <= 0)
            {
                if(moneyFlowActive)
                    InteractEventsEnd();
                yield break;
            }

            if (stepMoneyRate > 1.5f)
            {
                stepMoneyRate -= Time.deltaTime;
            }

            amountMoneyRate = Mathf.RoundToInt(costLocation / stepMoneyRate * Time.deltaTime);

            if (paidMoney + amountMoneyRate > costLocation)
            {
                amountMoneyRate = costLocation - paidMoney;
            }

            if (amountMoneyRate > wallet.Count && wallet.Count != 0)
            {
                amountMoneyRate = wallet.Count;
            }

            DebitMoney(other);

            yield return null;
        }

        Buy();
        bus.Raise(new SaveEvent());
        DestroyField();
    }

    private void Buy()
    {
        LocationBought = true;
        experience.LevelUp();
        LocationBuying?.Invoke();
        LocationBuy?.Invoke(this);
        Destroy(wall);
    }

    private void DebitMoney(Collider other)
    {
        if (wallet.TryChangeCountByVal(-amountMoneyRate))
        {
            if(!moneyFlowActive)
                InteractEvents(other.transform);
            paidMoney += amountMoneyRate;
            priceText.text = (costLocation - paidMoney).ToString();
            progressImage.fillAmount = (float)paidMoney / costLocation;
        }
        else if(moneyFlowActive)
            InteractEventsEnd();
    }

    private void ResetDelay()
    {
        delayImage.fillAmount = 0;
    }

    public void DestroyField()
    {
        InteractEventsEnd();
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void Use()
    {
        Buy();
        DestroyField();
    }
    
    private void InteractEvents(Transform t)
    {
        bus.Raise(new MoneyStartSpendEvent(transform.position, t));
        bus.Raise(new LongVibrateEventStart());
        moneyFlowActive = true;
    }

    private void InteractEventsEnd()
    {
        if(!moneyFlowActive)
            return;
        
        bus.Raise(new MoneyEndSpendEvent());
        bus.Raise(new LongVibrateEventEnd());
        moneyFlowActive = false;
    }
}
