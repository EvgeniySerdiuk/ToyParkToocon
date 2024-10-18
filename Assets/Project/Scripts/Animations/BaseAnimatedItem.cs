using System;
using UnityEngine;

namespace Project.Scripts.Animations
{
    public class BaseAnimatedItem : MonoBehaviour, IAnimatedItem
    {
        private ItemParabola parabola;
        
        public virtual void Config(ItemParabola parabola)
        {
            this.parabola = parabola;
        }

        public void ParabolaAnimate(Vector3 endPoint, float parabolaHeight, Transform targetTransform = null, Action callback = null)
        {
            parabola.ParabolaAnimate(transform,transform.position, endPoint, parabolaHeight, targetTransform, callback);
        }

        public void ParabolaAnimate(Vector3 localPosition, Action callback = null)
        {
            parabola.ParabolaAnimate(transform, localPosition, callback);
        }
    }
}