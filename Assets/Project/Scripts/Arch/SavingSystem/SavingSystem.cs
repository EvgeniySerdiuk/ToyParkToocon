using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SavingSystem
{
    private SavingStrategy strategy;

    public SavingSystem(SavingStrategy strategy)
    {
        this.strategy = strategy;
    }

    public bool IsSaveExist(string saveFile)
    {
        return strategy.IsSaveExist(saveFile);
    }

    public void SaveWallet(string saveFile)
    {
        JObject state = LoadJsonFromFile(saveFile);
        CaptureAsTokenWallet(state);
        SaveFileAsJSon(saveFile, state);
    }
    
    public void Save(string saveFile)
    {
        JObject state = LoadJsonFromFile(saveFile);
        CaptureAsToken(state);
        SaveFileAsJSon(saveFile, state);
    }

    public void LoadWallet(string saveFile)
    {
        RestoreWalletFromToken(LoadJsonFromFile(saveFile));
    }
    
    public void Load(string saveFile)
    {
        RestoreFromToken(LoadJsonFromFile(saveFile));
    }

    public void DeleteSaveFile(string saveFile)
    {
        var path = GetPathFromSaveFile(saveFile);
        if (!File.Exists(path)) 
            return;
#if UNITY_EDITOR
        Debug.Log($"Save file: {saveFile} deleted");
#endif
        File.Delete(path);
    }

    private void SaveFileAsJSon(string saveFile, JObject state)
    {
        strategy.SaveToFile(saveFile, state);
    }

    private JObject LoadJsonFromFile(string saveFile)
    {
        return strategy.LoadFromFile(saveFile);
    }

    private void CaptureAsTokenWallet(JObject state)
    {
        IDictionary<string, JToken> stateDict = state;
        foreach (var saveable in SavingWrapper.Wallet)
        {
            stateDict[saveable.GetUniqueIdentifier()] = saveable.CaptureAsJToken();
        }
    }
    
    private void CaptureAsToken(JObject state)
    {
        IDictionary<string, JToken> stateDict = state;
        foreach (var saveable in SavingWrapper.SaveableEntities)
        {
            stateDict[saveable.GetUniqueIdentifier()] = saveable.CaptureAsJToken();
        }
    }

    private void RestoreWalletFromToken(JObject state)
    {
        IDictionary<string, JToken> stateDict = state;
        int startCount = SavingWrapper.Wallet.Count;
        foreach (var saveable in SavingWrapper.Wallet)
        {
            var id = saveable.GetUniqueIdentifier();
            if (stateDict.ContainsKey(id))
                saveable.RestoreFromJToken(stateDict[id]);
            if (SavingWrapper.Wallet.Count != startCount)
            {
                RestoreWalletFromToken(state);
                return;
            }
        }
    }
    
    private void RestoreFromToken(JObject state)
    {
        IDictionary<string, JToken> stateDict = state;
        int startCount = SavingWrapper.SaveableEntities.Count;
        foreach (var saveable in SavingWrapper.SaveableEntities)
        {
            var id = saveable.GetUniqueIdentifier();
            if (stateDict.ContainsKey(id))
                saveable.RestoreFromJToken(stateDict[id]);
            if (SavingWrapper.SaveableEntities.Count != startCount)
            {
                RestoreFromToken(state);
                return;
            }
        }
    }

    private string GetPathFromSaveFile(string saveFile)
    {
        return strategy.GetPathFromSaveFile(saveFile);
    }
}
