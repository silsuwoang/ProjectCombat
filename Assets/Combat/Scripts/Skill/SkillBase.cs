using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SkillBase
{
    public enum CastingType
    {
        Instant,
        PositionSelect
    }
    protected SkillData SkillData;
    public string SkillKey => SkillData.Key;

    protected BattleManager.EffectData EffectData;

    protected Unit User;
    
    public virtual void Init(string key, Unit user)
    {
        SkillData = GameManager.Instance.TableContainer.GetTable<SkillTable>().GetData(key);
        EffectData = new BattleManager.EffectData()
        {
            EffectTypes = SkillData.EffectTypes,
            EffectValues = SkillData.EffectValues,
        };
        
        User = user;
    }
    
    public virtual IEnumerator Cast()
    {
        switch (SkillData.CastingType)
        {
            case CastingType.Instant:
                User.LockMovement(!SkillData.CanMove);
        
                User.AnimationController.SetAnimationSpeed(User.AttackSpeed);
                User.AnimationController.PlayAnimation(SkillData.ClipName);

                yield return Action();
                yield break;
            
            case CastingType.PositionSelect:
                IEnumerator actionCoroutine = null;
                switch (SkillData.Collider)
                {
                    case ColliderType.Box:
                        GameManager.Instance.PositionSelector.Enable(new Vector3(SkillData.Width, 2, SkillData.Depth));
                        break;
                    case ColliderType.Sphere:
                        var diameter = SkillData.Radius * 2;
                        GameManager.Instance.PositionSelector.Enable(new Vector3(diameter, diameter, diameter));
                        break;
                }
                GameManager.Instance.InputManager.SetBaseKeyEvent(() =>
                {
                    if (!GameManager.Instance.InputManager.GetMouseWorldPosition(out var pos))
                    {
                        return;
                    }
            
                    User.LockMovement(!SkillData.CanMove);
                    User.AnimationController.SetAnimationSpeed(User.AttackSpeed);
                    User.AnimationController.PlayAnimation(SkillData.ClipName);
            
                    GameManager.Instance.InputManager.ResetBaseKeyEvent();
                    GameManager.Instance.PositionSelector.Disable();
                    actionCoroutine = Action();
                });
                yield return new WaitUntil(() => actionCoroutine != null);
                yield return actionCoroutine;
                yield break;

            default:
                yield break;
        }
    }

    protected virtual IEnumerator Action()
    {
        yield break;   
    }

    protected IEnumerator WaitUntilAnimationComplete(AnimationController animationController)
    {
        var completes = false;
        animationController.SetOnComplete((clip) =>
        {
            if (clip != SkillData.ClipName)
            {
                return;
            }
            completes = true;
        });
        yield return new WaitUntil(() => completes);
    }

    protected List<HealthComponent> GetTargets(Vector3 center)
    {
        var result = new List<HealthComponent>();
        switch (SkillData.Collider)
        {
            case ColliderType.Self:
                result.Add(User.HealthComponent);
                break;
            case ColliderType.Box:
                result = BattleManager.GetBoxCollidedHealthComponent(center,
                    new Vector3(SkillData.Width, 2, SkillData.Depth), 
                    User.transform.rotation,
                    new[] { User.transform });
                break;
            case ColliderType.Sphere:
                result = BattleManager.GetSphereCollidedHealthComponent(center, SkillData.Radius, new[] { User.transform });
                break;
        }
        return result;
    }
}
