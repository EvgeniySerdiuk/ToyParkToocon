using UnityEngine;

namespace Project.Scripts.Animations
{
    public class ParabolaMath
    {
        public Vector3 GetPrabolaPointAtTime(Vector3 start, Vector3 end, float height, float time)
        {
            var mid = Vector3.Lerp(start, end, time);
            return new Vector3(mid.x, Fucn(height, time) + Mathf.Lerp(start.y, end.y, time), mid.z);
        }

        private float Fucn(float height, float time)
        {
            return -4 * height * time * time + 4 * height * time;
        }
    }
}