using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class ContainerUI : MonoBehaviour
{
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject itemIconPrefab;

    private IContainer container;
    private GameObject[,] cellObjects;
    private Dictionary<ItemObject, Vector2Int> itemPositions = new Dictionary<ItemObject, Vector2Int>();

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
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cell = Instantiate(gridCellPrefab, gridParent);
                cell.transform.localPosition = new Vector3(x * 50, y * -50, 0);
                cellObjects[x, y] = cell;
            }
        }
    }

    public void RefreshUI()
    {
        // Clear old item icons
        foreach (Transform child in gridParent)
        {
            if (child.CompareTag("ItemIcon"))
            {
                Destroy(child.gameObject);
            }
        }

        itemPositions.Clear();
        var grid = container.Grid.GetGrid();

        // First pass: collect item positions
        for (int x = 0; x < container.Grid.GridWidth; x++)
        {
            for (int y = 0; y < container.Grid.GridHeight; y++)
            {
                if (grid[x, y].IsOccupied && grid[x, y].OccupyingItem != null)
                {
                    if (!itemPositions.ContainsKey(grid[x, y].OccupyingItem))
                    {
                        itemPositions[grid[x, y].OccupyingItem] = new Vector2Int(x, y);
                    }
                }
            }
        }

        // Second pass: create item icons
        foreach (var kvp in itemPositions)
        {
            var item = kvp.Key;
            var pos = kvp.Value;
            
            GameObject icon = Instantiate(itemIconPrefab, gridParent);
            icon.transform.localPosition = new Vector3(pos.x * 50, pos.y * -50, 0);
            
            // Set icon size based on item capacity
            var rectTransform = icon.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(
                    item.Model.Capacities.x * 50,
                    item.Model.Capacities.y * 50
                );
            }

            Image img = icon.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = item.Model.Icons[0];
            }
            icon.tag = "ItemIcon";
        }
    }

    private void OnDestroy()
    {
        if (container != null && container.Grid != null)
        {
            container.Grid.OnGridChanged -= RefreshUI;
        }
    }
} 