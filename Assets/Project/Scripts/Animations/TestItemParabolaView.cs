using Project.Scripts.Interfaces;
using UnityEngine;

namespace Project.Scripts.Animations
{
    public class TestItemParabolaView : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private Vector3 start;
        [SerializeField] private Vector3 end;
        [SerializeField] private float height;
        [SerializeField] private float timer;
        [SerializeField] private AnimationCurve animationCurve;
        
        private ItemParabola parabola;

        private void Start()
        {
            parabola = new ItemParabola(this, animationCurve);
            // parabola.ParabolaAnimate(transform, start, end, height);
        }

        private void Callback(GameObject gO)
        {
            // parabola.ParabolaAnimate(transform, end, start, height, timer, (g) => g.gameObject.SetActive(false));
        }
    }
}