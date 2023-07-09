using System.Collections;
using UnityEngine;

public class Skill_NormalAttack : SkillBase
{
    protected override IEnumerator Action()
    {
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
