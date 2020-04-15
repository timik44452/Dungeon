using Game.Service;
using Game.Terrain.Service;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    #region Loaded data
    private Mesh mesh;
    private LoadedTerrainRegion loadedTerrain;
    private LoadedWorldObjects loadedWorldObjects;

    private Vector3[,] points = new Vector3[0, 0];
    #endregion

    public void InitializeChunk(Region region)
    {
        InitializeLoader();
        UpdateChunk(region);
    }

    private void InitializeLoader()
    {
        if (mesh == null)
        {
            mesh = new Mesh();

            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        if (loadedTerrain == null)
        {
            loadedTerrain = new LoadedTerrainRegion();
        }

        if (loadedWorldObjects == null)
        {
            loadedWorldObjects = new LoadedWorldObjects();
        }
    }

    public void UpdateChunk(Region region)
    {
        int region_width = (int)region.Width + 1;
        int region_length = (int)region.Length + 1;

        if (points.GetLength(0) < region.Width || points.GetLength(1) < region.Length)
        {
            //types = new int[region_width, region_length];
            points = new Vector3[region_width, region_length];
        }

        for (int x = 0; x < region_width; x++)
            for (int z = 0; z < region_length; z++)
            {
                loadedTerrain.GetBlockData(region.X + x, region.Z + z, out float height, out int type);

                //types[x, z] = type;
                points[x, z] = new Vector3(x, height * region.Height, z);
            }

        foreach (var reg in loadedTerrain.loadedRegions)
        {
            reg.SaveAsImage($"{reg.X}_{reg.Z}.png");
        }

        CreatePolyMesh(region);
    }

    private void CreatePolyMesh(Region region)
    {
        int childIndex = 0;
        int vertices_count = 0;
        int triangles_count = 0;

        int resolution = (int)(region.Width * region.Length + 0.5F);

        #region Mesh data
        int[] triangles = new int[resolution * 6];
        Vector2[] uvs = new Vector2[resolution * 6];
        Vector3[] normals = new Vector3[resolution * 6];
        Vector3[] vertices = new Vector3[resolution * 6];
        #endregion

        for (int x = 0; x < region.Width; x++)
        {
            for (int z = 0; z < region.Length; z++)
            {
                Vector3 v3 = points[x, z];
                Vector3 v2 = points[x, z + 1];
                Vector3 v1 = points[x + 1, z + 1];
                Vector3 v0 = points[x + 1, z];

                Vector3 normal = Vector3.Cross(v1 - v2, v3 - v2);

                #region Normals
                normals[vertices_count + 0] = normal;
                normals[vertices_count + 1] = normal;
                normals[vertices_count + 2] = normal;

                normals[vertices_count + 3] = normal;
                normals[vertices_count + 4] = normal;
                normals[vertices_count + 5] = normal;
                #endregion

                #region Verts
                vertices[vertices_count + 5] = v0;
                vertices[vertices_count + 4] = v1;
                vertices[vertices_count + 3] = v3;

                vertices[vertices_count + 2] = v1;
                vertices[vertices_count + 1] = v2;
                vertices[vertices_count + 0] = v3;
                #endregion

                #region Tris
                triangles[triangles_count + 0] = vertices_count;
                triangles[triangles_count + 1] = vertices_count + 1;
                triangles[triangles_count + 2] = vertices_count + 2;

                triangles[triangles_count + 3] = vertices_count + 3;
                triangles[triangles_count + 4] = vertices_count + 4;
                triangles[triangles_count + 5] = vertices_count + 5;
                #endregion

                vertices_count += 6;
                triangles_count += 6;

                childIndex++;
            }
        }

        mesh.Clear();

        mesh.SetVertices(vertices, 0, vertices_count);
        mesh.SetNormals(normals, 0, vertices_count);
        mesh.SetUVs(0, uvs, 0, vertices_count);
        mesh.SetTriangles(triangles, 0, triangles_count, 0);

        //mesh.RecalculateBounds();
        //mesh.RecalculateNormals();

        ////TODO: Fix it
        //if (gameObject.GetComponent<MeshCollider>())
        //{
        //    Destroy(gameObject.GetComponent<MeshCollider>());
        //}

        //gameObject.AddComponent<MeshCollider>();
    }
}
