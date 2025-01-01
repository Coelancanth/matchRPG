using System;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// Represents the base data structure for all items in the game
    /// </summary>
    [Serializable]
    public class ItemModel
    {
        #region Properties
        public string GroupID { get; private set; }
        public string SubGroupID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string AltDescription { get; private set; }
        public List<Sprite> Icons { get; private set; }
        public float Weight { get; private set; }
        public float Value { get; private set; }
        public float Durability { get; private set; }
        public float DegradePerUse { get; private set; }
        public string DegradeTreasureID { get; private set; }
        public List<string> EquipConditions { get; private set; }
        public List<string> PossessConditions { get; private set; }
        public List<string> UseConditions { get; private set; }
        public Vector2Int Capacities { get; private set; }
        public List<string> EquipSlots { get; private set; }
        public bool IsLocked { get; private set; }
        public bool IsStackable { get; private set; }
        public int StackSize { get; private set; }
        public int SlotDepth { get; private set; }
        public Dictionary<string, object> CustomData { get; private set; }
        #endregion

        #region Constructor
        public ItemModel(
            string groupID,
            string subGroupID,
            string name,
            string description,
            string altDescription,
            List<Sprite> icons,
            float weight,
            float value,
            float durability,
            float degradePerUse,
            string degradeTreasureID,
            List<string> equipConditions,
            List<string> possessConditions,
            List<string> useConditions,
            Vector2Int capacities,
            List<string> equipSlots,
            bool isLocked,
            bool isStackable,
            int stackSize,
            int slotDepth,
            Dictionary<string, object> customData)
        {
            GroupID = groupID;
            SubGroupID = subGroupID;
            Name = name;
            Description = description;
            AltDescription = altDescription;
            Icons = icons ?? new List<Sprite>();
            Weight = weight;
            Value = value;
            Durability = durability;
            DegradePerUse = degradePerUse;
            DegradeTreasureID = degradeTreasureID;
            EquipConditions = equipConditions ?? new List<string>();
            PossessConditions = possessConditions ?? new List<string>();
            UseConditions = useConditions ?? new List<string>();
            Capacities = capacities;
            EquipSlots = equipSlots ?? new List<string>();
            IsLocked = isLocked;
            IsStackable = isStackable;
            StackSize = stackSize;
            SlotDepth = slotDepth;
            CustomData = customData ?? new Dictionary<string, object>();
        }
        #endregion

        #region Public Methods
        public T GetCustomData<T>(string key, T defaultValue = default)
        {
            if (CustomData.TryGetValue(key, out object value) && value is T typedValue)
            {
                return typedValue;
            }
            return defaultValue;
        }

        public void SetCustomData<T>(string key, T value)
        {
            CustomData[key] = value;
        }
        #endregion
    }