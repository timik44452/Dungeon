using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public string Name;
    public Sprite icon;

    public float reloadingTimeout = 1.0F;

    public SkillEffect[] effects;

    public abstract void Invoke(Component sender, ITarget target, Skill skill);
}
