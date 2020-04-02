using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool IsReloading { get => reloadingTimer > 0; }

    public Skill defaultSkill { get; protected set; }

    
    protected float reloadingTimer = 0.0F;


    public abstract void Invoke(ITarget target, Skill skill);
}
