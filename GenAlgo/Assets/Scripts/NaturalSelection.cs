using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NaturalSelection : MonoBehaviour
{
    public float PopulationNumber;
    public GameObject RabbitObject;
    public Renderer TerrainRender;

    private List<RabbitController> rabbits;

    private void Start()
    {
        List<RabbitController> rabbits = new List<RabbitController>();

        for (int i = 0; i < PopulationNumber; i++)
        {
            GameObject temp = Instantiate(RabbitObject, GetRandomPosition(), Quaternion.identity, transform);
            rabbits.Add(temp.GetComponent<RabbitController>());
        }
    }

    private void Update()
    {
        if (rabbits.Count(o => o.gameObject.activeInHierarchy == true) == 10)
        {
            CreateGeneration();
        }
    }

    private void SpawnGeneration()
    {
        for (int i = 0; i < PopulationNumber; i++)
        {
            rabbits[i].gameObject.transform.position = GetRandomPosition();
        }
    }

    private void CreateGeneration()
    {
        List<RabbitController> Parents = TopTen();
        List<RabbitController> OldRabbits = new List<RabbitController>(2);
        List<int> randomIndexes;

        for (int i = Parents.Count; i < PopulationNumber; i += 2)
        {
            randomIndexes = GetRandomIndexes(10);

            OldRabbits[0] = rabbits[i];

            if (i != PopulationNumber - 1)
            {
                OldRabbits[1] = rabbits[i + 1];
            }
            else
            {
                OldRabbits.RemoveAt(1);
            }

            Crossover(
                Parents[randomIndexes[0]],
                Parents[randomIndexes[1]],
                OldRabbits);
        }

        SpawnGeneration();
    }

    private List<int> GetRandomIndexes(int ParentsAmount)
    {
        List<int> indexes = new List<int>();

        indexes[0] = Random.Range(0, ParentsAmount);
        indexes[1] = Random.Range(0, ParentsAmount);

        while (indexes[0] == indexes[1])
        {
            indexes[1] = Random.Range(0, ParentsAmount);
        }

        return indexes;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 center = TerrainRender.bounds.center;
        float boundsX = TerrainRender.bounds.size.x;
        float boundsZ = TerrainRender.bounds.size.z;
        float newX = Random.Range(-boundsX / 2, boundsX / 2);
        float newZ = Random.Range(-boundsZ / 2, boundsZ / 2);

        Vector3 newPos = center + new Vector3(newX, transform.position.y, newZ);

        return newPos;
    }

    public void Crossover(RabbitController Mother, RabbitController Father, List<RabbitController> ReplaceRabbits)
    {
        List<float> MotherGenes = Mother.GetGenes();
        List<float> FatherGenes = Father.GetGenes();
        List<float> NewParams = new List<float>(MotherGenes.Count);
        Vector3 NewColor;
        List<float> FirstParentGenes, SecondParentGenes;

        for (int j = 0; j < ReplaceRabbits.Count; j++)
        {
            if (j == 0)
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
                NewParams[i] = FirstParentGenes[i];
            }

            for (int i = MotherGenes.Count / 2; i < MotherGenes.Count; i++)
            {
                NewParams[i] = SecondParentGenes[i];
            }

            NewColor = (Mother.RabbitColor + Father.RabbitColor) / 2;
            ReplaceRabbits[j].ChangeParams(NewParams, NewColor);
        }
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
