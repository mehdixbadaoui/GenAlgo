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
    public MeshRenderer TerrainTexture;
    private Vector3 random_direction;

    private Vector3 MovementDirection;

    void Start()
    {
        InvokeRepeating("changeRandom", 0, 2);
    }

    void Update()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        
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
        Collider hit;
        float probability = 0f;
        float maxProbability = 0f;
        int index = -1;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            hit = hitColliders[i];
            if (hit.CompareTag("Prey"))
            {
                Color difference = TerrainTexture.material.color - hit.GetComponent<MeshRenderer>().material.color;
                probability = (difference.r + difference.g + difference.b) / Vector3.Distance(transform.position, hit.transform.position);
                if (probability > maxProbability)
                {
                    maxProbability = probability;
                    index = i;
                }
            }

        }

        if (index != -1)
        {
            direction = (hitColliders[index].transform.position - transform.position).normalized;
        }
        else
        {
            direction = (random_direction - transform.position).normalized;
        }

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
