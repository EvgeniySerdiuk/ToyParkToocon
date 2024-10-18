using System;
using ModestTree;
using Newtonsoft.Json.Linq;
using Project.Scripts.Arch.EventBus;
using UnityEngine;

#region ObjectDependency
[System.Serializable]
public class ObjectDependency
{
    public string Name;

    [SerializeField] private FieldByuUpgrade field;
    [SerializeField] private FieldByuUpgrade[] activateFields;
    [SerializeField] private GameObject[] activateObj;

    private Action<ObjectDependency> callback;
    public FieldByuUpgrade NextDependencyField => activateFields == null ? null : activateFields[0];
    public FieldByuUpgrade Field => field;
    
    public void SetCallback(Action<ObjectDependency> callback)
    {
        this.callback = callback;
    }
    
    public void Subscribes()
    {
        field.InstantiateObj += ActivateObj;
    }
    
    public void DeactivateObj()
    {
        if (activateObj.Length > 0)
        {
            foreach (var obj in activateObj)
            {
                obj.SetActive(false);
            }
        }

        if (activateFields!=null)
        {
            foreach (var obj in activateFields)
            {
                obj.LockField();
            }
        }
    }
    
    private void ActivateObj()
    {
        Activate();
        callback?.Invoke(this);
        callback = null;
    }

    public void ActivateObjectsWithoutAppMetrica()
    {
        if (activateObj.Length > 0)
        {
            foreach (var obj in activateObj)
            {
                obj.SetActive(true);
            }
        }

        if (activateFields != null)
        {
            foreach (var obj in activateFields)
            {
                obj.BuyField();
            }
        }

        field.InstantiateObj -= ActivateObj;
        callback = null;
    }
    
    public void ActivateObjects()
    {
        Activate();
        callback = null;
    }

    private void Activate()
    {
        if (activateObj.Length > 0)
        {
            foreach (var obj in activateObj)
            {
                obj.SetActive(true);
            }
        }

        if (activateFields != null)
        {
            foreach (var obj in activateFields)
            {
                obj.BuyField();
                obj.AppMetricaActivate();
            }
        }

        field.InstantiateObj -= ActivateObj;
    }
}
#endregion
public class ActivateObjScene : BaseEventTrigger
{
    [SerializeField] private ObjectDependency[] obj;
    
    [Header("AppMetrica settings"), Space (5)]
    [SerializeField] private int level_number;
    [SerializeField] private bool workFromStart;
    
    private int lastDepIndex;

    private void Start()
    {
        foreach (var obj in obj)
        {
            obj.Subscribes();
            obj.SetCallback(OnObjActivate);
        }

        for (int i = lastDepIndex+1; i < obj.Length; i++)
        {
            obj[i].DeactivateObj();
        }
    }
    

    private void OnObjActivate(ObjectDependency dep)
    {   
        lastDepIndex = obj.IndexOf(dep);
        if (IsLast(dep))
        {
            TriggerEnd();
            return;
        }
        Vector3 pos = Vector3.zero;
        if (dep.NextDependencyField != null)
            pos = dep.NextDependencyField.transform.position;
        EventOperatorData data = new EventOperatorData(cameraDelayOnFieldActivation, cameraDelayBeforeStartMoving, 
            dep.Field, pos);
        Trigger(data);
    }

    private bool IsLast(ObjectDependency dep)
    {
        return dep == obj[obj.Length - 1];
    }

    public override JToken CaptureAsJToken()
    {
        return JToken.FromObject(lastDepIndex);
    }

    public override void RestoreFromJToken(JToken state)
    {
        lastDepIndex = state.ToObject<int>();
        for (int i = 0; i <= lastDepIndex; i++)
        {
            obj[i].ActivateObjectsWithoutAppMetrica();
        }
        if(IsLast(obj[lastDepIndex]))
            TriggerEnd();
    }
}
