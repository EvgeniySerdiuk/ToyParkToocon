using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrashCan
{
    private MonoBehaviour coroutineRunner;   
    private int min;                         
    private int max;
    private bool filled;

    public bool IsClear => !filled;
    
    public event Action OnTrashCanFillEvent;
    public event Action OnTrashCanClearEvent;
    
    public TrashCan(MonoBehaviour coroutineRunner, int startMin, int startMax)
    {
        this.coroutineRunner = coroutineRunner;
        min = startMin;
        max = startMax;

        coroutineRunner.StartCoroutine(Loop(0));
    }
    
    public void Clear()
    {
        if (!filled)
            return;

        filled = false;
        OnTrashCanClearEvent?.Invoke();
        int duration = GetRandomTime();
        coroutineRunner.StartCoroutine(Loop(duration));
    }

    private IEnumerator Loop(int duration)
    {
        var endTime = Time.time + duration;
        while (Time.time < endTime)
            yield return new WaitForEndOfFrame();
        filled = true;
        OnTrashCanFillEvent?.Invoke();
    }

    private int GetRandomTime()
    {
        int time = Random.Range(min, max + 1);
        return time;
    }
}
