using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.InteractiveBuildings;
using UnityEngine;
using UnityEngine.AI;

public class Client : MonoBehaviour
{
    public bool IsFree { get; set; }

    private Animator animator;
    private NavMeshAgent agent;
    private List<HashSet<IInteractiveBuilding>> moveQueue;
    private Vector3 endPoint;

    private int index;
    
    public event Action<Client> OnEndEvent;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        moveQueue = new List<HashSet<IInteractiveBuilding>>();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("IsWalk", true);
    }

    public void SetEndPoint(Vector3 endPoint)
    {
        this.endPoint = endPoint;
    }
    
    public void AddInteractiveBuildingList(HashSet<IInteractiveBuilding> buildings)
    {
        moveQueue.Add(buildings);
    }
    
    public void MoveNextBuilding()
    {
        while (index < moveQueue.Count)
        {
            foreach (var building in moveQueue[index])
            {
                if (building.TryCheckPlaceInQueue())
                {
                    Move(building.ToGetInQueue(this));
                    index++;
                    return;
                }
            }
            index++;
        }

        StartCoroutine(GoToEnd());
    }

    public void Move(Vector3 targetPosition)
    {
        animator.SetBool("IsWalk",true);
        agent.destination = targetPosition;

        if (Vector3.Distance(agent.transform.position,targetPosition) < 0.05f)
        {
            animator.SetBool("IsWalk", false);
            animator.SetFloat("Blend", UnityEngine.Random.Range(0f, 1f));
            return;
        }

        agent.destination = targetPosition;
        StartCoroutine(CheckFinish());
    }

    private IEnumerator CheckFinish()
    {
        while(agent.hasPath == false) yield return null;

        while(agent.remainingDistance > 0.1)
        {
            yield return null;
        }

        animator.SetBool("IsWalk", false);
        animator.SetFloat("Blend", UnityEngine.Random.Range(0f, 1f));
    }

    private IEnumerator GoToEnd()
    {
        Move(endPoint);

        while (Vector3.Distance(transform.position, endPoint) > 0.5f)
        {
            yield return new WaitForFixedUpdate();
        }

        animator.SetFloat("Blend", UnityEngine.Random.Range(0f, 1f));
        index = 0;
        moveQueue.Clear();
        OnEndEvent?.Invoke(this);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
