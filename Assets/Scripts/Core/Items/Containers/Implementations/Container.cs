public class Container : IContainer
{
    public string ContainerID { get; }
    public ContainerType Type { get; }
    public IInventoryGrid Grid { get; }

    public Container(string containerID, ContainerType type, IInventoryGrid grid)
    {
        ContainerID = containerID;
        Type = type;
        Grid = grid;
    }
} 