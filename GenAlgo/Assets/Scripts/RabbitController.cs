using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    /*todo
     * change params function
     * mvt rotation
     * borders
     */
    public float Stupidity; 
    public float StunPosibility; //done
    public float FearRate;
    public Vector3 RabbitColor; // (first + second) / 2
    public float Speed; //done
    public float Stamina; //done
    public float PerceptionRange; //done
    public float PerceptionChance; //done

    public float fitness;

    [SerializeField]
    private float StaminaBar;
    private GameObject predatorObject;

    private void Start()
    {
        StaminaBar = Stamina;
    }

    public List<float> GetGenes()
    {
        List<float> Genes = new List<float>()
        {
            Stupidity,
            StunPosibility,
            FearRate,
            Speed,
            Stamina,
            PerceptionRange,
            PerceptionChance
        };

        return Genes;
    }
    
    public void ChangeParams(List<float> NewGenes, Vector3 NewColor)
    {
        for (int i = 0; i < NewGenes.Count; i++)
        {
            //TODO
        }

        ChangeColor(NewColor);
    }

    private void ChangeColor(Vector3 NewColor)
    {
        GetComponent<MeshRenderer>().material.SetColor(0, new Color(NewColor.x, NewColor.y, NewColor.z));
    }

    public float GetFitness()
    {
        return fitness;
    }

    private void CalcFitness()
    {
        fitness += Time.deltaTime;
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

        CalcFitness();
    }

    private void MoveRandomly()
    {
        Vector3 direction = (transform.position - new Vector3(Random.Range(-1f, 1f), transform.position.y, Random.Range(-1f, 1f))).normalized;
        Vector3 newPos = transform.position + direction * Time.deltaTime;
        
        //Debug.Log($"direction = {direction}, newPos = {newPos}");
        //transform.LookAt(newPos);
        transform.position = newPos;
        //Debug.Log("Moving randomly...");

        StaminaBar = Mathf.Min(StaminaBar + Time.deltaTime, Stamina);
    }

    private bool CheckStress()
    {
        if (HasBeenSeen())
        {
            if (Random.Range(0f, 1f) <= StunPosibility)
            {
                StaminaBar = -0.2f;
                return false;
            }

            return true;
        }

        return false;
    }

    private void RunAway()
    {
        if(Vector3.Distance(predatorObject.transform.position, transform.position) > PerceptionRange)
        {
            //Debug.Log("Too far");
            return;
        }


        if (StaminaBar > 0)
        {
            Vector3 direction = (transform.position - predatorObject.transform.position).normalized;
            Vector3 newPos = transform.position + direction * Speed * Time.deltaTime;
            
            /*
            Vector3 targetDirection = predatorObject.transform.position - transform.position;
            float singleStep = Speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            // Debug.DrawRay(transform.position, newDirection, Color.red);
            var newRotation = Quaternion.LookRotation(newDirection);
            var xd = newRotation.eulerAngles;
            xd.z = 90f;
            newRotation = Quaternion.Euler(xd);
            transform.rotation = newRotation;
            */

            transform.position = newPos;
            StaminaBar = Mathf.Max(StaminaBar - Time.deltaTime, 0);
        }
        else
        {
            StaminaBar = -0.5f;
        }

        //Debug.Log("Running away!");
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

    public bool HasBeenSeen()
    {
        //TODO

        return true;
    }
}
