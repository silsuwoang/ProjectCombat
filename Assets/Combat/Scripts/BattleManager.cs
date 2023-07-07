using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderType
{
    None,
    Box,
    Sphere,
}

public enum EffectType
{
    PhysicalDamage,
    Heal,
    Buff,
}

public static class BattleManager
{
    public struct AttackData
    {
        public Unit Attacker;
        public float Damage;
    }
    
    public struct EffectData
    {
        public Unit Sender;
        public HealthComponent Receiver;
        public EffectType[] EffectTypes;
        public float[] EffectValues;
    }
    
    public struct DamageData
    {
        public Unit Attacker;
        public float Damage;
    }

    private static int _hittableLayer;
    
    public static void Init()
    {
        _hittableLayer = 1 << LayerMask.NameToLayer("Unit") | 1 << LayerMask.NameToLayer("HittableObject");
    }
    
    public static List<HealthComponent> GetCollidedHealthComponent(Unit unit, ColliderType colliderType, float width, float depth)
    {
        var result = new List<HealthComponent>();
        var transform = unit.transform;
        var size = new Vector3(width, 2, depth);
        var halfSize = size * 0.5f;
        var center = transform.position + transform.forward * (halfSize.z);
        center.y += 1f;

        Utility.DrawWireCube(center, size, transform.rotation, Color.green, 3);
        
        var hits = Physics.BoxCastAll(center, halfSize, transform.forward, transform.rotation, 0, _hittableLayer);
        foreach (var hit in hits)
        {
            var hitTransform = hit.transform;
            if (transform == hitTransform)
            {
                // self
                continue;
            }

            if (!hitTransform.TryGetComponent<HealthComponent>(out var comp))
            {
                continue;
            }

            result.Add(comp);
        }

        return result;
    }

    public static void SendEffect(Unit sender, HealthComponent receiver, EffectData effectData)
    {
        var effectCount = effectData.EffectTypes.Length;
        for (var i = 0; i < effectCount; i++)
        {
            switch (effectData.EffectTypes[i])
            {
                case EffectType.PhysicalDamage:
                    receiver.Hit(new AttackData()
                    {
                        Attacker = sender,
                        Damage = sender.AttackPower * effectData.EffectValues[i],
                    });
                    break;
                case EffectType.Heal:
                    break;
                case EffectType.Buff:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            // receiver.Hit();
        }
    }
}
