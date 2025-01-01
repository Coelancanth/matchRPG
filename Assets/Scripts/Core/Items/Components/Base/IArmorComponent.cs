    /// <summary>
    /// Interface for armor item components
    /// </summary>
    public interface IArmorComponent : IItemComponent
    {
        /// <summary>
        /// Base armor value
        /// </summary>
        float BaseArmor { get; }
        
        /// <summary>
        /// Movement speed modifier (-1 to 1, where 0 is no modification)
        /// </summary>
        float MovementSpeedModifier { get; }
        
        /// <summary>
        /// Calculate damage reduction for incoming damage
        /// </summary>
        /// <param name="incomingDamage">The amount of incoming damage</param>
        /// <param name="damageType">The type of damage being received</param>
        /// <returns>The amount of damage after reduction</returns>
        float CalculateDamageReduction(float incomingDamage, DamageType damageType);
    }

    /// <summary>
    /// Types of damage that can be dealt or received
    /// </summary>
    public enum DamageType
    {
        Physical,
        Piercing,
        Slashing,
        Blunt,
        Fire,
        Cold,
        Lightning,
        Poison,
        Magic
    }