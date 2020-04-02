using UnityEngine;

public delegate void UnityEventHandler(Component sender);
public delegate void UnityEventHandler<T>(Component sender, T value);