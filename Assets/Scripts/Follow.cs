using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;

    public float speed = 1.0F;

    public Vector3 offset;

    private void FixedUpdate()
    {
        if(target == null)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, speed * Time.fixedDeltaTime);
    }
}
