using UnityEngine;

[CreateAssetMenu(fileName = "SkillTable", menuName = "Scriptable Object/SkillTable", order = int.MaxValue)]
public class SkillTable :BaseTable<SkillData>
{
}


[System.Serializable]
public struct SkillData
{
    [SerializeField] private string key;
    public string Key => key;
    
    [SerializeField] private string clipName;
    public string ClipName => clipName;
    
    [SerializeField] private bool canMove;
    public bool CanMove => canMove;
    
    [SerializeField] private SkillBase.CastingType castingType;
    public SkillBase.CastingType CastingType => castingType;
    
    [SerializeField] private ColliderType collider;
    public ColliderType Collider => collider;
    
    [SerializeField] private float width;
    public float Width => width;
    
    [SerializeField] private float depth;
    public float Depth => depth;
    
    [SerializeField] private float radius;
    public float Radius => radius;
    
    [SerializeField] private EffectType[] effectTypes;
    public EffectType[] EffectTypes => effectTypes;
    
    [SerializeField] private string[] effectValues;
    public string[] EffectValues => effectValues;
}