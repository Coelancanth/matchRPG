using UnityEngine;

    /// <summary>
    /// Interface for weapon item components
    /// </summary>
    public interface IWeaponComponent : IItemComponent
    {
        /// <summary>
        /// Base damage of the weapon
        /// </summary>
        float BaseDamage { get; }
        
        /// <summary>
        /// Attack speed in attacks per second
        /// </summary>
        float AttackSpeed { get; }
        
        /// <summary>
        /// Range of the weapon in units
        /// </summary>
        float Range { get; }
        
        /// <summary>
        /// Perform an attack with this weapon
        /// </summary>
        /// <param name="target">The target of the attack</param>
        /// <returns>The actual damage dealt</returns>
        float Attack(GameObject target);
    }