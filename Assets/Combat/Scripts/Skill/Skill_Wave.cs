using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wave : SkillBase
{
    public override IEnumerator Cast()
    {
        GameManager.Instance.InputManager.SetBaseKeyEvent(() =>
        {
            if (!GameManager.Instance.InputManager.GetMouseWorldPosition(out var pos))
            {
                return;
            }
            
            User.LockMovement(!SkillData.CanMove);
            User.LookMouseWorldPoint();

            User.AnimationController.SetAnimationSpeed(User.AttackSpeed);
            User.AnimationController.PlayAnimation(SkillData.ClipName);
            
            GameManager.Instance.InputManager.ResetBaseKeyEvent();
            
            User.AnimationController.SetOnAction(() =>
            {
                var targets = GetTargets(pos);
                foreach (var target in targets)
                {
                    BattleManager.SendEffect(User, target, EffectData);
                }
            });
        });
        

        yield return WaitUntilAnimationComplete(User.AnimationController);
        // done
        User.DoneCasting();
    }
}
