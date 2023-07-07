using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_NormalAttack : SkillBase
{
    public override void Init(string key)
    {
        base.Init(key);
        Width = 1.7f;
        Depth = 2.7f;
        EffectTypes = new[] { EffectType.PhysicalDamage };
        EffectValues = new[] { 1f };
    }

    public override IEnumerator Cast(Unit unit)
    {
        yield return base.Cast(unit);

        unit.LookMouseWorldPoint();
        unit.AnimationController.SetOnAction(() =>
        {
            var hits = BattleManager.GetCollidedHealthComponent(unit, Collider, Width, Depth);
            foreach (var hit in hits)
            {
                BattleManager.SendEffect(unit, hit, new BattleManager.EffectData()
                {
                    EffectTypes = EffectTypes,
                    EffectValues = EffectValues,
                });
                // BattleManager.Attack(unit, hit);
            }
        });


        yield return Wait(unit.AnimationController);
        // done
        unit.DoneCasting();
    }
}
