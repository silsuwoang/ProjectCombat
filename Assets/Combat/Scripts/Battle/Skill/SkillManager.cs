
public static class SkillManager
{
    public static SkillBase GetSkill(string skillKey)
    {
        // Note: 스킬키를 클래스 이름과 연동하여 이름을 통해 가져와서 스킬 추가 시 추가 작업 필요없게 할 수 있을듯.
        return skillKey switch
        {
            "normal_attack" => new Skill_NormalAttack(),
            "rage" => new Skill_Rage(),
            "wave" => new Skill_Wave(),
            _ => new SkillBase()
        };
    }
}
