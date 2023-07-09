using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wave : SkillBase
{
    protected override IEnumerator Action()
    {
        if (!GameManager.Instance.InputManager.GetMouseWorldPosition(out var pos))
        {
            yield break;
        }
        
        User.LookMouseWorldPoint();
        User.AnimationController.SetOnAction(() =>
        {
            var targets = GetTargets(pos);
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
