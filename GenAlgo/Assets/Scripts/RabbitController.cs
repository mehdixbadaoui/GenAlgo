using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    public float Stupidity; 
    public float StunPosibility; //done
    public float FearRate;
    public float RabbitColorR;
    public float RabbitColorG;
    public float RabbitColorB; // (first + second) / 2
    public float Speed; //done
    public float Stamina; //done
    public float PerceptionRange; //done
    public float PerceptionChance; //done
    private float StaminaBar;
    private GameObject predatorObject;

    public List<float> GetGenes()
    {
        List<float> Genes = new List<float>()
        {
            Stupidity,
            StunPosibility,
            FearRate,
            RabbitColorR,
            RabbitColorG,
            RabbitColorB,
            Speed,
            Stamina,
            PerceptionRange,
            PerceptionChance
        };

        return Genes;
    }

    private void Update()
    {
        if (CheckPreStress())
        {
            if (CheckStress())
            {
                RunAway();

                return;
            }
        }
        else
        {
            MoveRandomly();
        }

        StaminaBar = Mathf.Min(StaminaBar + Time.deltaTime, Stamina);
    }

    private void MoveRandomly()
    {
        Vector3 direction = (transform.position - new Vector3(Random.Range(-1f, 1f), transform.position.y, Random.Range(-1f, 1f))).normalized;
        Vector3 newPos = transform.position + direction * Time.deltaTime;
        
        Debug.Log($"direction = {direction}, newPos = {newPos}");
        transform.LookAt(newPos);
        transform.position = newPos;
        Debug.Log("Moving randomly...");
    }

    private bool CheckStress()
    {
        //float a = Random.Range(0f, 1f);
        if (Random.Range(0f, 1f) <= StunPosibility)
        {
            //Debug.Log($"Stunned {a} <= {StunPosibility}");
            return false;
        }

        return true;
    }

    private void RunAway()
    {
        if(Vector3.Distance(predatorObject.transform.position, transform.position) > PerceptionRange)
        {
            //Debug.Log("Too far");
            return;
        }

        Vector3 direction = (transform.position - predatorObject.transform.position).normalized;

        if (StaminaBar > 0)
        {
            Vector3 newPos = transform.position + direction * Speed * Time.deltaTime;

            transform.LookAt(newPos);
            transform.position = newPos;
            StaminaBar = Mathf.Max(StaminaBar - Time.deltaTime, 0);
        }
        else
        {
            StaminaBar = Mathf.Min(Stamina + Time.deltaTime, Stamina);
        }

        Debug.Log("Running away!");
    }

    private bool CheckPreStress()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, PerceptionRange);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Predator"))
            {
                if (Random.Range(0f, 1f) <= PerceptionChance)
                {
                    predatorObject = hit.gameObject;
                    return true;
                }
            }
        }

        return false;
    }

    public void HasBeenSeen()
    {

    }
}
