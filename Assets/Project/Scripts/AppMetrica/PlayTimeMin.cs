using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Project.Scripts;
using Project.Scripts.Interfaces;
using UnityEngine;
using Zenject;

public class PlayTimeMin : MonoBehaviour, ISaveable, IHaveLastBreath
{
    private int time;
    private int session_number;
    private int session_duration;

    private float duration;

    private float currentTime;

    [Inject]
    private void Init(LastBreathWrapper lastBreathWrapper)
    {
        lastBreathWrapper.Add(this);
    }

    private void Start()
    {
        session_number++;
    }

    private void Update()
    {
        duration += Time.deltaTime;
        currentTime += Time.deltaTime;

        if(currentTime > 60.5f)
        {
            currentTime = 0;
            LastBreath();
        }
    }

    public void LastBreath()
    {
        session_duration = (int)(duration / 60);
        time += session_duration;
        ReportEvent();
    }

    private void ReportEvent()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("time", time.ToString());
        parameters.Add("session_number", session_number.ToString());
        parameters.Add("session_duration", session_duration.ToString());

        AppMetrica.Instance.ReportEvent("playtime_min", parameters);
        AppMetrica.Instance.SendEventsBuffer();
    }

    public JToken CaptureAsJToken()
    {
        JToken[] tokens = new JToken[2];
        tokens[0] = JToken.FromObject(session_number);
        tokens[1] = JToken.FromObject(time);
        return JToken.FromObject(tokens);
    }

    public void RestoreFromJToken(JToken state)
    {
        JToken[] tokens = state.ToObject<JToken[]>();
        session_number = tokens[0].ToObject<int>();
        time = tokens[1].ToObject<int>();
    }
}
