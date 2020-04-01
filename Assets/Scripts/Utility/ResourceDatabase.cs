using UnityEngine;

[CreateAssetMenu]
public class ResourceDatabase : ScriptableObject
{
    public GameObject actionIconPrefab
    {
        get => m_actionIconPrefab;
    }
    public GameObject damageTextPrefab
    {
        get => m_damageTextPrefab;
    }

    [SerializeField]
    private GameObject m_actionIconPrefab = null;
    [SerializeField]
    private GameObject m_damageTextPrefab = null;
}
