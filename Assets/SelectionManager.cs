using LiveWorld.Mobs.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WorldManager))]
public class SelectionManager : MonoBehaviour
{
    public int codeLength = 50;
    public int populationCount = 50;

    public float speed;
    public float rotationStep;

    private List<TestMob> mobs = new List<TestMob>();


    private void Start()
    {
        StartCoroutine(CreatePopulation());
    }

    private void DestroyPopulation()
    {
        while (mobs.Count > 0)
        {
            mobs[0].DestroyMob();
            mobs.RemoveAt(0);
        }
    }

    private IEnumerator CreatePopulation()
    {
        for (int i = 0; i < populationCount; i++)
        {
            TestMob mob = CreateMob();

            mob.Run(RandomDNA(codeLength, 3));
            mobs.Add(mob);

            yield return new WaitForSeconds(0.1F);
        }
    }

    private TestMob CreateMob()
    {
        GameObject mobGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var mob = mobGO.AddComponent<TestMob>();

        mob.transform.position = GetComponent<WorldManager>().GetRandomPosition();
        mob.speed = speed;
        mob.stepRotate = rotationStep;

        return mob;
    }

    private DNA RandomDNA(int codeLenght, int codeCount)
    {
        byte[] code = new byte[codeLenght];

        for (int i = 0; i < codeLenght - 1; i++)
        {
            code[i] = (byte)Random.Range(0, codeCount);
        }

        DNA dna = new DNA(code);

        return dna;
    }
}
