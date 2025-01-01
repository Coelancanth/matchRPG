using UnityEngine;
using System.Collections.Generic;

    public class ArmorComponent : IArmorComponent
    {
        private readonly float m_BaseArmor;
        private readonly float m_MovementSpeedModifier;
        private readonly Dictionary<DamageType, float> m_DamageResistances;

        public ItemComponentType Type => ItemComponentType.Armor;
        public float BaseArmor => m_BaseArmor;
        public float MovementSpeedModifier => m_MovementSpeedModifier;

        public ArmorComponent(float baseArmor, float movementSpeedModifier, Dictionary<DamageType, float> damageResistances)
        {
            m_BaseArmor = baseArmor;
            m_MovementSpeedModifier = Mathf.Clamp(movementSpeedModifier, -1f, 1f);
            m_DamageResistances = damageResistances ?? new Dictionary<DamageType, float>();
        }

        public void Execute(ItemObject item)
        {
            // 执行防具的主要动作，比如激活特殊防护效果
            Debug.Log($"Executing armor action for {item.Model.Name}");
        }

        public float CalculateDamageReduction(float incomingDamage, DamageType damageType)
        {
            // 基础伤害减免
            float reduction = m_BaseArmor;

            // 应用特定类型的伤害抗性
            if (m_DamageResistances.TryGetValue(damageType, out float resistance))
            {
                reduction *= (1f + resistance);
            }

            // 确保不会完全免疫伤害（除非特殊设计需要）
            float minDamage = incomingDamage * 0.1f; // 至少承受10%的伤害
            float reducedDamage = Mathf.Max(minDamage, incomingDamage - reduction);

            return reducedDamage;
        }
    }