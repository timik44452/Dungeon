using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Skill skill;

    public void OnCollisionEnter(Collision collision)
    {
        foreach (var effectListener in collision.gameObject.GetComponents<ISkillEffectListener>())
        {
            effectListener.EffectInvoke(this, skill.effects);
        }

        Destroy(gameObject);    
    }
}
