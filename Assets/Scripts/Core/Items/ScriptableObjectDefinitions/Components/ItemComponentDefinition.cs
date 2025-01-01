using UnityEngine;
using System;

namespace Core.Items.Data
{
    /// <summary>
    /// Base class for all item component definitions
    /// </summary>
    [Serializable]
    public abstract class ItemComponentDefinition
    {
        [SerializeField] protected ItemComponentType _componentType;
        
        public ItemComponentType ComponentType => _componentType;

        /// <summary>
        /// Create a component instance from this definition
        /// </summary>
        public abstract IItemComponent CreateComponent();
    }
} 