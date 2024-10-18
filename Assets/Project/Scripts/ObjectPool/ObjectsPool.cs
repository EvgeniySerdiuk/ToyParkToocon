using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectsPool<T> where T : MonoBehaviour
{
    protected T[] objects;
    protected List<T> objectPool;
    protected Transform transformParent;

    public ObjectsPool(T[] objects, int amountObjects, Transform transformParent)
    {
        this.objects = objects;
        objectPool = new List<T>();
        this.transformParent = transformParent;

        for (int i = 0; i < amountObjects; i++)
        {
            var obj = Create();
            obj.gameObject.SetActive(false);
        }
    }

    public virtual void Remove(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    public virtual T Get()
    {
        //var obj = objectPool.FirstOrDefault(x=>!x.isActiveAndEnabled);
        var obj = objectPool.FirstOrDefault(x => x != null && !x.isActiveAndEnabled);

        if (obj == null)
        {
            obj = Create();
        }

        // Проверьте, что объект не является null перед использованием
        if (obj != null)
        {
            obj.gameObject.SetActive(true);
        }
        return obj;
    }

    protected virtual T Create()
    {
        var obj = GameObject.Instantiate(objects[Random.Range(0, objects.Length)],transformParent);
        objectPool.Add(obj);
        return obj;
    }

}
