using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Project.Scripts.Arch.EventBus;
using UnityEngine;

public class Tutorial : BaseEventTrigger
{
    [Space(10), Header("Tutorial Tasks")]
    [SerializeField] private TutorialView view;
    [SerializeField] private List<TutorialTask> tutorialTasks;

    private TutorialTask currentTask;
    private int currentTaskIndex;
    public event Action<TutorialTask> ChangeTask;
    public event Action TutorialComplete;

    private void Start()
    {
        SetTaskAndTriggerData();
    }

    private void SetTask()
    {
        if (tutorialTasks.Count == 0)
        {
            FinishTutorial();
            return;
        }

        currentTask = tutorialTasks[0];
        currentTask.Init();
        currentTask.Task.TaskComplete += CoroutineStart;
        currentTask.Task.StartTask();
        ChangeTask?.Invoke(currentTask);
    }

    private void CoroutineStart()
    {
        StopAllCoroutines();
        StartCoroutine(NextTaskWithDelay());
    }

    private IEnumerator NextTaskWithDelay()
    {
        AppMetricaReportEvent();
        currentTask.Task.TaskComplete -= CoroutineStart;

        yield return new WaitForSeconds(currentTask.Task.DelayForNextTask);
        
        currentTaskIndex++;
        tutorialTasks.Remove(currentTask);
        currentTask = null;

        if (tutorialTasks.Count > 0)
        {
            SetTaskAndTriggerData();
        }
        else
        {
            FinishTutorial();
        }
    }

    private void SetTaskAndTriggerData()
    {
        SetTask();
        TriggerWithData();
    }

    private void TriggerWithData()
    {
        if(currentTask == null || currentTask.Task.IsTaskComplete)
            return;
        Vector3 objectPoint = currentTask.Task.ObjectPoint;
        if (currentTask.taskType != TaskType.BuyLocation)
            objectPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1.0f));
        EventOperatorData data = new EventOperatorData(cameraDelayOnFieldActivation, cameraDelayBeforeStartMoving, null, objectPoint);
        Trigger(data);
    }

    private void FinishTutorial()
    {
        TutorialComplete?.Invoke();
        OnFinishEvents();
    }

    private void OnFinishEvents()
    {
        TriggerEnd();
        saveableEntity.DeleteThis();
        StopAllCoroutines();
    }

    public override JToken CaptureAsJToken()
    {
        return JToken.FromObject(currentTaskIndex);
    }

    public override void RestoreFromJToken(JToken state)
    {
        if (currentTaskIndex != 0)
            return;
        currentTaskIndex = state.ToObject<int>();
        for (int i = 0; i < currentTaskIndex; i++)
        {
            if (tutorialTasks[0].Task != null)
                tutorialTasks[0].Task.Complete();
            tutorialTasks.Remove(tutorialTasks[0]);
        }
    }

    private void AppMetricaReportEvent()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("step_name", currentTask.Name);
        AppMetrica.Instance.ReportEvent("tutorial", parameters);
        AppMetrica.Instance.SendEventsBuffer();
    }
}
