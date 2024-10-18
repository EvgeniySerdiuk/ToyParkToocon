using UnityEngine;

public class TimeScaleObject : MonoBehaviour
{
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
