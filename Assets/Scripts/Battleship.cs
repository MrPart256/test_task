using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Battleship : MonoBehaviour
{

    private NavMeshAgent agent;
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void MoveToTarget(Transform target)
    {
        StartCoroutine(MoveToTargetCoroutine(target));
    }

    private IEnumerator MoveToTargetCoroutine(Transform target)
    {
        agent.SetDestination(target.position);
        yield return WaitUntilReachedDesiredPlanet(target);
        
        InteractWithPlanet(target);

    }
    private void InteractWithPlanet(Transform target)
    {
        Debug.Log("a");
        target.GetComponent<Planet>().RecieveBattleShip(1);
        Destroy(this.gameObject);
    }
   WaitUntil WaitUntilReachedDesiredPlanet(Transform target)
    {
        return new WaitUntil(() => Vector2.Distance(transform.position,target.position)<=1);
    }
}
