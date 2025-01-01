using System.Collections.Generic;
using UnityEngine;

    public class InventoryManager
    {
        private readonly Dictionary<string, IContainer> containers = new Dictionary<string, IContainer>();
        private readonly ItemManager itemManager;

        public InventoryManager()
        {
            itemManager = ItemManager.Instance;
        }

        public void AddContainer(IContainer container)
        {
            if (!containers.ContainsKey(container.ContainerID))
            {
                containers.Add(container.ContainerID, container);
            }
            else
            {
                Debug.LogWarning($"Container with ID {container.ContainerID} already exists.");
            }
        }

        public void RemoveContainer(string containerID)
        {
            if (containers.ContainsKey(containerID))
            {
                containers.Remove(containerID);
            }
        }

        public IContainer GetContainer(string containerID)
        {
            return containers.TryGetValue(containerID, out var container) ? container : null;
        }

        public bool TryMoveItem(ItemObject item, IContainer sourceContainer, IContainer targetContainer, int targetX, int targetY)
        {
            if (sourceContainer == null || targetContainer == null || item == null)
            {
                return false;
            }

            if (!sourceContainer.Grid.RemoveItem(item))
            {
                return false;
            }

            if (!targetContainer.Grid.PlaceItem(item, targetX, targetY))
            {
                // If placement fails, try to put the item back in the source container
                sourceContainer.Grid.PlaceItem(item, 0, 0);
                return false;
            }

            return true;
        }

        public bool TryAddItem(ItemObject item, string containerID, int x, int y)
        {
            if (containers.TryGetValue(containerID, out var container))
            {
                return container.Grid.PlaceItem(item, x, y);
            }
            return false;
        }

        public bool TryRemoveItem(ItemObject item, string containerID)
        {
            if (containers.TryGetValue(containerID, out var container))
            {
                return container.Grid.RemoveItem(item);
            }
            return false;
        }

        public IReadOnlyDictionary<string, IContainer> GetContainers()
        {
            return containers;
        }
    }