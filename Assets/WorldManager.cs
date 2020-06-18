using UnityEngine;

using System.Collections;
using System.Collections.Generic;


public class WorldManager : MonoBehaviour
{
    public GameObject plant;

    public Vector3 area = Vector3.one;

    public int plantCount = 100;

    private List<Plant> plants = new List<Plant>();

    private void Start()
    {
        StartCoroutine(FloraThread());
    }

    private IEnumerator FloraThread()
    {
        while (true)
        {
            if (plants.Count < plantCount)
                plants.Add(CreatePlant());

            yield return new WaitForSeconds(0.1F);
        }
    }

    private Plant CreatePlant()
    {
        GameObject plantGameObject = Instantiate(plant);

        plantGameObject.transform.position = GetRandomPosition();

        return plantGameObject.AddComponent<Plant>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, area);
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 random = new Vector3(Random.value - 0.5F, 0, Random.value - 0.5F);

        random.Scale(area);

        return random;
    }
}
