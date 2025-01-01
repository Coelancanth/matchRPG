# Data Structure

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
    private List<string> capacities = new List<string>();
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


