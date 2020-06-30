using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class Recognition : MonoBehaviour
{
    private LineRenderer m_lineRenderer;

    private List<Vector2> m_points = new List<Vector2>();
    private int[,] m_indexTable = new int[,]
        {
            { 0, 1, 2, 3, 3, 2, 1, 0 },
            { 1, 2, 3, 4, 4, 3, 2, 1 },
            { 2, 3, 4, 5, 6, 4, 3, 2 },
            { 3, 4, 6, 7, 7, 5, 4, 3 },
            { 3, 4, 5, 7, 7, 6, 4, 3 },
            { 2, 3, 4, 6, 5, 4, 3, 2 },
            { 1, 2, 3, 4, 4, 3, 2, 1 },
            { 0, 1, 2, 3, 3, 2, 1, 0 }
        };

    private const int max_points_count = 7000;

    private void Start()
    {
        transform.parent = SceneUtility.Player.transform;
        transform.localPosition = Vector3.zero;

        m_lineRenderer = gameObject.AddComponent<LineRenderer>();
        m_lineRenderer.useWorldSpace = false;
        m_lineRenderer.startWidth = 0.1F;
        m_lineRenderer.endWidth = 0.1F;

        TryAddPoint(GetScreenPoint());
    }

    private void Update()
    {
        TryAddPoint(GetScreenPoint());
        DrawGlyph();
    }

    private void OnDestroy()
    {
        NormalizePoints();

        int width = m_indexTable.GetLength(0) - 1;
        int height = m_indexTable.GetLength(1) - 1;

        Dictionary<int, int> indexDictionary = new Dictionary<int, int>();

        foreach (Vector2 point in m_points)
        {
            int x = Mathf.RoundToInt(Mathf.Clamp01(point.x) * width);
            int y = Mathf.RoundToInt(Mathf.Clamp01(point.y) * height);

            int index = m_indexTable[x, y];

            if (!indexDictionary.ContainsKey(index))
            {
                indexDictionary.Add(index, 0);
            }

            indexDictionary[index]++;
        }

        int max = indexDictionary.Values.Max();
        int max_index_key = indexDictionary.Where(x => x.Value.Equals(max)).Select(x => x.Key).FirstOrDefault();

        indexDictionary.Remove(max_index_key);

        Debug.Log($"Cast {max_index_key}");
    }

    private void SaveTexture(Texture2D texture2D, bool dispose = true)
    {
        // Encode texture into PNG
        byte[] bytes = texture2D.EncodeToPNG();

        if (dispose)
        {
            Destroy(texture2D);
        }

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
    }

    private void NormalizePoints()
    {
        Vector2 topLeft = Vector2.zero;
        Vector2 bottomRight = Vector2.zero;

        topLeft.x = m_points.Min(p => p.x) + 1;
        topLeft.y = m_points.Min(p => p.y) + 1;

        bottomRight.x = m_points.Max(p => p.x) - 1;
        bottomRight.y = m_points.Max(p => p.y) - 1;

        Rect rect = new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);

        for (int i = 0; i < m_points.Count; i++)
        {
            Vector2 point = m_points[i];

            point.x = (point.x - topLeft.x) / rect.width;
            point.y = (point.y - topLeft.y) / rect.height;

            m_points[i] = point;
        }
    }

    private void DrawGlyph()
    {
        m_lineRenderer.positionCount = m_points.Count;

        Vector2 topLeft = Vector2.zero;
        Vector2 bottomRight = Vector2.zero;

        topLeft.x = m_points.Min(p => p.x) + 1;
        topLeft.y = m_points.Min(p => p.y) + 1;

        bottomRight.x = m_points.Max(p => p.x) - 1;
        bottomRight.y = m_points.Max(p => p.y) - 1;

        Rect rect = new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);

        for (int i = 0; i < m_points.Count; i++)
        {
            Vector2 point = m_points[i];

            point.x = (point.x - topLeft.x) / rect.width;
            point.y = (point.y - topLeft.y) / rect.height;

            m_lineRenderer.SetPosition(i, point);
        }
    }

    private void TryAddPoint(Vector2 point)
    {
        float threshould = Mathf.Min(Screen.width, Screen.height) * 0.1F;

        if (m_points.Count > max_points_count || Vector2.Distance(GetLastPoint(), point) < threshould)
        {
            return;
        }

        m_points.Add(point);
    }

    private Vector2 GetScreenPoint()
    {
        return new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
    }

    private Vector2 GetLastPoint()
    {
        if(m_points.Count == 0)
        {
            return Vector2.zero;
        }

        return m_points[m_points.Count - 1];
    }
}
