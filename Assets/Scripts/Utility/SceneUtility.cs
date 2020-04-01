using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneUtility
{
    public static GameObject Player
    { 
        get
        {
            if(s_player == null)
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
            if (s_targets == null)
            {
                s_targets = FindTargets();
            }

            s_targets.RemoveAll(x => x == null);

            return s_targets;
        }
    }


    private static GameObject s_player = null;
    private static List<ITarget> s_targets = null;
    private static Dictionary<System.Type, IList> componentRegister = null;

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

    public static List<ITarget> FindTargets()
    {
        List<ITarget> targets = new List<ITarget>();

        foreach (GameObject gameObject in Object.FindObjectsOfType<GameObject>())
            targets.AddRange(gameObject.GetComponents<ITarget>());

        return targets;
    }

    private static GameObject FindPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    private static void RegisterInterfaces(GameObject gameObject)
    {
        componentRegister.Add(typeof(ITarget), s_targets);

        foreach (var key in componentRegister)
        {
            key.Value.Add(gameObject.GetComponent(key.Key));
        }
    }
}
    