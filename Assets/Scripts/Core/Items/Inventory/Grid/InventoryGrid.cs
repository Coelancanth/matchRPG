using UnityEngine;
using System;

    public class InventoryGrid : IInventoryGrid
    {
        public int GridWidth { get; }
        public int GridHeight { get; }
    private GridCell[,] gridCells;
    
    public event Action OnGridChanged;

    public InventoryGrid(int gridWidth, int gridHeight)
    {
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        gridCells = new GridCell[gridWidth, gridHeight];
        
        // Initialize grid cells
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
        if (startX < 0 || startX + item.Model.Capacities.x > GridWidth || 
            startY < 0 || startY + item.Model.Capacities.y > GridHeight)
        {
            return false;
        }

        for (int x = startX; x < startX + item.Model.Capacities.x; x++)
        {
            for (int y = startY; y < startY + item.Model.Capacities.y; y++)
            {
                if (gridCells[x, y].IsOccupied)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool PlaceItem(ItemObject item, int startX, int startY)
    {
        if (!CanPlaceItem(item, startX, startY))
        {
            return false;
        }

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

    public bool RemoveItem(ItemObject item)
    {
        bool itemFound = false;
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                if (gridCells[x, y].OccupyingItem == item)
                {
                    gridCells[x, y].Vacate();
                    itemFound = true;
                }
            }
        }

        if (itemFound)
        {
            OnGridChanged?.Invoke();
        }
        return itemFound;
    }

    public GridCell[,] GetGrid()
    {
        return gridCells;
        }
    } 