using System;
using UnityEngine;

namespace Project.Scripts.Animations
{
    public interface IAnimatedItem
    {
        public void Config(ItemParabola parabola);
        public void ParabolaAnimate(Vector3 endPoint, float parabolaHeight, Transform targetTransform = null, Action callback = null);
    }
}