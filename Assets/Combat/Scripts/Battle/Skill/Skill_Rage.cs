using System.Collections;

public class Skill_Rage : SkillBase
{
    protected override IEnumerator Action()
    {
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
