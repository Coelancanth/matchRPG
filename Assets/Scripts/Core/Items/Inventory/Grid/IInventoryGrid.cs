using UnityEngine;
using System;

public interface IInventoryGrid
{
    int GridWidth { get; }
    int GridHeight { get; }
    event Action OnGridChanged;
    bool CanPlaceItem(ItemObject item, int startX, int startY);
    bool PlaceItem(ItemObject item, int startX, int startY);
    bool RemoveItem(ItemObject item);
    GridCell[,] GetGrid();
} 