using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class SaveableEntity : MonoBehaviour
{
    [SerializeField, ReadOnly] string uniqueIdentifier = "";

    protected List<ISaveable> saveables = new();
    private static Dictionary<string, SaveableEntity> _globalLookup = new();

    private bool needToBeDeleted;

    private void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        if (needToBeDeleted)
            Delete();
    }

    protected virtual void OnAwake()
    {
        saveables = GetComponents<ISaveable>().ToList();
        SavingWrapper.SaveableEntities.Add(this);
    }

    public string GetUniqueIdentifier() => uniqueIdentifier;

    public void DeleteThis()
    {
        needToBeDeleted = true;
    }

    public void Delete()
    {
        saveables.Clear();
        SavingWrapper.SaveableEntities.Remove(this);
        Destroy(gameObject);
    }

    public JToken CaptureAsJToken()
    {
        var state = new JObject();
        IDictionary<string, JToken> stateDict = state;

        foreach (var saveable in saveables)
        {
            if (saveable == null)
                continue;
            var token = saveable.CaptureAsJToken();
            var component = saveable.GetType().ToString();
            stateDict[component] = token;
            //print($"{name} Capture {component} = {token}");
        }

        return state;
    }

    public void RestoreFromJToken(JToken s)
    {
        var state = s.ToObject<JObject>();
        IDictionary<string, JToken> stateDict = state;

        foreach (var saveable in saveables)
        {
            var component = saveable.GetType().ToString();
            if (stateDict.ContainsKey(component))
            {
                saveable.RestoreFromJToken(stateDict[component]);
                print($"{name} Restore {component} =>{stateDict[component]}");
            }
        }
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Application.IsPlaying(gameObject)) return; // If game is playing
        if (string.IsNullOrEmpty(gameObject.scene.path)) return; // If in prefab scene

        var serializedObject = new SerializedObject(this);
        var property = serializedObject.FindProperty("uniqueIdentifier");

        if (!IsValid(property.stringValue))
        {
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

        _globalLookup[property.stringValue] = this;
    }

#endif

    private bool IsValid(string candidate)
    {
        if (string.IsNullOrEmpty(candidate) || !IsUnique(candidate))
            return false;

        return true;
    }

    private bool IsUnique(string candidate)
    {
        if (!_globalLookup.ContainsKey(candidate) ||
            _globalLookup[candidate] == this)
            return true;

        if (_globalLookup[candidate] == null
            || _globalLookup[candidate].GetUniqueIdentifier() != candidate)
            _globalLookup.Remove(candidate);
        return true;
    }
}