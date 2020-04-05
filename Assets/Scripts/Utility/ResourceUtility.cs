using UnityEngine;

public class ResourceUtility
{
    public static ResourceDatabase resourceDatabase
    {
        get
        {
            if (s_resourceDatabase == null)
            {
                s_resourceDatabase = Resources.Load<ResourceDatabase>(s_resourceDatabaseName);
            }

            return s_resourceDatabase;
        }
    }
    public static InventoryDatabase inventoryDatabase
    {
        get
        {
            if (s_inventoryDatabase == null)
            {
                s_inventoryDatabase = Resources.Load<InventoryDatabase>(s_inventoryDatabaseName);
            }

            return s_inventoryDatabase;
        }
    }
    public static WeaponDatabase weaponDatabase
    {
        get
        {
            if (s_weaponDatabase == null)
            {
                s_weaponDatabase = Resources.Load<WeaponDatabase>(s_weaponDatabaseName);
            }

            return s_weaponDatabase;
        }
    }
    public static SkillDatabase skillDatabase
    {
        get
        {
            if (s_skillDatabase == null)
            {
                s_skillDatabase = Resources.Load<SkillDatabase>(s_skillDatabaseName);
            }

            return s_skillDatabase;
        }
    }

    private static ResourceDatabase s_resourceDatabase;
    private static InventoryDatabase s_inventoryDatabase;
    private static WeaponDatabase s_weaponDatabase;
    private static SkillDatabase s_skillDatabase;

    private const string s_resourceDatabaseName = "ResourceDatabase";
    private const string s_inventoryDatabaseName = "InventoryDatabase";
    private const string s_weaponDatabaseName = "WeaponDatabase";
    private const string s_skillDatabaseName = "SkillDatabase";

#if UNITY_EDITOR
    public static void CreateResourceDatabase()
    {
        ResourceDatabase database = ScriptableObject.CreateInstance<ResourceDatabase>();

        UnityEditor.AssetDatabase.CreateAsset(database, $"Assets/Resources/{s_resourceDatabaseName}.asset");
    }
    public static void CreateInventoryDatabase()
    {
        InventoryDatabase database = ScriptableObject.CreateInstance<InventoryDatabase>();

        UnityEditor.AssetDatabase.CreateAsset(database, $"Assets/Resources/{s_inventoryDatabaseName}.asset");
    }
    public static void CreateWeaponDatabase()
    {
        WeaponDatabase database = ScriptableObject.CreateInstance<WeaponDatabase>();

        UnityEditor.AssetDatabase.CreateAsset(database, $"Assets/Resources/{s_weaponDatabaseName}.asset");
    }
    public static void CreateSkillsDatabase()
    {
        SkillDatabase database = ScriptableObject.CreateInstance<SkillDatabase>();

        UnityEditor.AssetDatabase.CreateAsset(database, $"Assets/Resources/{s_skillDatabaseName}.asset");
    }
#endif
}
