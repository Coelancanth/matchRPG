using UnityEngine;
using Core.Items.Interfaces;
using Core.Items.Models;

namespace Core.Items.Components
{
    public class WeaponComponent : IWeaponComponent
    {
        private readonly float m_BaseDamage;
        private readonly float m_AttackSpeed;
        private readonly float m_Range;
        private readonly DamageType m_DamageType;

        public ItemComponentType Type => ItemComponentType.Weapon;
        public float BaseDamage => m_BaseDamage;
        public float AttackSpeed => m_AttackSpeed;
        public float Range => m_Range;
        public DamageType DamageType => m_DamageType;

        public WeaponComponent(float baseDamage, float attackSpeed, float range, DamageType damageType)
        {
            m_BaseDamage = baseDamage;
            m_AttackSpeed = attackSpeed;
            m_Range = range;
            m_DamageType = damageType;
        }

        public void Execute(ItemObject item)
        {
            // 执行武器的主要动作，比如尝试攻击最近的敌人
            Debug.Log($"Executing weapon action for {item.Model.Name}");
        }

        public float Attack(GameObject target)
        {
            return 0;
        }
    }
} 