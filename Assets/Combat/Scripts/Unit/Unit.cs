using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;

    [SerializeField] private AnimationController animationController;
    public AnimationController AnimationController => animationController;

    [SerializeField] private List<SkillBase> skillBases = new List<SkillBase>();


    private void Start()
    {
        animationController.SetMaxSpeed(moveSpeed);
    }

    public void Move(Vector3 dir)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
        characterController.Move(transform.forward * (moveSpeed * Time.deltaTime));
    }

    public void CastSkill(int index)
    {
        StartCoroutine(CastRoutine(index));
    }

    private IEnumerator CastRoutine(int index)
    {
        yield return skillBases[index].Cast(this);
    }
}
