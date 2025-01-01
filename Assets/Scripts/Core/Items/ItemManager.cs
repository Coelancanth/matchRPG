using System;
using System.Collections.Generic;
using Core.Items.Data;
using Core.Items.Models;
using UnityEngine;

namespace Core.Items
{
    /// <summary>
    /// Manages item creation and global item operations
    /// </summary>
    public class ItemManager : MonoBehaviour
    {
        #region Singleton
        private static ItemManager s_Instance;
        public static ItemManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<ItemManager>();
                    if (s_Instance == null)
                    {
                        var go = new GameObject("ItemManager");
                        s_Instance = go.AddComponent<ItemManager>();
                    }
                }
                return s_Instance;
            }
        }
        #endregion

        #region Serialized Fields
        [SerializeField] private ItemObject m_ItemPrefab;
        [SerializeField] private List<ItemDefinition> m_ItemDefinitions = new List<ItemDefinition>();
        #endregion

        #region Private Fields
        private readonly Dictionary<string, ItemDefinition> m_ItemDefinitionMap = new Dictionary<string, ItemDefinition>();
        private readonly List<ItemObject> m_ActiveItems = new List<ItemObject>();
        #endregion

        #region Properties
        public IReadOnlyList<ItemObject> ActiveItems => m_ActiveItems;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeItemDefinitions();
        }

        private void OnDestroy()
        {
            if (s_Instance == this)
            {
                s_Instance = null;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a new item instance from a definition
        /// </summary>
        /// <param name="definitionId">The ID of the item definition (GroupID.SubGroupID)</param>
        /// <param name="position">World position to spawn the item at</param>
        /// <param name="parent">Optional parent transform</param>
        /// <returns>The created item instance</returns>
        public ItemObject CreateItem(string definitionId, Vector3 position, Transform parent = null)
        {
            if (!m_ItemDefinitionMap.TryGetValue(definitionId, out var definition))
            {
                Debug.LogError($"No item definition found with ID: {definitionId}");
                return null;
            }

            return CreateItem(definition, position, parent);
        }

        /// <summary>
        /// Create a new item instance from a definition
        /// </summary>
        /// <param name="definition">The item definition to create from</param>
        /// <param name="position">World position to spawn the item at</param>
        /// <param name="parent">Optional parent transform</param>
        /// <returns>The created item instance</returns>
        public ItemObject CreateItem(ItemDefinition definition, Vector3 position, Transform parent = null)
        {
            if (definition == null)
            {
                Debug.LogError("Cannot create item from null definition");
                return null;
            }

            var item = Instantiate(m_ItemPrefab, position, Quaternion.identity, parent);
            item.name = $"Item_{definition.Name}";
            item.Initialize(definition.CreateModel());

            // Add components based on definition
            foreach (var componentData in definition.Components)
            {
                var component = CreateComponent(componentData);
                if (component != null)
                {
                    item.AddComponent(component);
                }
            }

            m_ActiveItems.Add(item);
            return item;
        }

        /// <summary>
        /// Destroy an item instance
        /// </summary>
        /// <param name="item">The item to destroy</param>
        public void DestroyItem(ItemObject item)
        {
            if (item != null)
            {
                m_ActiveItems.Remove(item);
                Destroy(item.gameObject);
            }
        }

        /// <summary>
        /// Get an item definition by its ID
        /// </summary>
        /// <param name="definitionId">The ID of the item definition (GroupID.SubGroupID)</param>
        /// <returns>The item definition, or null if not found</returns>
        public ItemDefinition GetItemDefinition(string definitionId)
        {
            m_ItemDefinitionMap.TryGetValue(definitionId, out var definition);
            return definition;
        }
        #endregion

        #region Private Methods
        private void InitializeItemDefinitions()
        {
            m_ItemDefinitionMap.Clear();
            foreach (var definition in m_ItemDefinitions)
            {
                var id = $"{definition.GroupID}.{definition.SubGroupID}";
                if (m_ItemDefinitionMap.ContainsKey(id))
                {
                    Debug.LogError($"Duplicate item definition ID: {id}");
                    continue;
                }
                m_ItemDefinitionMap.Add(id, definition);
            }
        }

        private Interfaces.IItemComponent CreateComponent(ItemComponentData componentData)
        {
            // TODO: Implement component creation based on type and data
            // This will need to be expanded as more component types are added
            return null;
        }
        #endregion
    }
} 