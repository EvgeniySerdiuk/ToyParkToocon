using Newtonsoft.Json.Linq;
using Project.Scripts;
using UnityEngine;

public class UpdateField : BuyField
{
    //protected override void Start()
    //{
    //    price = interactivebuilding[0].UpgradePrice;
    //    amountExpSpawn = interactivebuilding[0].AmountExp;
    //    base.Start();
    //}

    //protected override void FinishFiling()
    //{
    //    GiveExperience();
    //    Upgrade();
    //    GlobalEvents.InteractionFieldFinishFilling();
    //    InvokeFinishFilling();
    //}

    //public void Upgrade()
    //{
    //    UpgradeItems();
    //    if (IsMaxLvl())
    //    {
    //        transform.parent = null;
    //        transform.position = new Vector3(0,-100,0);
    //        return;
    //    }
    //    UpdateUI();
    //}
    
    //private void UpgradeItems()
    //{
    //    foreach(var item in interactivebuilding) 
    //    {
    //        item.Upgrade();
    //    }
    //}

    //private bool IsMaxLvl()
    //{
    //    return interactivebuilding[0].CurrentLevel == interactivebuilding[0].MaxLevel;
    //}

    //private void UpdateUI()
    //{
    //    price = interactivebuilding[0].UpgradePrice;
    //    textPrice.text = price.ToString();
    //    amountExpSpawn = interactivebuilding[0].AmountExp;
    //    ResetProgressBar();
    //}
    
    //public override JToken CaptureAsJToken()
    //{
    //    return null;
    //}

    //public override void RestoreFromJToken(JToken state)
    //{
    //}
}
