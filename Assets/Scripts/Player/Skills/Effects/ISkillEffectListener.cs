using UnityEngine;
using System.Collections.Generic;

public interface ISkillEffectListener
{
    void EffectInvoke(Component sender, IEnumerable<SkillEffect> effects);
}
