## 2. **UI Layout for Inventory and Paper Doll**

  1. **Inventory Panel**
    - Create a **UI Panel** in Unity for the “Items on the Ground” Inventory.
    - The ground will be separate grids.
    - The item can be placed on the ground, the occupied grids are determined by the item's capacity property.

# Container(Grid) Data Structure
## GridCell
```csharp
public class GridCell
{
  public bool IsOccupied {get; private set;}
  public ItemObject OccupyingItem {get; private set;}

  public void Occupy(ItemObject item)
  {
    IsOccupied = true;
    OccupyingItem = item;
  }

  public void Vacate()
  {
    IsOccupied = false;
    OccupyingItem = null;
  }
}
```

## IInventoryGrid
```csharp
public interface IInventoryGrid
{
  int GridWidth {get;}
  int GridHeight {get;}
  bool CanPlaceItem(ItemObject item, int startX, int startY);
  bool PlaceItem(ItemObject item, int startX, int startY);
  bool RemoveItem(ItemObject item);
  GridCell[,] GetGrid();
}
``` 

## InventoryGrid
```csharp
public class InventoryGrid : IInventoryGrid
{
  public int GridWidth {get;}
  public int GridHeight {get;}
  private GridCell[,] gridCells;
  
  public event Action OnGridChanged;

  public InventoryGrid(int gridWidth, int gridHeight)
  {
    GridWidth = gridWidth;
    GridHeight = gridHeight;
    gridCells = new GridCell[gridWidth, gridHeight];
    for (int x = 0; x < gridWidth; x++)
    {
      for (int y = 0; y < gridHeight; y++)
      {
        gridCells[x, y] = new GridCell();
      }
    }
  }

  public bool CanPlaceItem(ItemObject item, int startX, int startY)
  {
    if (startX < 0 || startX + item.Model.Capacities.x > GridWidth || startY < 0 || startY + item.Model.Capacities.y > GridHeight)
      return false;

    for (int x = startX; x <  startX + item.Model.Capacities.x; x++)
    {
      for (int y = startY; y < startY + item.Model.Capacities.y; y++)
      {
        if (gridCells[x, y].IsOccupied)
          return false;
      }
    }
    return true;
  }

  public bool PlaceItem(ItemObject item, int startX, int startY)
  {
    if (!CanPlaceItem(item, startX, startY))
      return false;
    for (int x = startX; x < startX + item.Model.Capacities.x; x++)
    {
      for (int y = startY; y < startY + item.Model.Capacities.y; y++)
      {
        gridCells[x, y].Occupy(item);
      }
    }
    OnGridChanged?.Invoke();
    return true;
  }

  public void RemoveItem(ItemObject item)
  {
    for (int x = 0; x < GridWidth; x++)
    {
      for (int y = 0; y < GridHeight; y++)
      {
        if (gridCells[x, y].OccupyingItem == item)
          gridCells[x, y].Vacate();
      }
    }
    OnGridChanged?.Invoke();
  }

  public GridCell[,] GetGrid()
  {
    return gridCells;
  }
}
```

## IContainer
```csharp
public interface IContainer
{
  string ContainerID {get;}
  ContainerType Type {get;}
  InventoryGrid Grid {get;}
}
```


## Container
```csharp
public enum ContainerType
{
  Backpack,
  Ground,
}

public class Container
{
  public string ContainerID {get; private set;}
  public ContainerType Type {get; private set;}
  public InventoryGrid Grid {get; private set;}

  public Container(string containerID, ContainerType type, InventoryGrid grid)
  {
    ContainerID = containerID;
    Type = type;
    Grid = grid;
  }
}
```


