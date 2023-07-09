using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ColliderType
{
    Self,
    Box,
    Sphere,
}

public enum EffectType
{
    PhysicalDamage, // 물리 데미지
    Heal,           // 치료
    Buff,           // (디)버프
}

public static class BattleManager
{
    public struct EffectData
    {
        public Unit Sender;
        public EffectType[] EffectTypes;
        public string[] EffectValues;
    }
    
    public struct AttackData
    {
        public Unit Attacker;
        public float Damage;
    }
    
    private static int _hittableLayer;
    
    public static void Init()
    {
        _hittableLayer = 1 << LayerMask.NameToLayer("Unit") | 1 << LayerMask.NameToLayer("HittableObject");
    }
    
    public static List<HealthComponent> GetBoxCollidedHealthComponent(Vector3 center, Vector3 size, Quaternion rotation, Transform[] exception)
    {
        var result = new List<HealthComponent>();

#if UNITY_EDITOR
        Utility.DrawWireCube(center, size, rotation, Color.green, 3);
#endif
        
        var hits = Physics.BoxCastAll(center, size * 0.5f, Vector3.up, rotation, 0, _hittableLayer);
        foreach (var hit in hits)
        {
            var hitTransform = hit.transform;
            if (exception.Contains(hitTransform))
            {
                // except
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

    public static List<HealthComponent> GetSphereCollidedHealthComponent(Vector3 center, float radius, Transform[] exception)
    {
        var result = new List<HealthComponent>();

#if UNITY_EDITOR
        Utility.DrawSphere(center, radius, Color.green, 3);
#endif
        
        var hits = Physics.SphereCastAll(center, radius, Vector3.up, 0, _hittableLayer);
        foreach (var hit in hits)
        {
            var hitTransform = hit.transform;
            if (exception.Contains(hitTransform))
            {
                // except
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
            var effectValue = effectData.EffectValues[i];
            switch (effectData.EffectTypes[i])
            {
                case EffectType.PhysicalDamage:
                    if (float.TryParse(effectValue,out var value))
                    {
                        receiver.Hit(new AttackData()
                        {
                            Attacker = sender,
                            Damage = sender.AttackPower * value,
                        });
                    }
                    break;
                
                case EffectType.Heal:
                    break;
                
                case EffectType.Buff:
                    if (receiver.TryGetComponent<Unit>(out var unit))
                    {
                        var buff = new BuffBase();
                        buff.Init(GameManager.Instance.TableContainer.GetTable<BuffTable>().GetData(effectValue), unit);
                        unit.AddBuff(buff);
                    }
                    break;
            }
        }
    }
}
