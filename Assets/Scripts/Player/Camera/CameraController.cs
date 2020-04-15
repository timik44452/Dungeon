using UnityEngine;

public class CameraController : MonoBehaviour
{     
    public Transform target;
    
    public float distance = 1.0F;

    public float minDistance = 1.0F;
    public float maxDistance = 1.0F;

    public float zoomSpeed = 1.0F;
    public float rotationSpeed = 1.0F;

    private Vector3 postiion;
    private Quaternion rotation;
    
    private TargetSystem targetSystem;

    private float x = 0.0F;
    private float y = 0.0F;

    private void Start()
    {
        targetSystem = FindObjectOfType<TargetSystem>();    
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, postiion, 10F * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10F * Time.deltaTime);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * rotationSpeed;
            y += Input.GetAxis("Mouse Y") * rotationSpeed;
        }

        if (targetSystem && !TargetSystem.ITargetIsNull(targetSystem.Target) && targetSystem.TargetFix)
        {
            Quaternion _rotation = Quaternion.LookRotation(targetSystem.Target.transform.position - transform.position);

            x = _rotation.eulerAngles.y;
        }

        y = Mathf.Clamp(y, -89, -5);

        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        float localDistance = distance;

        if (Physics.Raycast(target.position, transform.position - target.position, out RaycastHit hit, maxDistance))
        {
            localDistance = Mathf.Min(localDistance, hit.distance * 0.8F);
        }

        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        localDistance = Mathf.Clamp(localDistance, minDistance, maxDistance);

        rotation = Quaternion.Euler(-y, x, 0);
        postiion = target.position + rotation * new Vector3(0, 0, -localDistance);
    }
}
