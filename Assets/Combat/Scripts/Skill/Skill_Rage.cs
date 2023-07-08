using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Rage : SkillBase
{
    public override IEnumerator Cast()
    {
        yield return base.Cast();
        
        User.AnimationController.SetOnAction(() =>
        {
            var targets = GetTargets(User.transform.position);
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
