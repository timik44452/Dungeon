using System;
using UnityEngine;
using UnityTimer;

public static class RuntimeManager
{
    private static event Action onClosing;
    private static event Action onUpdate;

    public static void RegisterApplicationClosingCallback(Action callback)
    {
        var controller = CreateIfNotCreated();

        onClosing += callback;

        controller.OnApplicationQuitEvent += () => onClosing?.Invoke();
    }

    public static void RegisterApplicationUpdateCallback(Action callback)
    {
        var controller = CreateIfNotCreated();

        onUpdate += callback;

        controller.OnApplicationUpdateEvent += () => onUpdate?.Invoke();
    }

    private static RuntimeController CreateIfNotCreated()
    {
        var runtimeController = UnityEngine.Object.FindObjectOfType<RuntimeController>();

        if (runtimeController == null)
        {
            GameObject gameObject = new GameObject("Runtime Manager");

            runtimeController = gameObject.AddComponent<RuntimeController>();
        }

        return runtimeController;
    }
}
