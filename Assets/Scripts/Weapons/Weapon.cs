using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public string Name;
    public Sprite icon;

    public float reloadingTimeout = 1.0F;

    public SkillEffect[] effects;

    public virtual void BeginInvoke(Component sender, ITarget target, Skill skill)
    {

    }

    public virtual void EndInvoke(Component sender, ITarget target, Skill skill)
    {

    }
}
