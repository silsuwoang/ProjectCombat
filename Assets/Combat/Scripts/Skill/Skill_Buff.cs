using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Buff : SkillBase
{
    public override IEnumerator Cast(Unit unit)
    {
        yield return base.Cast(unit);
        
        unit.AnimationController.SetOnAction(() =>
        {
            
        });
        
        yield return Wait(unit.AnimationController);
        // done
        unit.DoneCasting();
    }
}
