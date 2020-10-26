using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NaturalSelection : MonoBehaviour
{
    public List<float> crosseover(List<float> mom, List<float> dad)
    {
        List<float> child = new List<float>();
        int crossAt = Random.Range(0, mom.Count - 1);
        for(int i = 0; i<= mom.Count - 1; i++)
        {
            if (i <= crossAt) child[i] = mom[i];
            else child[i] = dad[i];
        }

        return child;
    }

    public List<GameObject> topTen()
    {
        List<GameObject> topten = GameObject.FindGameObjectsWithTag("rabbit").ToList(); // need to add tag to rabbits
        topten = topten.OrderBy(o => o.GetComponent<RabbitController>().getFitness()).ToList();
        topten = topten.Take(10).ToList();


        return topten;
    }
}
