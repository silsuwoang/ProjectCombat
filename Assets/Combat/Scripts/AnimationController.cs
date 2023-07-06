using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Transform master;
    
    [SerializeField] private Animator animator;
    private Action _onAction = null;

    [SerializeField] private bool checksSpeed;
    
    private float _maxSpeed;
    private Vector3 _lastMasterPosition;

    private void Update()
    {
        if (checksSpeed)
        {
            ApplySpeed();
        }
    }

    public void SetMaxSpeed(float value)
    {
        _maxSpeed = value;
    }
    
    private void CheckSpeed()
    {
        var current = master.localPosition;
        current.y = 0;
        var speed = Vector3.Distance(current, _lastMasterPosition);
        speed /= Time.deltaTime;
        SetFloat("Speed", Mathf.Clamp(speed / _maxSpeed, 0f, 1f));
        _lastMasterPosition = current;
    }
    
    private void ApplySpeed()
    {
        var currentMasterPosition = master.localPosition;
        currentMasterPosition.y = 0f;
        var speed = Vector3.Distance(_lastMasterPosition, currentMasterPosition);
        speed /= Time.deltaTime;
        Debug.Log(speed);
        SetParameter("Speed",speed / _maxSpeed);
        _lastMasterPosition = currentMasterPosition;
    }

    public void SetApplyRootMotion(bool value)
    {
        animator.applyRootMotion = value;
    }

    public void PlayAnimation(string clipName)
    {
        animator.Play(clipName);
    }

    public void SetParameter(string parameterName, float value)
    {
        animator.SetFloat(parameterName, value);
    }
    
    public void SetFloat(string parameter, float value)
    {
        animator.SetFloat(parameter, value);
    }

    public void SetOnAction(Action onAction)
    {
        _onAction = onAction;
    }

    private void Action()
    {
        _onAction?.Invoke();
    }
}
