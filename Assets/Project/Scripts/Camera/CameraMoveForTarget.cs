using System;
using System.Collections;
using Project.Scripts.Arch.EventBus.Events;
using Project.Scripts.Arch.EventBus.Interfaces;
using UnityEngine;
using Zenject;

public class CameraMoveForTarget : MonoBehaviour, IEventReceiver<MoveCameraEvent>
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private int speed;

    [SerializeField, Range(0,1)] private float speedPercentOnCameraEvent = 0.75f;
    [SerializeField, Range(0, 1)] private float offsetPercentOnCameraEvent = 0.5f;
    private bool movePlayer = true;
    private IEventBus bus;
    private Vector3 customOffset;
    private float customSpeed;
    
    [Inject]
    private void Init(IEventBus bus)
    {
        this.bus = bus;
        bus.Register(this);
        customOffset = offset * offsetPercentOnCameraEvent;
        customSpeed = speed * speedPercentOnCameraEvent;
    }

    private void OnDestroy()
    {
        bus.Unregister(this);
    }

    private void LateUpdate()
    {
        if(movePlayer)
            transform.position = Vector3.MoveTowards(transform.position, target.position + offset, speed *  Time.deltaTime);
    }

    public void OnEvent(MoveCameraEvent @event)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPoint( @event.TargetPosition, @event.FinishCallbackDelay, @event.Callback));
    }

    private IEnumerator MoveToPoint(Vector3 pos, float delay, Action callback)
    {
        movePlayer = false; 
        Vector3 targetPosition = pos + customOffset;
        while (Vector3.Distance(transform.position, targetPosition) > 0.25f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, customSpeed *  Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(delay);
        movePlayer = true;
        callback?.Invoke();
    }
}
