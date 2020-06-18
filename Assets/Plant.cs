using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour
{
    private float size = 1;
    private float growSpeed = 0.1F;

    private void Awake()
    {
        size = Random.Range(0.75F, 1F);
        growSpeed = Random.Range(0.001F, 0.01F);
         
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        for (float alpha = 0; alpha < 1F + growSpeed; alpha += growSpeed)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * size, alpha);

            yield return new WaitForSeconds(0.05F);
        }
    }
}
