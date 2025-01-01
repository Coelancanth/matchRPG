using System;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private string m_GroupID;
        [SerializeField] private string m_SubGroupID;
        [SerializeField] private string _Name;
        [SerializeField, TextArea] private string m_Description;

        [Tooltip ("真实描述（用于需要技能识别的物品")]
        [SerializeField, TextArea] private string m_AltDescription;
        [SerializeField] private List<Sprite> m_Icons = new List<Sprite>();

        [Header("Properties")]
        [SerializeField] private float m_Weight;
        [SerializeField] private float m_Value;
        [SerializeField] private float m_Durability;
        [SerializeField] private float m_DegradePerUse;
        [SerializeField] private string m_DegradeTreasureID;

        [Header("Conditions")]
        [Tooltip ("装备上这个物品能给玩家带来的状态")]
        [SerializeField] private List<string> m_EquipConditions = new List<string>();
        [Tooltip ("拥有这个物品（带在身上即可）能给玩家带来的状态")]
        [SerializeField] private List<string> m_PossessConditions = new List<string>();
        [Tooltip ("使用这个物品能给玩家带来的状态")]
        [SerializeField] private List<string> m_UseConditions = new List<string>();

        [Header("Inventory")]
        [SerializeField] private Vector2Int m_Capacities;
        [SerializeField] private List<string> m_EquipSlots = new List<string>();
        [SerializeField] private bool m_IsLocked;
        [SerializeField] private bool m_IsStackable;
        [SerializeField] private int m_StackSize = 1;
        [SerializeField] private int m_SlotDepth = 1;

        [Header("Components")]
        [SerializeField] private List<ItemComponentData> m_Components = new List<ItemComponentData>();
        #endregion

        #region Properties
        public string GroupID => m_GroupID;
        public string SubGroupID => m_SubGroupID;
        public string Name => _Name;
        public string Description => m_Description;
        public string AltDescription => m_AltDescription;
        public IReadOnlyList<Sprite> Icons => m_Icons;
        public float Weight => m_Weight;
        public float Value => m_Value;
        public float Durability => m_Durability;
        public float DegradePerUse => m_DegradePerUse;
        public string DegradeTreasureID => m_DegradeTreasureID;
        public IReadOnlyList<string> EquipConditions => m_EquipConditions;
        public IReadOnlyList<string> PossessConditions => m_PossessConditions;
        public IReadOnlyList<string> UseConditions => m_UseConditions;
        public Vector2Int Capacities => m_Capacities;
        public IReadOnlyList<string> EquipSlots => m_EquipSlots;
        public bool IsLocked => m_IsLocked;
        public bool IsStackable => m_IsStackable;
        public int StackSize => m_StackSize;
        public int SlotDepth => m_SlotDepth;
        public IReadOnlyList<ItemComponentData> Components => m_Components;
        #endregion

        #region Public Methods
        /// <summary>
        /// Create an ItemModel instance from this definition
        /// </summary>
        public Models.ItemModel CreateModel()
        {
            return new Models.ItemModel(
                m_GroupID,
                m_SubGroupID,
                _Name,
                m_Description,
                m_AltDescription,
                new List<Sprite>(m_Icons),
                m_Weight,
                m_Value,
                m_Durability,
                m_DegradePerUse,
                m_DegradeTreasureID,
                new List<string>(m_EquipConditions),
                new List<string>(m_PossessConditions),
                new List<string>(m_UseConditions),
                m_Capacities,
                new List<string>(m_EquipSlots),
                m_IsLocked,
                m_IsStackable,
                m_StackSize,
                m_SlotDepth,
                new Dictionary<string, object>()
            );
        }
        #endregion

        #region Validation
        private void OnValidate()
        {
            // Ensure stack size is at least 1
            m_StackSize = Mathf.Max(1, m_StackSize);
            
            // Ensure slot depth is at least 1
            m_SlotDepth = Mathf.Max(1, m_SlotDepth);
            
            // Ensure non-stackable items have stack size of 1
            if (!m_IsStackable)
            {
                m_StackSize = 1;
            }
        }
        #endregion
    }

    /// <summary>
    /// Data for an item component
    /// </summary>
    [Serializable]
    public class ItemComponentData
    {
        public Models.ItemComponentType Type;
        public string Data;
    }
} 