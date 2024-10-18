using DG.Tweening;
using UnityEngine;

public class RepulsingVisitors : MonoBehaviour
{
    public float pushForce = 0.1f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Visitor"))
        {
            Vector3 pushDirection = hit.moveDirection.normalized * pushForce;
            hit.transform.DOMove(hit.transform.position + pushDirection, 0.5f);
        }
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
