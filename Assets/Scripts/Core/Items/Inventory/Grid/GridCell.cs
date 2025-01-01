using UnityEngine;

public class GridCell
{
    public bool IsOccupied { get; private set; }
    public ItemObject OccupyingItem { get; private set; }

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