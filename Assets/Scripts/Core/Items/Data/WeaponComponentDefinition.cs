using UnityEngine;
using Core.Items.Components;
using Core.Items.Interfaces;
using System;

namespace Core.Items.Data
{
    [Serializable]
    public class WeaponComponentDefinition : ItemComponentDefinition
    {
        [Header("Weapon Properties")]
        [SerializeField] private float _baseDamage = 10f;
        [SerializeField] private float _attackSpeed = 1f;
        [SerializeField] private float _range = 2f;
        [SerializeField] private DamageType _damageType = DamageType.Physical;

        public WeaponComponentDefinition()
        {
            _componentType = Models.ItemComponentType.Weapon;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _baseDamage = Mathf.Max(0f, _baseDamage);
            _attackSpeed = Mathf.Max(0.1f, _attackSpeed);
            _range = Mathf.Max(0.1f, _range);
        }
#endif

        public override IItemComponent CreateComponent()
        {
            return new WeaponComponent(_baseDamage, _attackSpeed, _range, _damageType);
        }
    }
} 