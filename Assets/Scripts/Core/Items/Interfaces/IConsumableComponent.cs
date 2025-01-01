using System.Collections.Generic;

namespace Core.Items.Interfaces
{
    /// <summary>
    /// Interface for consumable item components
    /// </summary>
    public interface IConsumableComponent : IItemComponent
    {
        /// <summary>
        /// Number of uses remaining
        /// </summary>
        int RemainingUses { get; }
        
        /// <summary>
        /// Maximum number of uses
        /// </summary>
        int MaxUses { get; }
        
        /// <summary>
        /// Time in seconds it takes to consume this item
        /// </summary>
        float ConsumptionTime { get; }
        
        /// <summary>
        /// Whether this item can be consumed while moving
        /// </summary>
        bool CanConsumeWhileMoving { get; }
        
        /// <summary>
        /// Effects that are applied when consuming this item
        /// </summary>
        IReadOnlyList<ConsumableEffect> Effects { get; }
        
        /// <summary>
        /// Consume the item and apply its effects
        /// </summary>
        /// <returns>True if successfully consumed, false otherwise</returns>
        bool Consume();
        
        /// <summary>
        /// Check if the item can be consumed
        /// </summary>
        /// <param name="reason">The reason why the item cannot be consumed, if any</param>
        /// <returns>True if the item can be consumed, false otherwise</returns>
        bool CanConsume(out string reason);
    }

    /// <summary>
    /// Represents an effect that can be applied by consuming an item
    /// </summary>
    public struct ConsumableEffect
    {
        /// <summary>
        /// The type of effect
        /// </summary>
        public ConsumableEffectType Type { get; set; }
        
        /// <summary>
        /// The magnitude of the effect
        /// </summary>
        public float Magnitude { get; set; }
        
        /// <summary>
        /// Duration of the effect in seconds (0 for instant effects)
        /// </summary>
        public float Duration { get; set; }
    }

    /// <summary>
    /// Types of effects that can be applied by consuming items
    /// </summary>
    public enum ConsumableEffectType
    {
        Health,
        Stamina,
        Hunger,
        Thirst,
        Temperature,
        StatusEffect,
        Buff,
        Debuff
    }
} 