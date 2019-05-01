using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Spikes : MonoBehaviour
{
    public Rigidbody Spikes;
    private Vector3 Movement;
    // Start is called before the first frame update
    void Start()
    {
        Spikes = GetComponent<Rigidbody>();
        StartCoroutine(Move());
        Movement = new Vector3();
    }

    IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            Movement = new Vector3(transform.position.x, (transform.position.y - 0.5f), transform.position.z);
            Spikes.MovePosition(Movement);
            yield return new WaitForSeconds(3);
            Movement = new Vector3(transform.position.x, (transform.position.y + 0.5f), transform.position.z);
            Spikes.MovePosition(Movement);
        }
    }
}
