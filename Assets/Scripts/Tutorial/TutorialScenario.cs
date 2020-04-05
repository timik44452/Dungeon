using UnityEngine;

public abstract class TutorialScenario : ScriptableObject
{
    public string text = string.Empty;
    public float timeout = 0.0F;

    public abstract void Start();
    public abstract bool Update();
}
