using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float baseMoveSpeed;
    private bool _isMoving;
    private bool _canMove;
    private bool _canCast;

    [SerializeField] private AnimationController animationController;
    public AnimationController AnimationController => animationController;


    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;
    
    [SerializeField] private List<string> skillKeys;
    private List<SkillBase> _skillBases = new List<SkillBase>();
    private Coroutine _castingRoutine;

    [SerializeField] private float attackSpeed;
    public float AttackSpeed => attackSpeed;

    [SerializeField] private float attackPower;
    public float AttackPower => attackPower;
    
    

    private void Start()
    {
        animationController.SetMaxSpeed(baseMoveSpeed);
        _canMove = true;
        _canCast = true;

        healthComponent.Init(100f,
            damage =>
            {
                AnimationController.PlayAnimation("hit");
            },
            () =>
            {
                AnimationController.PlayAnimation("die");
            },
            remainHP =>
            {
                
            });
        
        foreach (var skillKey in skillKeys)
        {
            var newSkill = SkillManager.GetSkill(skillKey);
            newSkill.Init(skillKey);
            _skillBases.Add(newSkill);
        }
    }

    public void Move(Vector3 dir)
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
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
        characterController.Move(transform.forward * (baseMoveSpeed * Time.deltaTime));
    }

    public void Stop()
    {
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
        var index = _skillBases.FindIndex(skill => skill.SkillKey == skillKey);
        if (index < 0)
        {   
            Debug.Log($"Error: Can't find the skill ({skillKey})");
            return;
        }

        CastSkill(index);
    }
    
    public void CastSkill(int index)
    {
        if (!_canCast)
        {
            return;
        }
        
        _castingRoutine = StartCoroutine(CastRoutine(index));
    }

    private IEnumerator CastRoutine(int index)
    {
        _canCast = false;
        yield return _skillBases[index].Cast(this);
        _canCast = true;
    }

    public void DoneCasting()
    {
        LockMovement(false);
        AnimationController.ResetAnimationSpeed();
    }
}
