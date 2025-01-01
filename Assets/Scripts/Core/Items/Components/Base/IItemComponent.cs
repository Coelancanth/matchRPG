using UnityEngine;

    /// <summary>
    /// Base interface for all item components
    /// </summary>
    public interface IItemComponent
    {
        /// <summary>
        /// The type of the component
        /// </summary>
        ItemComponentType Type { get; }
        
        /// <summary>
        /// Execute the component's primary action
        /// </summary>
        /// <param name="item">The item this component belongs to</param>
        void Execute(ItemObject item);
    }