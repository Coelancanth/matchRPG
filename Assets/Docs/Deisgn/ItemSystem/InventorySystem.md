# Inventory System
The inventory system of this game is grid-based. 
This means that objects take up space in addition to their weight. 
Items may be rotated (via mouse scroll or arrow buttons) to fit better in one's inventory.
Items can be sorted/ filtered by certain standard

# Inventory Page 
- This page will be split into 3 parts:
    - Items on the ground
    - "Paper Doll" Character (visual representation of the player)
    - Description part (for later use)

## Items on the ground
- Player can pick up items on the ground
- Right click to pop up interaction menu
- They can be dragged to equip the avatar on the right
    - By dragging, the paper doll will highlight the 'slot' (body part) that the item can be equipped to
- Tooltip
## "Paper Doll" Character
- When equipped an item, the avatar on the right will change its appearance accordingly
- Player can also drag items from the avatar to the ground inventory


# O1's Implementation Suggestion

***You***: Use Neo Scavenger's equipment system as reference, please give me a high-level, step-by-step guide on how to implement the following features in unity2D:


***ChatGPT***: Below is a high-level, step-by-step outline for creating a Neo Scavenger–style, grid-based inventory system in Unity2D, including a “paper doll” (character equipment) section, ground items section, and optional description panel. This guide assumes familiarity with Unity’s UI system (Canvas, RectTransforms, EventSystem) and C# scripting.
 

## 1. **Set Up Your Data Structures**

  1. **Item Data**
    - Create a C# `Item` class or use to store item data (name, icon, dimensions, weight, equip slot type, etc.).
      - GroupID
      - SubGroupID
      - Name
      - Description
      - AltDescription (Certain items need some skills to read the 'real' description)
          - Condition (SkillID, SkillLevel)
      - Icon(List, OnTheGround, InInventory, OnHand etc.)
      - Weight
      - Value
      - Durability
      - DegradePerUser
      - DegradeTreasureID (Might have something left even if durability reaches 0)
      - EquipConditions (buffs)
      - PossessConditions (no need to be equipped)
      - UseConditions
      - Capacities (e.g 4x6)
      - EquipSlots (e.g Head, Torso, Legs, Hands, Feet)
      - IsLocked
      - IsStackable
      - StackSize
      - SlotDepth
      - Dict (Placeholder for potential complex data structure)
    - Include fields for the item’s grid size (e.g., width=2, height=3) and its orientation state (normal or rotated).
    - If the item can be worn/equipped, store the **equip slot type** (e.g., head, torso, legs, feet, etc.).
    - Model for static data
    - Object for dynamic data
    - Component for decoupling, e.g. (ItemComponent, WeaponComponent : ItemComponent)
  2. **Inventory Grid Management**
    - Create a structure that can represent the layout of your grid. For example, a 2D array or list of “grid cells” that track whether they’re occupied and by what item.
    - Each “grid cell” can be a small script or data struct with references to the item occupying it, or `null` if empty.
  3. **Character Equipment Slots**
    - For the paper doll, define a set of **slots** (e.g., HeadSlot, TorsoSlot, FeetSlot). Each slot should know what type of items it can accept (so a “FeetSlot” only accepts footwear items).
    - You can use small scripts or dedicated UI placeholders (like a dedicated `GameObject` with a `Slot` script) that references its acceptable item type.
    - Slot is actually not a "slot", but a overlap body part. Check the paper doll system in Neo Scavenger as reference.
 

## 2. **UI Layout for Inventory and Paper Doll**

  1. **Inventory Panel**
    - Create a **UI Panel** in Unity for the “Items on the Ground”(grid-based).
    - The ground will be separate grids.For the grid approach, each cell can be a `Button` or `Image` representing a slot.
  2. **Paper Doll Panel**
    - Create a **UI Panel** for the paper doll. At the simplest level, this is a background image or sprite representing the character silhouette, plus child objects that represent each slot (head, torso, arms, etc.).
    - Each slot is a UI element that can highlight or show a different sprite when an item is equipped.
  3. **Description Panel** (Optional)
    - Have a placeholder UI area for showing additional details about an item (name, stats, lore, etc.) when hovered or selected.


## 3. **Item Rotation Mechanic**

  1. **Tracking Orientation**
    - In your `Item` data, have a boolean or enum (e.g., `Orientation` = Normal or Rotated).
    - When the user presses a designated key (mouse scroll, arrow keys), flip the orientation.
    - Swapping orientation effectively swaps the item’s width and height.
  2. **Visual Representation**
    - When drawing the item in the grid UI, if rotated, visually rotate the item’s icon or sprite 90 degrees.
    - Update the logic for checking whether the item will fit in the grid when placing it.
 

