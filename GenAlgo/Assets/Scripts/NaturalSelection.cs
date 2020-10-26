using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NaturalSelection : MonoBehaviour
{
    public float PopulationNumber;
    public GameObject RabbitObject;
    public GameObject PredatorObject;
    public Renderer TerrainRender;

    private List<RabbitController> rabbits;

    private void Start()
    {
        rabbits = new List<RabbitController>();

        for (int i = 0; i < PopulationNumber; i++)
        {
            GameObject temp = Instantiate(RabbitObject, GetRandomPosition(), RabbitObject.transform.rotation, transform);
            rabbits.Add(temp.GetComponent<RabbitController>());
        }
    }

    private void Update()
    {
        if (rabbits.Count(o => o.gameObject.activeInHierarchy == true) == 10)
        {
            PredatorObject.SetActive(false);
            CreateGeneration();
            PredatorToStart();
        }
    }

    private void PredatorToStart()
    {
        PredatorObject.transform.position = TerrainRender.bounds.center;
        PredatorObject.SetActive(true);
    }

    private void SpawnGeneration()
    {
        for (int i = 0; i < PopulationNumber; i++)
        {
            rabbits[i].gameObject.SetActive(true);
            rabbits[i].gameObject.transform.position = GetRandomPosition();
        }
    }

    private void CreateGeneration()
    {
        List<RabbitController> Parents = TopTen();
        List<RabbitController> OldRabbits;
        List<int> randomIndexes;

        OldRabbits = rabbits.GetRange(Parents.Count, (int)PopulationNumber - Parents.Count - 1);
        foreach (RabbitController oldRabbit in OldRabbits)
        {
            randomIndexes = GetRandomIndexes(10);

            Crossover(
                Parents[randomIndexes[0]],
                Parents[randomIndexes[1]],
                oldRabbit);
        }

        SpawnGeneration();
    }

    private List<int> GetRandomIndexes(int ParentsAmount)
    {
        List<int> indexes = new List<int>
        {
            Random.Range(0, ParentsAmount),
            Random.Range(0, ParentsAmount)
        };

        while (indexes[0] == indexes[1])
        {
            indexes[1] = Random.Range(0, ParentsAmount);
        }

        Debug.Log($"{indexes[0]} : {indexes[1]}");

        return indexes;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 center = TerrainRender.bounds.center;
        float boundsX = TerrainRender.bounds.size.x;
        float boundsZ = TerrainRender.bounds.size.z;
        float newX = Random.Range(-boundsX / 2, boundsX / 2);
        float newZ = Random.Range(-boundsZ / 2, boundsZ / 2);

        Vector3 newPos = center + new Vector3(newX, transform.position.y + .5f, newZ);

        return newPos;
    }

    public void Crossover(RabbitController Mother, RabbitController Father, RabbitController ReplaceRabbit)
    {
        List<float> MotherGenes = Mother.GetGenes();
        List<float> FatherGenes = Father.GetGenes();
        List<float> NewParams = new List<float>();
        Vector3 NewColor;
        List<float> FirstParentGenes, SecondParentGenes;

        if (Random.value <= 0.5)
        {
            FirstParentGenes = MotherGenes;
            SecondParentGenes = FatherGenes;
        }
        else
        {
            FirstParentGenes = FatherGenes;
            SecondParentGenes = MotherGenes;
        }

        for (int i = 0; i < MotherGenes.Count / 2; i++)
        {
            Debug.Log($"First: {i} out of {MotherGenes.Count / 2}");
            NewParams.Add(FirstParentGenes[i]);
        }

        for (int i = MotherGenes.Count / 2; i < MotherGenes.Count; i++)
        {
            Debug.Log($"Second: {i} out of {MotherGenes.Count / 2}");
            NewParams.Add(SecondParentGenes[i]);
        }

        NewColor = (Mother.RabbitColor + Father.RabbitColor) / 2;
        ReplaceRabbit.ChangeParams(NewParams, NewColor);
    }

    public List<RabbitController> TopTen()
    {
        rabbits.Sort(delegate (RabbitController left, RabbitController right)
        {
            if (left.GetFitness() > right.GetFitness())
            {
                return -1;
            }
            else
            {
                return 1;
            }
        });

        return rabbits.Take(10).ToList();
    }
}
