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
            "buff" => new Skill_Buff(),
            _ => new SkillBase()
        };
    }
}
