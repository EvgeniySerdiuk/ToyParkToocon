using System.Collections;
using UnityEngine;

public class MovementCamera
{
    private CameraMoveForTarget moveTargetComponent;
    private Camera camera;

    private Vector3 startPosition;

    public MovementCamera()
    {
        camera = Camera.main;
        moveTargetComponent = camera.GetComponent<CameraMoveForTarget>();
    }

    public void Move(Vector3 targetPosition, MonoBehaviour monoBehaviour)
    {
        startPosition = camera.transform.position;
        monoBehaviour.StartCoroutine(Movement(targetPosition));
    }

    IEnumerator Movement(Vector3 targetPosition)
    {
        moveTargetComponent.enabled = false;

        while (camera.transform.position != targetPosition)
        {
            var finishPos = targetPosition;
            finishPos.y = camera.transform.position.y;
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, finishPos, 10 * Time.deltaTime);
            yield return null;
        }

        yield return  new WaitForSeconds(1);

        while (camera.transform.position != startPosition)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, startPosition, 10 * Time.deltaTime);
            yield return null;
        }

        moveTargetComponent.enabled = true;
    }

}
