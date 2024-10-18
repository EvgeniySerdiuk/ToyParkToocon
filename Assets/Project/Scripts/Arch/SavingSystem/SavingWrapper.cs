using System.Collections.Generic;
using UnityEngine;

public class SavingWrapper
{
    public static HashSet<SaveableEntity> SaveableEntities = new();
    public static HashSet<SaveableEntity> Wallet = new();

    private const string defaultSaveFile = "save";
    private const string walletSaveFile = "walletSave";
    private const string tutorialSaveFile = "TutorPath";
    private SavingSystem savingSystem;
    
    public SavingWrapper(SavingSystem savingSystem)
    {
        this.savingSystem = savingSystem;
    }

    public bool IsSaveExist()
    {
        return savingSystem.IsSaveExist(defaultSaveFile);
    }

    public void SaveWallet()
    {
        savingSystem.SaveWallet(walletSaveFile);
    }

    public void Save()
    {
        savingSystem.Save(defaultSaveFile);
        savingSystem.SaveWallet(walletSaveFile);
#if UNITY_EDITOR
        Debug.Log("Saved");
#endif
    }

    public void Load()
    {
        savingSystem.Load(defaultSaveFile);
        savingSystem.LoadWallet(walletSaveFile);
#if UNITY_EDITOR
        Debug.Log("Loaded");
#endif
    }

    public void DeleteSaveFile()
    {
        savingSystem.DeleteSaveFile(defaultSaveFile);
        savingSystem.DeleteSaveFile(walletSaveFile);
        savingSystem.DeleteSaveFile(tutorialSaveFile);
    }

    public void ClearList()
    {
        SaveableEntities.Clear();
        Wallet.Clear();
    }
}
