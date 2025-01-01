using UnityEngine;

namespace Core.Items.Interfaces
{
    /// <summary>
    /// Base interface for all item components
    /// </summary>
    public interface IItemComponent
    {
        /// <summary>
        /// The type of the component
        /// </summary>
        Core.Items.Models.ItemComponentType Type { get; }
        
        /// <summary>
        /// Execute the component's primary action
        /// </summary>
        /// <param name="item">The item this component belongs to</param>
        void Execute(Core.Items.Models.ItemObject item);
    }
} 