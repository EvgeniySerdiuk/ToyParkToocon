using System.Collections;
using System.Linq;
using Project.Scripts.Interfaces;
using Project.Scripts.ObjectPool;
using UnityEngine;

public class SpawnClients 
{
    private Transform[] spawnPoints;
    private Transform[] endPoints; 
    private InteractiveBuildingRegister buildingRegister;
    private ICoroutineRunner coroutineRunner;
    private ObjectsPool<Client> clientsPool;
    private Camera mainCamera;    
    private int locations;

    public SpawnClients(Transform clientsParentTransform, Transform[] spawnPoints, Transform[] endPoints, Client[] clients, 
        InteractiveBuildingRegister buildingRegister, ICoroutineRunner coroutineRunner)
    {
        clientsPool = new ClientPool(clients, 0, clientsParentTransform);
        mainCamera = Camera.main;
        this.spawnPoints = spawnPoints;
        this.endPoints = endPoints;
        this.buildingRegister = buildingRegister;
        this.coroutineRunner = coroutineRunner;
    }

    public void SpawnClientsInLocation(int count)
    {
        locations += count;
        if (locations == count)
            coroutineRunner.StartCoroutine(SpawnClientsAsync());
    }

    private IEnumerator SpawnClientsAsync()
    {
        while (locations > 0)
        {
            var client = clientsPool.Get();
            client.OnEndEvent += ReleaseClient;
            ConfigureClient(client);
            client.MoveNextBuilding();
            locations--;
            yield return new WaitForSeconds(0.4f);
        }
    }
    
    private void ReleaseClient(Client client)
    {
        client.OnEndEvent -= ReleaseClient;
        clientsPool.Remove(client);
    }

    private void ConfigureClient(Client client)
    {
        var buildingsInLocation = buildingRegister.GetBuildingsInLocation();
        for (int i = 0; i < buildingsInLocation.Length; i++)
            client.AddInteractiveBuildingList(buildingsInLocation[i]);
        client.SetEndPoint(GetEndPoint());
        client.transform.position = GetCurrectSpawnPoint();
    }

    private Vector3 GetCurrectSpawnPoint()
    {
        var randomInvisiblePoint 
            = spawnPoints
                .Where(p => p.gameObject.activeInHierarchy == true && IsPointInvisible(p.position))
                .OrderBy(p => Random.value)
                .FirstOrDefault().position;

        return randomInvisiblePoint;
    }

    private Vector3 GetEndPoint()
    {
        var point 
            = endPoints.Where(p => p.gameObject.activeInHierarchy)
                .OrderBy(p => Random.value)
                .FirstOrDefault().position;
        return point;
    }

    private bool IsPointInvisible(Vector3 point)
    {
        // Получаем позицию точки в координатах экрана
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(point); 
        /// Проверяем, находится ли точка в пределах видимой области экрана
        return viewportPoint.x < 0 
               || viewportPoint.x > 1 
               || viewportPoint.y < 0 
               || viewportPoint.y > 1 
               || viewportPoint.z <= 0;
    }
}