## 4. **Dragging and Dropping Items**

  1. **Set Up Event System**
    - Use Unity’s [EventSystem]() with `IPointerDownHandler`, `IBeginDragHandler`, `IDragHandler`, and `IEndDragHandler` to handle drag & drop.
    - Alternatively, you can use a well-known [Drag & Drop UI approach]().
  2. **Dragging from Ground to Inventory**
    - When the user clicks and drags an item from the “ground” panel, instantiate a draggable representation ('floating item' effect).
    - During drag, highlight valid “slots” in the inventory grid (cells that are free and big enough for the item in its current orientation).
    - On drop, if dropped on a valid grid area, place the item there; otherwise, return it to its original position.
  3. **Dragging from Inventory to Paper Doll**
    - Similar approach: if the item has an equip slot type that matches one of the character’s equipment slots, highlight that slot on mouse-over.
    - If dropped on that slot, equip the item to that slot (remove from grid, visually appear on the paper doll).
  4. **Dragging from Paper Doll to Inventory**
    - Reverse logic: drag an equipped item from a slot. If dropped back onto the grid, place it. If dropped anywhere invalid, snap back.
 

## 5. **Highlighting Valid Slots**

  - When dragging an item, update a highlight or color overlay:
    - **Inventory Grid Cells**: highlight green if the item will fit, red if it won’t.
    - **Equipment Slots**: highlight if the dragged item’s “equip slot type” matches the slot.This feedback is critical so that the player understands what’s possible or not.
 

## 6. **Equipping Items (Paper Doll Appearance)**

  1. **Appearance Changes**
    - Each slot on the paper doll can have an “Equipped Item Sprite” or a mask layer that changes to display the item’s sprite or texture when equipped.
    - Alternatively, if your character is a full 2D sprite, you might swap out individual layers or sprite renderers for each body part.
  2. **Data Binding**
    - When the player drops an item onto the slot, store a reference to that `Item` object in the slot’s script.
    - Update the paper doll’s visual representation accordingly.
  3. **Unequipping**
    - If the item is removed from the slot, clear that reference, revert the paper doll’s visuals to a default state for that slot.
 

## 7. **Right-Click Context Menus & Tooltips**

  1. **Context Menu**
    - For each item (on ground or in inventory), handle a right-click to open a small context menu (Unity UI Panel) with options like “Examine,” “Pick Up,” “Equip,” “Use,” etc.
    - The exact approach can be done by instantiating a prefab for the menu and positioning it at the mouse cursor, or using a single floating panel that gets updated.
  2. **Tooltip System**
    - On hover (or pointer enter), display a small panel showing item name, weight, etc.
    - This can be a UI `Panel` that follows the mouse, or a fixed position in your description area.
    - Use `OnPointerEnter` and `OnPointerExit` to show/hide the tooltip with the relevant item data.
 

## 8. **Sorting and Filtering**

  1. **Filter UI**
    - Add dropdowns or toggle buttons that filter items by categories (e.g., “Weapons,” “Clothing,” “Food”).
    - When active, items not matching the filter are either hidden or grayed out.
  2. **Implementation**
    - Maintain an item list for the ground/inventory. When the filter setting changes, update the visible items.
    - You can skip the filter check for the “paper doll” portion since it’s typically only one item per slot.
 

## 9. **Putting It All Together**

  1. **Initialization**
    - On scene load, set up the player’s inventory grid (empty or partially filled).
    - Populate the “ground items” grid/list with items in range.
  2. **User Interaction Flow**
    - Player sees items on the ground, can pick them up or right-click for a context menu.
    - On drag, highlight valid slots or grid cells.
    - Drop item to inventory or equip slot.
    - If equipped, paper doll updates visuals.
    - On hover or right-click, show tooltip or context menu.
  3. **Testing and Refinement**
    - Check alignment, spacing, rotation visuals, and scaling.
    - Ensure user feedback is clear (color highlights, tooltip info).
    - Confirm that no item can be placed in a slot or grid cell that doesn’t fit (dimensions and weight constraints).
 

### **Key Tips & Considerations**

  - **ScriptableObjects** can simplify item data management: you create multiple item assets in your project without replicating code.
  - **Performance**: For large grids, ensure your drag-and-drop checks are efficient (use bounding boxes, caching, etc.).
  - **Polish**: Add sound effects for picking up/dropping items, small animations for equipping, and so on.
  - **Edge Cases**: Items that cannot be rotated or only have certain orientation constraints; partial overlap with other items; removing items from stacked or nested containers.
 
This outline should give you a clear, high-level roadmap for building a Neo Scavenger–style, grid-based inventory system in Unity2D. From here, you can refine each step based on your project’s requirements and expand functionality as needed (e.g., item stats, durability, combining items, etc.).


