using Newtonsoft.Json.Linq;
using UnityEngine;

public class UpgradeWorkers : BuyField
{
    public int  CurrentLvl { get; private set; }
    private IWorker worker;

    public void Init(IWorker workerObj)
    {
        worker = workerObj;
        price = worker.UpgradePrice;
        amountExpSpawn = worker.AmountExp;
        base.Start();
    }

    protected override void Finish()
    {
        //костыль
        gameObject.SetActive(true);
        CurrentLvl++;
        worker.Upgrade();
        if (worker.MaxLevel)
        {
            //костыль
            //gameObject.SetActive(false);
            transform.parent = null;
                transform.position = new Vector3(0,-100,0);
        }

        price = worker.UpgradePrice;
        textPrice.text = price.ToString();
        amountExpSpawn = worker.AmountExp;
        ResetProgressBar();
    }

    public override JToken CaptureAsJToken()
    {
        return JToken.FromObject(CurrentLvl);
    }

    public override void RestoreFromJToken(JToken state)
    {
        int lvl = state.ToObject<int>();
        for (int i = 0; i < lvl; i++)
            Finish();
    }
}
