using System;
using System.Collections.Generic;
using UnityEngine;
using Core.Items.Models;

namespace Core.Items.Data
{
    /// <summary>
    /// ScriptableObject that defines the base data for an item type
    /// </summary>
    [CreateAssetMenu(fileName = "New Item Definition", menuName = "Game/Items/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        #region Serialized Fields
        [Header("Basic Information")]
        [SerializeField] private string _groupId;
        [SerializeField] private string _subGroupId;
        [SerializeField] private string _itemName;
        [SerializeField, TextArea] private string _description;
        [Tooltip("真实描述（用于需要技能识别的物品")]
        [SerializeField, TextArea] private string _altDescription;
        [SerializeField] private List<Sprite> _icons = new List<Sprite>();

        [Header("Properties")]
        [SerializeField] private float _weight;
        [SerializeField] private float _value;
        [SerializeField] private float _durability;
        [SerializeField] private float _degradePerUse;
        [SerializeField] private string _degradeTreasureId;

        [Header("Conditions")]
        [Tooltip("装备上这个物品能给玩家带来的状态")]
        [SerializeField] private List<string> _equipConditions = new List<string>();
        [Tooltip("拥有这个物品（带在身上即可）能给玩家带来的状态")]
        [SerializeField] private List<string> _possessConditions = new List<string>();
        [Tooltip("使用这个物品能给玩家带来的状态")]
        [SerializeField] private List<string> _useConditions = new List<string>();

        [Header("Inventory")]
        [SerializeField] private Vector2Int _capacities;
        [SerializeField] private List<string> _equipSlots = new List<string>();
        [SerializeField] private bool _isLocked;
        [SerializeField] private bool _isStackable;
        [SerializeField] private int _stackSize = 1;
        [SerializeField] private int _slotDepth = 1;

        [Header("Components")]
        [SerializeReference] private List<ItemComponentDefinition> _components = new List<ItemComponentDefinition>();
        #endregion

        #region Properties
        public string GroupID => _groupId;
        public string SubGroupID => _subGroupId;
        public string Name => _itemName;
        public string Description => _description;
        public string AltDescription => _altDescription;
        public IReadOnlyList<Sprite> Icons => _icons;
        public float Weight => _weight;
        public float Value => _value;
        public float Durability => _durability;
        public float DegradePerUse => _degradePerUse;
        public string DegradeTreasureID => _degradeTreasureId;
        public IReadOnlyList<string> EquipConditions => _equipConditions;
        public IReadOnlyList<string> PossessConditions => _possessConditions;
        public IReadOnlyList<string> UseConditions => _useConditions;
        public Vector2Int Capacities => _capacities;
        public IReadOnlyList<string> EquipSlots => _equipSlots;
        public bool IsLocked => _isLocked;
        public bool IsStackable => _isStackable;
        public int StackSize => _stackSize;
        public int SlotDepth => _slotDepth;
        public IReadOnlyList<ItemComponentDefinition> Components => _components;
        #endregion

        #region Public Methods
        /// <summary>
        /// Create an ItemModel instance from this definition
        /// </summary>
        public Models.ItemModel CreateModel()
        {
            return new Models.ItemModel(
                _groupId,
                _subGroupId,
                _itemName,
                _description,
                _altDescription,
                new List<Sprite>(_icons),
                _weight,
                _value,
                _durability,
                _degradePerUse,
                _degradeTreasureId,
                new List<string>(_equipConditions),
                new List<string>(_possessConditions),
                new List<string>(_useConditions),
                _capacities,
                new List<string>(_equipSlots),
                _isLocked,
                _isStackable,
                _stackSize,
                _slotDepth,
                new Dictionary<string, object>()
            );
        }
        #endregion

        #region Validation
        private void OnValidate()
        {
            // Ensure stack size is at least 1
            _stackSize = Mathf.Max(1, _stackSize);
            
            // Ensure slot depth is at least 1
            _slotDepth = Mathf.Max(1, _slotDepth);
            
            // Ensure non-stackable items have stack size of 1
            if (!_isStackable)
            {
                _stackSize = 1;
            }

            // Validate components
            if (_components != null)
            {
                for (int i = _components.Count - 1; i >= 0; i--)
                {
                    if (_components[i] == null)
                    {
                        _components.RemoveAt(i);
                    }
                }
            }
        }
        #endregion
    }
} 