using UnityEngine;
using System.Collections.Generic;

public class InfinityRoom : MonoBehaviour
{
    public GameObject column;

    public float viewDistance = 24.0F;
    public float distanceBetweenColumn = 4.0F;

    private float columnDelta = 0;

    private Dictionary<Vector3, GameObject> columns;

    private void Start()
    {
        columns = new Dictionary<Vector3, GameObject>();

        foreach (Vector3 point in GetPoints())
        {
            columns.Add(point, Instantiate(column, point, Quaternion.identity));
        }

        columnDelta = distanceBetweenColumn * Mathf.Sqrt(2);
    }

    private void FixedUpdate()
    {
        if(SceneUtility.Player == null)
        {
            return;
        }

        Vector3 offset = SceneUtility.Player.transform.position;

        offset.x = Mathf.Round(offset.x / columnDelta) * columnDelta;
        offset.z = Mathf.Round(offset.z / columnDelta) * columnDelta;
        offset.y = 0;

        foreach (var column in columns)
        {
            column.Value.transform.position = column.Key + offset;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 point in GetPoints())
        {
            Gizmos.DrawLine(point, point + Vector3.up);
        }
    }

    private IEnumerable<Vector3> GetPoints()
    {
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

        for (float x = distanceBetweenColumn * 0.5F; x < viewDistance * 0.5F; x += distanceBetweenColumn)
        {
            for (float z = distanceBetweenColumn * 0.5F; z < viewDistance * 0.5F; z += distanceBetweenColumn)
            {
                yield return rotation * new Vector3(x, 0, z);
                yield return rotation * new Vector3(-x, 0, z);
                yield return rotation * new Vector3(x, 0, -z);
                yield return rotation * new Vector3(-x, 0, -z);
            }
        }
    }
}
