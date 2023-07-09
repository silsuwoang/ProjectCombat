using System.Collections;

public class Skill_Wave : SkillBase
{
    protected override IEnumerator Action()
    {
        if (!GameManager.Instance.InputManager.GetMouseWorldPosition(out var pos))
        {
            // Error: 마우스 위치 얻을 수 없음
            User.DoneCasting();
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
