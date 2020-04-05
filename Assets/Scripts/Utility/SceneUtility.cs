using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneUtility
{
    public static GameObject Player
    {
        get
        {
            if (s_player == null)
            {
                s_player = FindPlayer();
            }

            return s_player;
        }
    }
    public static List<ITarget> Targets
    {
        get
        {
            s_targets.RemoveAll(x => TargetSystem.ITargetIsNull(x));

            return s_targets;
        }
    }


    private static GameObject s_player = null;
    private static List<ITarget> s_targets = new List<ITarget>();
    private static Dictionary<System.Type, IList> componentRegister = new Dictionary<System.Type, IList>()
    {
        { typeof(ITarget), s_targets }
    };

    
    public static void CreateObject(GameObject prototype, Transform parent = null)
    {
        GameObject gameObject;

        if (parent == null)
        {
            gameObject = Object.Instantiate(prototype);
        }
        else
        {
            gameObject = Object.Instantiate(prototype, parent);
        }

        RegisterInterfaces(gameObject);
    }

    public static void CreateObject(GameObject prototype, Vector3 position, Quaternion rotation)
    {
        GameObject gameObject = Object.Instantiate(prototype, position, rotation);

        RegisterInterfaces(gameObject);
    }

    public static List<T> FindObjects<T>()
    {
        List<T> targets = new List<T>();

        foreach (GameObject gameObject in Object.FindObjectsOfType<GameObject>())
        {
            targets.AddRange(gameObject.GetComponents<T>());
        }

        return targets;
    }

    public static GameObject FindPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    private static void RegisterInterfaces(GameObject gameObject)
    {
        foreach (var key in componentRegister)
        {
            var component = gameObject.GetComponent(key.Key);

            key.Value.Add(component);
        }
    }
}
    