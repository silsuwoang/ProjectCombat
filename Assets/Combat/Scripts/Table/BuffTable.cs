using UnityEngine;

[CreateAssetMenu(fileName = "BuffTable", menuName = "Scriptable Object/BuffTable", order = int.MaxValue)]
public class BuffTable : BaseTable<BuffData>
{
}

[System.Serializable]
public struct BuffData
{
    [SerializeField] private string key;
    public string Key => key;
    
    [SerializeField] private Status[] targetStatuses;
    public Status[] TargetStatuses => targetStatuses;
    
    [SerializeField] private float[] values;
    public float[] Values => values;

    [SerializeField] private float duration;
    public float Duration => duration;
}
