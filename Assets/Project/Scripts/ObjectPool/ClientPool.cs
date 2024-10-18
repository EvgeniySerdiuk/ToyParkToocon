using UnityEngine;

namespace Project.Scripts.ObjectPool
{
    public class ClientPool : ObjectsPool<Client>
    {
        public ClientPool(Client[] objects, int amountObjects, Transform transformParent) : base(objects, amountObjects, transformParent)
        {
            
        }

        public override void Remove(Client c)
        {
            c.gameObject.SetActive(false);
            c.IsFree = true;
        }

        public override Client Get()
        {
            foreach (var obj in objectPool)
            {
                if (obj.IsFree)
                    return GetConfigedClient(obj);
            }
            
            return GetConfigedClient(Create());
        }

        private Client GetConfigedClient(Client c)
        {
            c.gameObject.SetActive(true);
            c.IsFree = false;
            return c;
        }
        
        protected override Client Create()
        {
            var obj = GameObject.Instantiate(objects[Random.Range(0, objects.Length)],transformParent);
            objectPool.Add(obj);
            return obj;
        }
        
    }
}