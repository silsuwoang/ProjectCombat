using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_NormalAttack : SkillBase
{
    public override IEnumerator Cast()
    {
        yield return base.Cast();

        User.LookMouseWorldPoint();
        User.AnimationController.SetOnAction(() =>
        {
            var targets = GetTargets(User.transform.position + 
                                     (User.transform.rotation * new Vector3(0, 1, SkillData.Depth * 0.5f)));
            foreach (var target in targets)
            {
                BattleManager.SendEffect(User, target, EffectData);
            }
        });


        yield return WaitUntilAnimationComplete(User.AnimationController);
        // done
        User.DoneCasting();
    }
}
