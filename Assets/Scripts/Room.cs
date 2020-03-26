using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector3 size = Vector3.one;

    public void BuildRoom()
    {
        CheckComponents();

        CreateMesh();
    }

    private void CreateMesh()
    {
        Vector3 up = size.z * 0.5F * Vector3.up;
        Vector3 right = size.z * 0.5F * Vector3.right;
        Vector3 forward = size.z * 0.5F * Vector3.forward;

        //Create inverse cube...
        Mesh inverseCube = new Mesh();

        List<int> triangles = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector3> vertices = new List<Vector3>();

        #region vertices
        vertices.Add(forward - right - up);
        vertices.Add(forward - right + up);
        vertices.Add(forward + right + up);
        vertices.Add(forward + right - up);
        #endregion

        #region triangles

        #endregion

        inverseCube.vertices = vertices.ToArray();
        inverseCube.triangles = triangles.ToArray();
        inverseCube.normals = normals.ToArray();

        GetComponent<MeshFilter>().sharedMesh = inverseCube;
    }

    private void CheckComponents()
    {
        AddIfNotComponent<MeshFilter>();
        AddIfNotComponent<MeshCollider>();
        AddIfNotComponent<MeshRenderer>();
    }

    private void AddIfNotComponent<T>() where T : Component
    {
        if (!GetComponent<T>())
        {
            gameObject.AddComponent<T>();
        }
    }
}
