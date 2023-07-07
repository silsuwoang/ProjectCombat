using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SkillBase
{
    public string SkillKey { get; private set; }
    [SerializeField] protected string clipName;

    [SerializeField] protected bool canMove;

    public enum Targeting
    {
        Self,
        
    }
    


    protected ColliderType Collider;
    protected float Width;
    protected float Depth;

    protected EffectType[] EffectTypes;
    protected float[] EffectValues;
    
    public virtual void Init(string key)
    {
        SkillKey = key;
        clipName = key;
        canMove = false;
    }
    
    public virtual IEnumerator Cast(Unit unit)
    {
        unit.LockMovement(!canMove);
        
        unit.AnimationController.SetAnimationSpeed(unit.AttackSpeed);
        unit.AnimationController.PlayAnimation(clipName);

        yield break;
    }

    protected IEnumerator Wait(AnimationController animationController)
    {
        // if (_clipLength < 0)
        // {
        //     // _clipLength = unit.AnimationController
        //
        // }

        var completes = false;
        animationController.SetOnComplete((clip) =>
        {
            if (clip != clipName)
            {
                return;
            }
            completes = true;
        });
        yield return new WaitUntil(() => completes);
        // yield return new WaitUntil(() => animationController.IsPlayingAnimation(clipName));
        // yield return new WaitWhile(() => animationController.IsPlayingAnimation(clipName));
    }
}
