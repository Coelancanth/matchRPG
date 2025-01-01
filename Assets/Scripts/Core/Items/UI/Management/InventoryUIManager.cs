using UnityEngine;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject containerUIPrefab;
    [SerializeField] private Transform containersParent;
    [SerializeField] private Vector2Int groundGridSize = new Vector2Int(10, 10);
    [SerializeField] private Vector2Int backpackGridSize = new Vector2Int(8, 8);

    private InventoryManager inventoryManager;
    private Dictionary<string, ContainerUI> containerUIs;

    private void Awake()
    {
        inventoryManager = new InventoryManager();
        containerUIs = new Dictionary<string, ContainerUI>();

        // Initialize default containers
        InitializeDefaultContainers();
    }

    private void InitializeDefaultContainers()
    {
        // Create ground container
        var groundGrid = new InventoryGrid(groundGridSize.x, groundGridSize.y);
        var groundContainer = new Container("ground", ContainerType.Ground, groundGrid);
        inventoryManager.AddContainer(groundContainer);
        CreateContainerUI(groundContainer);

        // Create backpack container
        var backpackGrid = new InventoryGrid(backpackGridSize.x, backpackGridSize.y);
        var backpackContainer = new Container("backpack", ContainerType.Backpack, backpackGrid);
        inventoryManager.AddContainer(backpackContainer);
        CreateContainerUI(backpackContainer);
    }

    private void CreateContainerUI(IContainer container)
    {
        GameObject containerUIObj = Instantiate(containerUIPrefab, containersParent);
        ContainerUI containerUI = containerUIObj.GetComponent<ContainerUI>();
        
        // Set container name
        containerUIObj.name = $"Container_{container.ContainerID}";
        
        containerUI.Initialize(container);
        containerUIs.Add(container.ContainerID, containerUI);

        // Set initial position based on container type
        SetContainerPosition(container.Type, containerUIObj.GetComponent<RectTransform>());
    }

    private void SetContainerPosition(ContainerType type, RectTransform rectTransform)
    {
        switch (type)
        {
            case ContainerType.Ground:
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0, 0);
                rectTransform.anchoredPosition = new Vector2(10, 10);
                break;
            case ContainerType.Backpack:
                rectTransform.anchorMin = new Vector2(1, 0);
                rectTransform.anchorMax = new Vector2(1, 0);
                rectTransform.pivot = new Vector2(1, 0);
                rectTransform.anchoredPosition = new Vector2(-10, 10);
                break;
        }
    }

    public void RefreshAllUI()
    {
        foreach (var containerUI in containerUIs.Values)
        {
            containerUI.RefreshUI();
        }
    }

    public void ShowContainerUI(string containerID)
    {
        if (containerUIs.TryGetValue(containerID, out var containerUI))
        {
            containerUI.gameObject.SetActive(true);
        }
    }

    public void HideContainerUI(string containerID)
    {
        if (containerUIs.TryGetValue(containerID, out var containerUI))
        {
            containerUI.gameObject.SetActive(false);
        }
    }

    public void ToggleContainerUI(string containerID)
    {
        if (containerUIs.TryGetValue(containerID, out var containerUI))
        {
            containerUI.gameObject.SetActive(!containerUI.gameObject.activeSelf);
        }
    }

    public InventoryManager GetInventoryManager()
    {
        return inventoryManager;
    }
} 