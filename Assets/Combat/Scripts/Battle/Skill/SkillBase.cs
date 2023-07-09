using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public enum CastingType
    {
        Instant,
        PositionSelect
    }
    
    protected SkillData SkillData;
    public string SkillKey => SkillData.Key;
    protected Unit User;
    protected BattleManager.EffectData EffectData; // Init 단계에서 미리 생성
    
    public virtual void Init(string key, Unit user)
    {
        SkillData = GameManager.Instance.TableContainer.GetTable<SkillTable>().GetData(key);
        User = user;
        EffectData = new BattleManager.EffectData()
        {
            Sender = user,
            EffectTypes = SkillData.EffectTypes,
            EffectValues = SkillData.EffectValues,
        };
    }
    
    public virtual IEnumerator Cast()
    {
        switch (SkillData.CastingType)
        {
            // 즉시 사용
            case CastingType.Instant:
                User.LockMovement(!SkillData.CanMove);
        
                User.AnimationController.SetAnimationSpeed(User.AttackSpeed);
                User.AnimationController.PlayAnimation(SkillData.ClipName);
                
                yield return Action();
                yield break;
            
            // 위치 선택 후 사용
            case CastingType.PositionSelect:
                IEnumerator actionCoroutine = null;
                switch (SkillData.Collider)
                {
                    // Note: PositionSelector 모양 추가 시 수정 
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

                    actionCoroutine = Action();
                    GameManager.Instance.PositionSelector.Disable();
                    GameManager.Instance.InputManager.ResetBaseKeyEvent();
                });
                
                yield return new WaitUntil(() => actionCoroutine != null); // BaseKeyEvent 호출될 때까지 대기
                yield return actionCoroutine;
                yield break;
        }
    }

    protected virtual IEnumerator Action()
    {
        User.DoneCasting();
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
        // ColliderType에 따라 타겟 얻어오기
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
