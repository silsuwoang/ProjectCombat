using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Transform master;
    
    [SerializeField] private Animator animator;
    private Action _onAction = null;

    [SerializeField] private bool appliesSpeed;
    
    private float _maxSpeed;
    private Vector3 _lastMasterPosition;

    private Action<string> _onComplete;
    
    private void Awake()
    {
        for (var i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            var clip = animator.runtimeAnimatorController.animationClips[i];

            var animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "OnComplete";
            animationEndEvent.stringParameter = clip.name;

            clip.AddEvent(animationEndEvent);
        }
    }

    private void Update()
    {
        if (appliesSpeed)
        {
            ApplySpeed();
        }
    }

    public void SetMaxSpeed(float value)
    {
        _maxSpeed = value;
    }
    
    private void ApplySpeed()
    {
        var currentMasterPosition = master.localPosition;
        currentMasterPosition.y = 0f;
        var speed = Vector3.Distance(_lastMasterPosition, currentMasterPosition);
        speed /= Time.deltaTime;
        SetParameter("Speed",speed / _maxSpeed);
        _lastMasterPosition = currentMasterPosition;
    }

    public void SetApplyRootMotion(bool value)
    {
        animator.applyRootMotion = value;
    }

    public void SetAnimationSpeed(float value)
    {
        animator.speed = value;
    }

    public void ResetAnimationSpeed()
    {
        animator.speed = 1f;
    }

    public int GetCurrentClipLength(string clipName)
    {
        return animator.GetCurrentAnimatorClipInfoCount(0);
    }
    
    public bool IsPlayingAnimation(string clipName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(clipName);
    }

    public void PlayAnimation(string clipName, Action<string> onFinished = null)
    {
        animator.Play(clipName);
        
    }

    public void SetParameter(string parameterName, float value)
    {
        animator.SetFloat(parameterName, value);
    }

    public void SetOnAction(Action onAction)
    {
        _onAction = onAction;
    }

    public void SetOnComplete(Action<string> onComplete)
    {
        _onComplete = onComplete;
    }
    
    private void OnAction()
    {
        _onAction?.Invoke();
    }
    
    public void OnComplete(string clipName)
    {
        _onComplete?.Invoke(clipName);
    }
}
