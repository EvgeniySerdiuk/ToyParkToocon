using DG.Tweening;
using UnityEngine;

public class ArrowTutorial : MonoBehaviour
{
    [SerializeField] private Material materialPointer;
    [SerializeField] private Material materialDirectionIndicator;

    private Transform target;
    private float radiusCollider;

    private void Awake()
    {
        radiusCollider = GetComponentInParent<SphereCollider>().radius;
        materialDirectionIndicator.color = new Color(1, 1, 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        target = other.transform;
        materialDirectionIndicator.DOColor(new Color(1, 1, 1, 0), 0.65f);
    }

    private void OnTriggerStay(Collider other)
    {
        var distance = Vector3.Distance(transform.position, target.position);
        materialPointer.color = new Vector4(1, 1, 1, distance / radiusCollider);
    }

    private void OnTriggerExit(Collider other)
    {
        materialPointer.color = Vector4.one;
        materialDirectionIndicator.DOColor(new Color(1, 1, 1, 1), 0.65f);
    }
}
