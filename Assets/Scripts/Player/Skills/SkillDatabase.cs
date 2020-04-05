using UnityEngine;

using System.Collections.Generic;


[CreateAssetMenu(menuName = "Skills/Skill Database")]
public class SkillDatabase : ScriptableObject
{
    public List<Skill> skills;

    public Skill GetSkill(string skillName)
    {
        return skills.Find(x => x.name == skillName);
    }
}
