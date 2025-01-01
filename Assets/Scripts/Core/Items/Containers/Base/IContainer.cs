public interface IContainer
{
    string ContainerID { get; }
    ContainerType Type { get; }
    IInventoryGrid Grid { get; }
} 