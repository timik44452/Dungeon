using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Skill defaultSkill { get; protected set; }

    public abstract void Invoke(ITarget target, Skill skill);
}
