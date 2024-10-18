using Newtonsoft.Json.Linq;
using UnityEngine;

public class BuyCleaner : BuyField
{
    [Header("Cleaner")]
    [SerializeField] private Cleaner cleaner;
    [SerializeField] private WorkersConfig cleanerConfig;
    [SerializeField] private Transform idlePoint;
    [SerializeField] private Toilet[] toilets;
    [SerializeField] private GarbageCanView[] garbageCans;
    [SerializeField] private TrashCanView trashCan;
    [SerializeField] private UpgradeWorkers upgradeWorkerField;

    protected override void Finish()
    {
        var obj = Instantiate(cleaner,this.transform.position,Quaternion.identity);
        upgradeWorkerField.gameObject.SetActive(true);
        upgradeWorkerField.Init(obj.GetComponent<IWorker>());
        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    public override JToken CaptureAsJToken()
    {
        JToken[] array = new JToken[2];
        array[0] = JToken.FromObject(used);
        array[1] = JToken.FromObject(upgradeWorkerField.CaptureAsJToken());
        return JToken.FromObject(array);
    }

    public override void RestoreFromJToken(JToken state)
    {
        JToken[] array = state.ToObject<JToken[]>();
        used = array[0].ToObject<bool>();
        if(!used)
            return;
        Finish();
        upgradeWorkerField.RestoreFromJToken(array[1]);
    }
}
