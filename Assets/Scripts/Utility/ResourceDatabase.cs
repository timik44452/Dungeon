using UnityEngine;

[CreateAssetMenu]
public class ResourceDatabase : ScriptableObject
{
    public Sprite inventoryIconSprite
    {
        get => m_inventoryIconSprite;
    }
    public Sprite skillIconSprite
    {
        get => m_skillIconSprite;
    }
    public Sprite weaponIconSprite
    {
        get => m_weaponIconSprite;
    }

    public GameObject actionIconPrefab
    {
        get => m_actionIconPrefab;
    }
    public GameObject damageTextPrefab
    {
        get => m_damageTextPrefab;
    }

    [SerializeField]
    private Sprite m_inventoryIconSprite = null;
    [SerializeField]
    private Sprite m_skillIconSprite = null;
    [SerializeField]
    private Sprite m_weaponIconSprite = null;

    [SerializeField, Space]
    private GameObject m_actionIconPrefab = null;
    [SerializeField]
    private GameObject m_damageTextPrefab = null;
}
