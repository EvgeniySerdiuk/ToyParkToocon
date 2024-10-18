using Newtonsoft.Json.Linq;
using UnityEngine;

public class BuyCasher : BuyField
{
    [Header("Casher")]
    [SerializeField] private Casher casher;
    [SerializeField] private WorkersConfig casherConfig;
    [SerializeField] private Attraction[] attractions;
    [SerializeField] private InteractionField takeTickets;
    [SerializeField] private UpgradeWorkers upgradeWorkerField;

    private JToken[] saved;
    private Casher spawnedChasher;

    protected override void Finish()
    {
        SpawnChasher();
        gameObject.SetActive(true);
        transform.parent = null;
        transform.position = new Vector3(0,-100,0);
    }

    private void SpawnChasher()
    {
        spawnedChasher = Instantiate(casher, this.transform.position, Quaternion.identity);
        //spawnedChasher.Initialize(attractions,takeTickets,casherConfig);
        upgradeWorkerField.gameObject.SetActive(true);
        upgradeWorkerField.Init(spawnedChasher.GetComponent<IWorker>());
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
