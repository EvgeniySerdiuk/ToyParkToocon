using NaughtyAttributes;
using Project.Scripts.Interfaces;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

public class CharacterCharacteristics : MonoBehaviour , IUpgradable
{
    public const string sheetId = "1EU9ippL2o0zxc1mvE1niwTeWwTbPfqpL4QL92N5j5cM";

    [SerializeField] private float rotationSpeed;

    [SerializeField] private Characteristics characteristics;

    private UIUpgradeFactory upgradeFactory;
    public float RotationSpeed => rotationSpeed;

    public Characteristics Characteristics => characteristics;

    [Inject]
    private void Init(UIUpgradeFactory uIUpgradeFactory)
    {
        upgradeFactory = uIUpgradeFactory;
    }

    private void Start()
    {
        characteristics.Init();
    }

    public void Upgrade()
    {
        upgradeFactory.GetUIUpgrade(characteristics,true);
    }

    [Button]
    public void DownloadCharacteristics()
    {
        characteristics.DownloadData(sheetId);
    }

    public JToken CaptureAsJToken()
    {
        return null;
    }

    public void RestoreFromJToken(JToken state)
    {
        
    }
}
