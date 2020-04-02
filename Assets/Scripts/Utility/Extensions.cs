using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Возвращает элемент с именеи формата $name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tag"></param>
    /// <param name="parent"></param>
    public static T GetElement<T>(string tag, GameObject parent) where T : Component
    {
        foreach (T t_component in parent.GetComponentsInChildren<T>(true))
        {
            Component component = t_component as Component;

            if (component)
            {
                string name = component.gameObject.name;

                name = name.Replace($"${tag}", string.Empty);
                name = name.Trim();

                if (string.IsNullOrEmpty(name))
                    return t_component;
            }
        }

        return default;
    }

    /// <summary>
    /// Возвращает элементы с именеи формата $name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tag"></param>
    /// <param name="parent"></param>
    public static T[] GetElements<T>(this GameObject parent, string tag) where T : Component
    {
        List<T> result = new List<T>();

        foreach (T t_component in parent.GetComponentsInChildren<T>(true))
        {
            Component component = t_component as Component;

            if (component)
            {
                string name = component.gameObject.name;

                name = name.Replace($"${tag}", string.Empty);
                name = name.Trim();

                if (string.IsNullOrEmpty(name))
                    result.Add((T)component);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Возвращает элемент с именеи формата $name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tag"></param>
    /// <param name="parent"></param>
    public static T GetElement<T>(this GameObject parent, string tag) where T : Component
    {
        return GetElement<T>(tag, parent);
    }

    /// <summary>
    /// Возвращает типа T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tag"></param>
    /// <param name="parent"></param>
    public static List<T> GetElement<T>(GameObject parent) where T : Component
    {
        return new List<T>(parent.GetComponentsInChildren<T>(true));
    }

}