using UnityEngine;

public class Bow : Weapon
{
    private void Start()
    {
        defaultSkill = ResourceUtility.skillDatabase.GetSkill("bow");
    }

    public override void Invoke(ITarget target, Skill skill)
    {
        Skill attackSkill = Skill.CombineSkills(defaultSkill, skill);

        Projectile projectile = CreateArrow(new Vector3(0.05F, 1F, 0.05F) * 0.4F);

        Vector3 normal = (target.transform.position - transform.position).normalized;

        projectile.skill = attackSkill;
        projectile.transform.rotation = Quaternion.FromToRotation(projectile.transform.up, normal);

        projectile.GetComponent<Rigidbody>().AddForce(normal * 70, ForceMode.Impulse);
    }

    private Projectile CreateArrow(Vector3 scale)
    {
        GameObject arrow = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        arrow.transform.position = transform.position;

        arrow.transform.localScale = scale;
        arrow.AddComponent<Rigidbody>();

        Projectile projectile = arrow.AddComponent<Projectile>();
        

        return projectile;
    }
}
