using System.Collections;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using Project.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class VibrationHandler : IEventReceiver<VibrateEvent>, IEventReceiver<LongVibrateEventStart>, IEventReceiver<LongVibrateEventEnd>
{
    public AndroidJavaClass unityPlayer;
    public AndroidJavaObject currentActivity;
    public AndroidJavaObject vibrator;

    private ICoroutineRunner runner;
    private long vibrationLength = 100L;
    private float gapBetweenVibrations = 0.25f;
    private Coroutine currentCoroutine;
    
    public VibrationHandler(IEventBus bus, ICoroutineRunner runner, long vibrationLength, float gapBetweenVibrations)
    {
        Subscribe(bus);
        this.runner = runner;
        this.vibrationLength = vibrationLength;
        this.gapBetweenVibrations = gapBetweenVibrations;

#if UNITY_ANDROID && !UNITY_EDITOR
     unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
     currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
     vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif
    }

    private void Subscribe(IEventBus bus)
    {
        bus.Register<VibrateEvent>(this);
        bus.Register<LongVibrateEventStart>(this);
        bus.Register<LongVibrateEventEnd>(this);
    }
    
    private void StartLongVibrate()
    {
        currentCoroutine = runner.StartCoroutine(LongVibrate());
    }

    private void EndLongVibrate()
    {
        if(currentCoroutine != null)
            runner.StopCoroutine(currentCoroutine);
    }
    
    private IEnumerator LongVibrate()
    {
        while (true)
        {
            Vibrate(vibrationLength);
            yield return new WaitForSeconds(gapBetweenVibrations);
        }
    }
    
    public void Vibrate()
    {
        if (isAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }


    public void Vibrate(long milliseconds)
    {
        if (isAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    public void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }

    public bool HasVibrator()
    {
        return isAndroid();
    }

    public void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }

    public void OnEvent(VibrateEvent @event)
    {
        Vibrate(vibrationLength);
    }

    public void OnEvent(LongVibrateEventStart @event)
    {
        StartLongVibrate();
    }

    public void OnEvent(LongVibrateEventEnd @event)
    {
        EndLongVibrate();
    }
}