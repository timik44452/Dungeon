using System.Collections;
using UnityEngine;

public class DamageTextEffect : MonoBehaviour
{
    public float duration = 1.0F;
    public float height = 1.0F;

    private TextMesh textMesh;
    private Vector3 startPosition;

    private void Start()
    {
        textMesh = GetComponentInChildren<TextMesh>();
        startPosition = transform.position;

        StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    { 
        for (float alpha = 0; alpha <= duration; alpha += 0.05F)
        {
            float delta = alpha / duration;

            transform.position = startPosition + Vector3.up * height * delta;
            
            Color color = textMesh.color;

            color.a = 1.0F - delta;

            textMesh.color = color;

            yield return new WaitForSeconds(0.05F * duration);
        }

        Destroy(gameObject);
    }
}
