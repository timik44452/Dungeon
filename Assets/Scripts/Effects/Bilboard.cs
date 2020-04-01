using UnityEngine;

public class Bilboard : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
