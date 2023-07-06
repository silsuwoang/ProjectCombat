using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillBase
{
    [SerializeField] private string skillKey;
    [SerializeField] private string clipName;


    public IEnumerator Cast(Unit unit)
    {
        unit.AnimationController.SetOnAction(() =>
        {
            Debug.Log("Hit");
        });
        unit.AnimationController.PlayAnimation(clipName);
        yield break;
    }
}
