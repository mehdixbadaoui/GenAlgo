using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    //TODO:
    //1. REWRITE RANDOM MOVEMENT
    //2. REWRITE TARGET ACQUIRING

    public float EatingRange;
    public float PerceptionRange;
    //public float SniffRange;
    public float Speed;
    private Vector3 random_direction;

    private Vector3 MovementDirection;

    void Start()
    {
        InvokeRepeating("changeRandom", 0, 2);
    }

    void Update()
    {
        
        MovementDirection = NextDirection();
        if (transform.position == random_direction) changeRandom();
        Move(MovementDirection);

        CheckPray();
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * Speed * Time.deltaTime;
    }

    private void changeRandom()
    {
        random_direction = new Vector3(Random.Range(-50f, 50f), transform.position.y, Random.Range(-50f, 50f));
    }

    private Vector3 NextDirection()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, PerceptionRange);
        Vector3 direction;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Prey"))
            {
                //TODO
                direction = (hit.transform.position - transform.position).normalized;
                return direction;
            }
        }

        direction = (random_direction - transform.position).normalized;
        Debug.Log(direction);
        return direction;
    }

    private void CheckPray()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, EatingRange);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Prey"))
            {
                hit.gameObject.SetActive(false);

                return;
            }
        }
    }
}
