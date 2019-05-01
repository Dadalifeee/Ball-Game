using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move_Captivator : MonoBehaviour
{
    private Vector3 Target;
    private bool Traveling;
    [SerializeField]
    Transform Destination1;
    NavMeshAgent Captivator;

    [SerializeField]
    Transform Destination2;

    // Start is called before the first frame update
    void Start()
    {
        Captivator = this.GetComponent<NavMeshAgent>();
        StartCoroutine(SetDestination());
    }

    IEnumerator SetDestination()
    {
        Traveling = false;
        while (true)
        {
            Target = Destination2.transform.position;
            Captivator.SetDestination(Target);
            yield return new WaitForSeconds(3);
            Target = Destination1.transform.position;
            Captivator.SetDestination(Target);
            yield return new WaitForSeconds(3);
        }
    }
}
