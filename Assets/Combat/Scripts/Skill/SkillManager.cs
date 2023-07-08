using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillManager
{
    public static SkillBase GetSkill(string skillKey)
    {
        return skillKey switch
        {
            "normal_attack" => new Skill_NormalAttack(),
            "rage" => new Skill_Rage(),
            "wave" => new Skill_Wave(),
            _ => new SkillBase()
        };
    }
}