## InventoryManager
```csharp
// Manage multiple containers, handle item swap between containers, add/remove container, etc.
public class InventoryManager
{
  private readonly Dictionary<string, IContainer> containers;

  public InventoryManager()
  {
    containers = new Dictionary<string, IContainer>();
  }
  
  public void AddContainer(IContainer container)
  {
    if (!containers.ContainsKey(container.ContainerID))
      containers.Add(container.ContainerID, container);
  }
  
  public IContainer GetContainer(string containerID)
  {
    containers.TryGetValue(containerID, out var container);
    return container;
  }

  public bool MoveItem(string sourceContainerID, string targetContainerID, ItemObject item, int startX, int startY)
  {
    var sourceContainer = GetContainer(sourceContainerID);
    var targetContainer = GetContainer(targetContainerID);
    
    if (sourceContainer != null && targetContainer != null)
    {
      sourceContainer.Grid.RemoveItem(item);
      if (targetContainer.Grid.PlaceItem(item, startX, startY))
        return true;
      else
      {
        sourceContainer.Grid.PlaceItem(item, startX, startY);
        return false;
      }
    }
    return false;
  }
}
```


## ContainerUI
```cs
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject itemIconPrefab;

    private IContainer container;
    private GameObject[,] cellObjects;

    public void Initialize(IContainer container)
    {
        this.container = container;
        container.Grid.OnGridChanged += RefreshUI;
        CreateGrid();
        RefreshUI();
    }

    private void CreateGrid()
    {
        int width = container.Grid.GridWidth;
        int height = container.Grid.GridHeight;
        cellObjects = new GameObject[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                GameObject cell = Instantiate(gridCellPrefab, gridParent);
                cell.transform.localPosition = new Vector3(x * 50, y * -50, 0); // 调整位置
                cellObjects[x, y] = cell;
            }
    }

    public void RefreshUI()
    {
        // 清除旧的物品图标
        foreach (Transform child in gridParent)
            if (child.CompareTag("ItemIcon"))
                Destroy(child.gameObject);

        GridCell[,] cells = container.Grid.GetGridCells();

        for (int x = 0; x < container.Grid.GridWidth; x++)
            for (int y = 0; y < container.Grid.GridHeight; y++)
            {
                GridCell cell = cells[x, y];
                if (cell.IsOccupied && cell.OccupyingItem != null)
                {
                    // 仅在物品的起始格子显示图标
                    if (IsItemStartingCell(cell.OccupyingItem, x, y))
                    {
                        GameObject icon = Instantiate(itemIconPrefab, gridParent);
                        icon.transform.localPosition = new Vector3(x * 50, y * -50, 0); // 调整位置
                        Image img = icon.GetComponent<Image>();
                        if (img != null)
                            img.sprite = cell.OccupyingItem.Icon;
                        icon.tag = "ItemIcon";
                    }
                }
            }
    }

    private bool IsItemStartingCell(Item item, int x, int y)
    {
        // 需要在 Item 类中记录物品的起始位置，或通过其他方式确定
        // 这里简化处理，假设所有物品仅在一个格子显示图标
        return true;
    }

    private void OnDestroy()
    {
        if (container != null && container.Grid != null)
            container.Grid.OnGridChanged -= RefreshUI;
    }
}
```

## InventoryUIManager
```csharp
using UnityEngine;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject containerUIPrefab;
    [SerializeField] private Transform containersParent;

    private InventoryManager inventoryManager;
    private Dictionary<string, ContainerUI> containerUIs;

    void Start()
    {
        inventoryManager = new InventoryManager();
        containerUIs = new Dictionary<string, ContainerUI>();

        // 初始化容器并创建对应的 UI
        foreach (var container in inventoryManager.GetAllContainers())
        {
            CreateContainerUI(container);
        }
    }

    private void CreateContainerUI(IContainer container)
    {
        GameObject containerUIObj = Instantiate(containerUIPrefab, containersParent);
        ContainerUI containerUI = containerUIObj.GetComponent<ContainerUI>();
        containerUI.Initialize(container);
        containerUIs.Add(container.ContainerID, containerUI);
    }

    public void RefreshAllUI()
    {
        foreach (var containerUI in containerUIs.Values)
            containerUI.RefreshUI();
    }

    public void ShowContainerUI(string containerID)
    {
        if (containerUIs.TryGetValue(containerID, out var containerUI))
            containerUI.gameObject.SetActive(true);
    }

    public void HideContainerUI(string containerID)
    {
        if (containerUIs.TryGetValue(containerID, out var containerUI))
            containerUI.gameObject.SetActive(false);
    }
}

```