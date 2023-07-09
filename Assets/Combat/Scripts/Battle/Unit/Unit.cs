using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Status
{
    BaseRotationSpeed,
    BaseMoveSpeed,
    MoveSpeedMultiplier,
    BaseAttackSpeed,
    AttackSpeedMultiplier,
    BaseMaxHealthPoint,
    MaxHealthPointMultiplier,
    BaseAttackPower,
    AttackPowerMultiplier
}

public class Unit : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private AnimationController animationController;
    public AnimationController AnimationController => animationController;


    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;
    
    [SerializeField] private List<string> skillKeys;
    
    private bool _isMoving;
    private bool _canMove;
    private bool _canCast;
    
    [SerializeField] private bool setSpeedAutomatically;
    private Vector3 _lastLocalPosition;


    private Dictionary<string, SkillBase> _skillBases = new Dictionary<string, SkillBase>();
    private Coroutine _castingRoutine;

    private List<BuffBase> _applyingBuffs = new List<BuffBase>();
    
    [Header("[ Status ]")] 
    [SerializeField] private float baseRotationSpeed;
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float moveSpeedMultiplier;
    public float RotationSpeed => baseRotationSpeed * moveSpeedMultiplier;
    public float MoveSpeed => baseMoveSpeed * moveSpeedMultiplier;
    [SerializeField] private float baseAttackSpeed;
    [SerializeField] private float attackSpeedMultiplier;
    public float AttackSpeed => baseAttackSpeed * moveSpeedMultiplier;
    [SerializeField] private float baseMaxHealthPoint;
    [SerializeField] private float maxHealthPointMultiplier;
    public float MaxHealthPoint => baseMaxHealthPoint * maxHealthPointMultiplier;
    [SerializeField] private float baseAttackPower;
    [SerializeField] private float attackPowerMultiplier;
    public float AttackPower => baseAttackPower * attackPowerMultiplier;
    
    public void Init()
    {
        healthComponent.Init(MaxHealthPoint, OnHit, OnDead, OnChangedHP);
        
        foreach (var skillKey in skillKeys)
        {
            if (_skillBases.ContainsKey(skillKey))
            {
                Debug.Log($"Error: The skill already added ({skillKey})");
                continue;
            }
            var newSkill = SkillManager.GetSkill(skillKey);
            newSkill.Init(skillKey, this);
            _skillBases.Add(skillKey,newSkill);
        }

        _isMoving = false;
        _canMove = true;
        _canCast = true;
    }

    private void Update()
    {
        UpdateBuffsTime();
    }

    private void FixedUpdate()
    {
        if (setSpeedAutomatically)
        { 
            SetSpeed();
        }
    }

    public void Move(Vector3 dir, float delta)
    {
        if (!_canMove)
        {
            return;
        }

        if (!_isMoving)
        {
            _isMoving = true;
            AnimationController.SetApplyRootMotion(false);
        }
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), RotationSpeed * delta);
        characterController.Move(transform.forward * (MoveSpeed * delta));
    }

    private void SetSpeed()
    {
        // 이동 속도 적용
        var currentMasterPosition = transform.localPosition;
        currentMasterPosition.y = 0f;
        var speed = Vector3.Distance(_lastLocalPosition, currentMasterPosition);
        AnimationController.SetParameter("Speed",speed / (MoveSpeed * Time.fixedDeltaTime));
        _lastLocalPosition = currentMasterPosition;
    }

    public void Stop()
    {
        if (!_isMoving)
        {
            return;
        }
        
        _isMoving = false;
        AnimationController.SetApplyRootMotion(true);
    }
    
    public void LockMovement(bool value)
    {
        _canMove = !value;
        if (value)
        {
            Stop();
        }
    }

    public void LockCasting(bool value)
    {
        _canCast = !value;
    }

    public void LookMouseWorldPoint()
    {
        if (!GameManager.Instance.InputManager.GetMouseWorldPosition(out var pos))
        {
            return;
        }
        pos.y = transform.position.y;
        transform.LookAt(pos);
    }

    public void CastSkill(string skillKey)
    {
        if (!_canCast)
        {
            return;
        }

        if (_skillBases.TryGetValue(skillKey, out var skill))
        {
            _castingRoutine = StartCoroutine(CastRoutine(skill));
            return;
        }
        
        Debug.Log($"Error: Can't find the skill ({skillKey})");
    }
    
    private IEnumerator CastRoutine(SkillBase skill)
    {
        LockCasting(true);
        yield return skill.Cast();
        LockCasting(false);
    }

    public void DoneCasting()
    {
        LockMovement(false);
        AnimationController.ResetAnimationSpeed();
        AnimationController.SetOnAction(null);
    }

    public void AddStatusValue(Status targetStatus, float value)
    {
        switch (targetStatus)
        {
            case Status.BaseRotationSpeed:
                baseRotationSpeed += value;
                break;
            case Status.BaseMoveSpeed:
                baseMoveSpeed += value;
                break;
            case Status.MoveSpeedMultiplier:
                moveSpeedMultiplier += value;
                break;
            case Status.BaseAttackSpeed:
                baseAttackSpeed += value;
                break;
            case Status.AttackSpeedMultiplier:
                attackSpeedMultiplier += value;
                break;
            case Status.BaseMaxHealthPoint:
                baseMaxHealthPoint += value;
                break;
            case Status.MaxHealthPointMultiplier:
                maxHealthPointMultiplier += value;
                break;
            case Status.BaseAttackPower:
                baseAttackPower += value;
                break;
            case Status.AttackPowerMultiplier:
                attackPowerMultiplier += value;
                break;
        }
    }

    public void AddBuff(BuffBase buff)
    {
        _applyingBuffs.Add(buff);
        buff.Apply();
    }

    public bool RemoveBuff(BuffBase buff)
    {
        if (_applyingBuffs.Remove(buff))
        {
            buff.Remove();
            return true;
        }
        return false;
    }

    private void UpdateBuffsTime()
    {
        var buffCount = _applyingBuffs.Count;
        for (var i = 0; i < buffCount; i++)
        {
            var current = _applyingBuffs[i];
            if (!current.UpdateTime())
            {
                if (RemoveBuff(current))
                {
                    buffCount--;
                }
            }
        }
    }

    private void OnHit(float damage)
    {
        AnimationController.PlayAnimation("hit");
        AnimationController.SetOnComplete((clip) =>
        {
            if (clip == "hit")
            {
                // hit done
                _canMove = true;
                _canCast = true;
            }
        });
        
        _canMove = false;
        _canCast = false;
        StopAllCoroutines();
    }

    private void OnDead()
    {
        AnimationController.PlayAnimation("die");
        characterController.enabled = false;
        _canMove = false;
        _canCast = false;
        StopAllCoroutines();
    }

    private void OnChangedHP(float remainHP)
    {
        
    }
}
