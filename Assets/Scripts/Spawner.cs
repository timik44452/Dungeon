using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prototype;

    private void Start()
    {
        if (transform.childCount == 0)
        {
            SceneUtility.CreateObject(prototype, transform.position, Quaternion.identity);
        }
        else
        {
            for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
            {
                SceneUtility.CreateObject(prototype, transform.GetChild(childIndex).position, Quaternion.identity);
            }
        }
    }
}
