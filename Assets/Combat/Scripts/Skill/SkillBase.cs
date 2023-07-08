using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SkillBase
{
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
        User.LockMovement(!SkillData.CanMove);
        
        User.AnimationController.SetAnimationSpeed(User.AttackSpeed);
        User.AnimationController.PlayAnimation(SkillData.ClipName);

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
                result = BattleManager.GetCollidedHealthComponent(center,
                    new Vector3(SkillData.Width, 2, SkillData.Depth), 
                    User.transform.rotation,
                    new[] { User.transform });
                break;
            case ColliderType.Sphere:
                result = BattleManager.GetCollidedHealthComponent(center, SkillData.Radius, new[] { User.transform });
                break;
        }
        return result;
    }
}
