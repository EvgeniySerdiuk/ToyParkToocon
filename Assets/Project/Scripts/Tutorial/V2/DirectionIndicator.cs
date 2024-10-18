using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    [SerializeField] private Transform pointer;

    void Update()
    {
        transform.LookAt(pointer);
    }
}
