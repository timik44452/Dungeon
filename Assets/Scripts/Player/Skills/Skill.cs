﻿using UnityEngine;

using System.Collections.Generic;

[System.Serializable]
public class Skill
{
    public string name;

    public float mana;
    public float power;
    public float reloadingTime;

    public Texture icon;

    public List<SkillEffect> effects;

    public Skill()
    {
        effects = new List<SkillEffect>();
    }

    //TODO: bad is bad
    public static Skill CombineSkills(params Skill[] skills)
    {
        Skill result = new Skill();

        foreach (Skill skill in skills)
        {
            if(skill == null)
            {
                continue;
            }

            result.mana += skill.mana;
            result.power += skill.power;
            result.reloadingTime += skill.reloadingTime;

            result.effects.AddRange(skill.effects);
        }

        return result;
    }
}
