public class Gun : Weapon
{
    private void Start()
    {
        defaultSkill = ResourceUtility.skillDatabase.GetSkill("gun");
    }

    public override void Invoke(ITarget target, Skill skill)
    {
        Skill attackSkill = Skill.CombineSkills(defaultSkill, skill);

        foreach (var effectListener in target.gameObject.GetComponents<ISkillEffectListener>())
        {
            effectListener.Invoke(attackSkill);
        }
    }
}
