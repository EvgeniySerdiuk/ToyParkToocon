using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyContainer : MonoBehaviour
{
    [SerializeField] private float itemSpacing;
    [SerializeField] private Money moneyPrefab;
    private Transform parentTransform;
    private Bounds itemBound;
    private Bounds planeBound;
    private GameObject pointObject;
    private List<Transform> points = new List<Transform>();

    private void Awake()
    {
        parentTransform = this.transform;
        planeBound = parentTransform.GetComponent<Renderer>().bounds;
        itemBound = moneyPrefab.GetComponentInChildren<Renderer>().bounds;
        SpawnPoints();
    }

    private void SpawnPoints()
    {
        pointObject = new GameObject("Point");

        float startX = planeBound.min.x;
        float startZ = planeBound.min.z;
        float startY = planeBound.min.y;

        while (startY <= 0.5F)
        {
            while (startX <= planeBound.max.x)
            {
                while (startZ <= planeBound.max.z)
                {
                    var obj = GameObject.Instantiate(pointObject, new Vector3(startX, startY, startZ), Quaternion.identity, parentTransform);
                    points.Add(obj.transform);
                    startZ += itemBound.size.z + itemSpacing;
                }

                startZ = planeBound.min.z;
                startX += itemBound.size.x + itemSpacing;
            }
            startZ = planeBound.min.z;
            startX = planeBound.min.x;
            startY += itemBound.size.y + 0.05f;
        }
        GameObject.Destroy(pointObject);
    }

    public void AddContainer(Transform parentForItem)
    {
        parentTransform = parentForItem;
        planeBound = parentTransform.GetComponent<Renderer>().bounds;
        SpawnPoints();
    }

    public Transform GetPoint()
    {
        Transform point = points.FirstOrDefault(x => x.childCount == 0);

        return point;
    }

    public Money GetFirstMoney()
    {
        var obj = points.FirstOrDefault(x => x.childCount == 1).GetComponentInChildren<Money>();
        return obj;
    }
}


