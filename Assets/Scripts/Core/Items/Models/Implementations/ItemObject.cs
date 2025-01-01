using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    /// <summary>
    /// Represents an instance of an item in the game
    /// </summary>
    public class ItemObject : MonoBehaviour
    {
        #region Fields
        private readonly Dictionary<Type, IItemComponent> components = new Dictionary<Type, IItemComponent>();
        #endregion

        #region Properties
        public ItemModel Model { get; private set; }
        public bool IsInitialized { get; private set; }
        #endregion

        #region Unity Lifecycle
        private void OnDestroy()
        {
            // Clean up components
            components.Clear();
        }
        #endregion

        #region Public Methods
        public void Initialize(ItemModel model)
        {
            if (IsInitialized)
            {
                Debug.LogWarning($"ItemObject {name} is already initialized.");
                return;
            }

            Model = model ?? throw new ArgumentNullException(nameof(model));
            IsInitialized = true;
        }

        public void AddComponent<T>(T component) where T : class, IItemComponent
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            var type = typeof(T);
            if (components.ContainsKey(type))
            {
                Debug.LogWarning($"Component of type {type.Name} already exists on item {name}. Replacing...");
            }

            components[type] = component;
        }

        public void RemoveComponent<T>() where T : class, IItemComponent
        {
            components.Remove(typeof(T));
        }

        public T GetComponent<T>() where T : class, IItemComponent
        {
            components.TryGetValue(typeof(T), out var component);
            return component as T;
        }

        public bool HasComponent<T>() where T : class, IItemComponent
        {
            return components.ContainsKey(typeof(T));
        }

        public IEnumerable<IItemComponent> GetAllComponents()
        {
            return components.Values;
        }

        public IEnumerable<T> GetAllComponents<T>() where T : class, IItemComponent
        {
            return components.Values.OfType<T>();
        }

        public void ExecuteComponent<T>() where T : class, IItemComponent
        {
            var component = GetComponent<T>();
            component?.Execute(this);
        }
        #endregion
    }