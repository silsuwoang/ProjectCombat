using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Action _onAction = null;
    private Action<string> _onComplete;
    
    private void Awake()
    {
        // 애니메이션 종료 이벤트 추가
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            var animationEndEvent = new AnimationEvent
            {
                time = clip.length,
                functionName = "OnComplete",
                stringParameter = clip.name
            };

            clip.AddEvent(animationEndEvent);
        }
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
    
    public void PlayAnimation(string clipName)
    {
        animator.Play(clipName);
        
    }

    public bool IsPlayingAnimation(string clipName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(clipName);
    }
    
    public void SetParameter(string parameterName, float value)
    {
        animator.SetFloat(parameterName, value);
    }

    public void SetParameter(string parameterName, bool value)
    {
        animator.SetBool(parameterName, value);
    }

    public void SetOnAction(Action onAction)
    {
        _onAction = onAction;
    }

    public void SetOnComplete(Action<string> onComplete)
    {
        _onComplete = onComplete;
    }
    
    // Events
    private void OnAction()
    {
        _onAction?.Invoke();
    }
    
    public void OnComplete(string clipName)
    {
        _onComplete?.Invoke(clipName);
    }
}
