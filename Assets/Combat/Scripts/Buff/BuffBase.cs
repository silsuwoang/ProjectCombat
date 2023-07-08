using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBase
{
    private Unit _target;
    private BuffData _buffData;
    private float _currentDuration;
    
    public void Init(BuffData buffData, Unit target)
    {
        _buffData = buffData;
        _target = target;
        _currentDuration = 0f;
    }

    public void Apply()
    {
        var targetStatusCount = _buffData.TargetStatuses.Length;
        for (var i = 0; i < targetStatusCount; i++)
        {
            _target.AddStatusValue(_buffData.TargetStatuses[i], _buffData.Values[i]);
        }
    }

    public void Remove()
    {
        var targetStatusCount = _buffData.TargetStatuses.Length;
        for (var i = 0; i < targetStatusCount; i++)
        {
            _target.AddStatusValue(_buffData.TargetStatuses[i], -_buffData.Values[i]);
        }
    }

    public bool UpdateTime()
    {
        _currentDuration += Time.deltaTime;

        return _currentDuration < _buffData.Duration;
    }
}
