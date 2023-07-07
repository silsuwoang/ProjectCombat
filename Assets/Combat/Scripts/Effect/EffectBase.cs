using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase
{
    
    public Unit User;
    protected HealthComponent Target;
    protected float Value;

    public void Init(Unit user, HealthComponent target, float value)
    {
        User = user;
        Target = target;
        Value = value;
    }
    
}
