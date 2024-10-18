using UnityEngine;

public class FPSController : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
