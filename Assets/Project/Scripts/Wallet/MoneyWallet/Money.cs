using System;
using Project.Scripts.Animations;
using UnityEngine;

public class Money : MonoBehaviour, IAnimatedItem
{
    [SerializeField] private int amountMoney;

    private float delay;
    private Transform mainParent;

    public int AmountMoney => amountMoney;
    private ItemParabola parabola;
    private Action<Money, Vector3> Callback;

    public void Config(ItemParabola parabola, Action<Money, Vector3> callback = null)
    {
        mainParent = transform.parent;
        Config(parabola);
        Callback = callback;
    }
    
    public void Config(ItemParabola parabola)
    {
        this.parabola = parabola;
    }

    public void ParabolaAnimate(Vector3 endPoint, float parabolaHeight, Transform targetTransform = null, Action callback = null)
    {
        parabola.ParabolaAnimate(transform, transform.position, GetEndPosition(endPoint), 0.5f, callback: callback);
    }

    private void OnTriggerStay(Collider other)
    {
        delay += Time.deltaTime;

        if (delay < 0.4f) 
            return;
        
        if (other.TryGetComponent<PlayerTag>(out var tag))
        {
            delay = 0;                
            transform.parent = mainParent;
            Callback?.Invoke(this, other.transform.position);
        }
    }

    private Vector3 GetEndPosition(Vector3 targetPosition)
    {
        return new Vector3(targetPosition.x, targetPosition.y + 1.3f, targetPosition.z);
    }
    
    public void SetAmountMoney(int amount)
    {
        amountMoney = amount;
    }
    
}
