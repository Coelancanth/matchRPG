using UnityEngine;

using System;
using System.Collections.Generic;

namespace Core.Items.Data
{
    [Serializable]
    public class ArmorComponentDefinition : ItemComponentDefinition
    {
        [Header("Armor Properties")]
        [SerializeField] private float _baseArmor = 5f;
        [SerializeField, Range(-1f, 1f)] private float _movementSpeedModifier = 0f;
        [SerializeField] private DamageResistance[] _damageResistances;

        public ArmorComponentDefinition()
        {
            _componentType = ItemComponentType.Armor;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _baseArmor = Mathf.Max(0f, _baseArmor);
        }
#endif

        public override IItemComponent CreateComponent()
        {
            var resistances = new Dictionary<DamageType, float>();
            if (_damageResistances != null)
            {
                foreach (var resistance in _damageResistances)
                {
                    resistances[resistance.Type] = resistance.Value;
                }
            }

            return new ArmorComponent(_baseArmor, _movementSpeedModifier, resistances);
        }
    }

    [Serializable]
    public struct DamageResistance
    {
        public DamageType Type;
        [Range(0f, 1f)] public float Value;
    }
} 