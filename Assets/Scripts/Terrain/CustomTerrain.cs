using Game.Service;
using Game.Terrain.Service;
using UnityEngine;

public class CustomTerrain : MonoBehaviour
{
    public Material terrain_material;
    public TerrainSettings terrainSettings;

    public const int chunk_size = 64;

    private void Start()
    {
        Region region = new Region(transform.position, new Vector3(terrainSettings.width, terrainSettings.height, terrainSettings.length));

        foreach (var sub_region in region.GetSubRegions(chunk_size, chunk_size))
            CreateChunk(sub_region);
    }

    public void CreateChunk(Region region)
    {
        GameObject chunkGameObject = new GameObject($"chunk {transform.childCount}");

        chunkGameObject.transform.parent = transform;
        chunkGameObject.transform.localPosition = new Vector3(region.X, 0, region.Z);

        CheckChunkComponents(chunkGameObject);

        var chunk = chunkGameObject.GetComponent<Chunk>();

        chunk.InitializeChunk(region);
    }

    private void CheckChunkComponents(GameObject gameObject)
    {
        if (!gameObject.GetComponent<MeshFilter>())
            gameObject.AddComponent<MeshFilter>();

        if (!gameObject.GetComponent<MeshRenderer>())
            gameObject.AddComponent<MeshRenderer>();

        if (!gameObject.GetComponent<Chunk>())
            gameObject.AddComponent<Chunk>();

        if (!gameObject.GetComponent<MeshCollider>())
            gameObject.AddComponent<MeshCollider>();

        gameObject.GetComponent<MeshRenderer>().material = terrain_material;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(terrainSettings.width, 0, terrainSettings.length));
    }
}
