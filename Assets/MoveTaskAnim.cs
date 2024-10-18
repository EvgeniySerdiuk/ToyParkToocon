using System;
using UnityEngine;

public class MoveTaskAnim : MonoBehaviour
{
    private Vector3 startPosition;
    public event Action AnimCompliete;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if(startPosition != transform.position)
        {
            AnimCompliete?.Invoke();
        }
    }
}
