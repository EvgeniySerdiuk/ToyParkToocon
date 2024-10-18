using System;
using UnityEngine;

[Serializable]
public abstract class Task 
{
    public bool IsTaskComplete { get; private set; }
    
    public event Action TaskComplete;
    public abstract Vector3 ObjectPoint { get; }
    [field: SerializeField, Range(0, 5)] public float DelayForNextTask { get; protected set; }
    public abstract void StartTask();
    public virtual void Complete()
    {
        IsTaskComplete = true;
        TaskComplete?.Invoke();
    }
}
