using System;
using System.Collections;
using Project.Scripts.Interfaces;
using UnityEngine;

namespace Project.Scripts.Animations
{
    public class ItemParabola
    {
        private ParabolaMath parabolaMath;
        private ICoroutineRunner runner;
        private AnimationCurve curve;
        private float animationDuration;
        
        public ItemParabola(ICoroutineRunner runner, AnimationCurve curve)
        {
            parabolaMath = new ParabolaMath();
            this.runner = runner;
            this.curve = curve;
            animationDuration = curve.keys[curve.length - 1].time;
        }

        public void ParabolaAnimate(Transform t, Vector3 localPosition, Action callback = null)
        {
            runner.StartCoroutine(LocalAnimate(t, localPosition, callback));
        }

        private IEnumerator LocalAnimate(Transform t, Vector3 localPosition, Action callback = null)
        {
            float timer = 0f;
            while (Vector3.Distance(t.localPosition, localPosition) > 0.1f) 
            {
                timer += Time.deltaTime;
                t.transform.localPosition = Vector3.LerpUnclamped(t.transform.localPosition , localPosition, timer);
                yield return new WaitForEndOfFrame();
            }
            t.localPosition = localPosition;
            if(callback != null)
                callback.Invoke();
        }
        
        public void ParabolaAnimate(Transform transform, Vector3 start, Vector3 end,
            float height, Transform targetTransform = null, Action callback = null)
        {
            if (targetTransform == null)
                runner.StartCoroutine(ParabolaAnimate(transform, start, end, height, callback));
            else
                runner.StartCoroutine(LinearAnimate(transform, targetTransform, callback));
        }

        private IEnumerator ParabolaAnimate(Transform t, Vector3 start, Vector3 end, float height, Action callback = null)
        {
            float timer = 0f;
            while (timer < animationDuration)
            {
                float point = curve.Evaluate(timer);
                t.position = parabolaMath.GetPrabolaPointAtTime(start, end, height, point);
                timer +=  Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            if(callback != null)
                callback.Invoke();
        }

        private IEnumerator LinearAnimate(Transform t, Transform targetTransform, Action callback = null)
        {
            float timer = 0f;
            while (Vector3.Distance(t.position, targetTransform.position) > 0.1f) 
            {
                timer += Time.deltaTime;
                t.transform.position = Vector3.LerpUnclamped(t.position, targetTransform.position, timer);
                yield return new WaitForEndOfFrame();
            }
            if(callback != null)
                callback.Invoke();
        }
    }
}