# Item Data Structure

## ItemModel
```csharp
public class ItemModelBuilder
{
    private string groupID;
    private string subGroupID;
    private string name;
    private string description = string.Empty;
    private string altDescription = string.Empty;
    private List<string> icon = new List<string>();
    private float weight;
    private float value;
    private float durability;
    private float degradePerUser;
    private string degradeTreasureID = string.Empty;
    private List<string> equipConditions = new List<string>();
    private List<string> possessConditions = new List<string>();
    private List<string> useConditions = new List<string>();
    private Vector2Int capacities;
    private List<string> equipSlots = new List<string>();
    private bool isLocked;
    private bool isStackable;
    private int stackSize;
    private int slotDepth;
    private Dictionary<string, object> dictData = new Dictionary<string, object>();

    public ItemModelBuilder SetGroupID(string groupID) { this.groupID = groupID; return this; }
    public ItemModelBuilder SetSubGroupID(string subGroupID) { this.subGroupID = subGroupID; return this; }
    public ItemModelBuilder SetName(string name) { this.name = name; return this; }
    public ItemModelBuilder SetDescription(string description) { this.description = description; return this; }
    public ItemModelBuilder SetAltDescription(string altDescription) { this.altDescription = altDescription; return this; }
    public ItemModelBuilder SetIcon(List<string> icon) { this.icon = icon; return this; }
    public ItemModelBuilder SetWeight(float weight) { this.weight = weight; return this; }
    public ItemModelBuilder SetValue(float value) { this.value = value; return this; }
    // 其他设置方法...

    public ItemModel Build()
    {
        if (string.IsNullOrEmpty(groupID)) throw new InvalidOperationException("GroupID is required.");
        if (string.IsNullOrEmpty(subGroupID)) throw new InvalidOperationException("SubGroupID is required.");
        if (string.IsNullOrEmpty(name)) throw new InvalidOperationException("Name is required.");

        return new ItemModel(
            groupID, subGroupID, name, description, altDescription,
            icon.Select(path => new Image(path)).ToList(), // 假设Image有接受路径的构造函数
            weight, value, durability, degradePerUser,
            degradeTreasureID, equipConditions, possessConditions,
            useConditions, capacities, equipSlots,
            isLocked, isStackable, stackSize, slotDepth, dictData
        );
    }
}


```

## ItemObject
```csharp
public class ItemObject
{
    private readonly Dictionary<Type, IItemComponent> components = new Dictionary<Type, IItemComponent>();

    public ItemModel Model { get; }

    public ItemObject(ItemModel model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
    }

    public void AddComponent<T>(T component) where T : IItemComponent
    {
        if (component is null)
            throw new ArgumentNullException(nameof(component));

        components[typeof(T)] = component;
    }

    public void RemoveComponent<T>() where T : IItemComponent
    {
        components.Remove(typeof(T));
    }

    public T GetComponent<T>() where T : class, IItemComponent
    {
        components.TryGetValue(typeof(T), out var component);
        return component as T;
    }

    public IEnumerable<T> GetAllComponents<T>() where T : class, IItemComponent
    {
        return components.Values.OfType<T>();
    }
}

```
### Enum: ItemComponentType
```csharp
public enum ItemComponentType
{
  Weapon,
  Consumable,
  // Add more components as needed
}
```

## IItemComponent
```csharp
public interface IItemComponent
{
  ItemComponentType Type {get;};
  void Execute(ItemObject item);
}
```

## Specialized Interface
```csharp
public interface IWeaponComponent : IItemComponent
{
  int Damage {get;};
  void Attack(ItemObject item);
}

public interface IConsumableComponent : IItemComponent
{
  int Quantity {get;};
  void Consume(ItemObject item);
}
```

## WeaponComponent
```csharp

public class WeaponComponent : IWeaponComponent
{
    public ItemComponentType Type { get; } = ItemComponentType.Weapon;
    public int Damage { get; private set; }
    private readonly Action<ItemObject> executeAction;

    public WeaponComponent(int damage, Action<ItemObject> executeAction)
    {
        if (damage < 0)
            throw new ArgumentException("Damage cannot be negative");
        Damage = damage;
        this.executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
    }

    public void Execute(ItemObject item)
    {
        executeAction(item);
    }
}
```

## ConsumableComponent
```csharp
public class ConsumableComponent : IConsumableComponent
{
    public ItemComponentType Type { get; } = ItemComponentType.Consumable;
    public int Quantity { get; private set; }
    private readonly Action<ItemObject> executeAction;

    public ConsumableComponent(int quantity, Action<ItemObject> executeAction)
    {
        Quantity = quantity;
        this.executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
    }

    public void Execute(ItemObject item)
    {
        executeAction(item);
    }
}
```

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