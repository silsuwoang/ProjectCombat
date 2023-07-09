using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private float _maxHP;
    private float _currentHP;

    private Action<float> _onHit;
    private Action _onDead;
    private Action<float> _onChangedHP;

    public void Init(float maxHP, Action<float> onHit, Action onDead, Action<float> onChangedHP)
    {
        _maxHP = maxHP;
        _currentHP = maxHP;
        _onHit = onHit;
        _onDead = onDead;
        _onChangedHP = onChangedHP;
    }
    
    public void Hit(BattleManager.AttackData attackData)
    {
        _currentHP -= attackData.Damage;
        
        _onHit?.Invoke(attackData.Damage);
        _onChangedHP?.Invoke(_currentHP);
    
        if (_currentHP <= 0)
        {
            _onDead?.Invoke();
        }
    }
}
