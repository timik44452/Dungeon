using UnityEngine;

using System.Collections.Generic;


[CreateAssetMenu]
public class SkillDatabase : ScriptableObject
{
    public List<Skill> skills;

    public Skill GetSkill(string skillName)
    {
        return skills.Find(x => x.name == skillName);
    }
}
