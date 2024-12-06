using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventories
{
    public sealed class Inventory : IEnumerable<Item>
{
    public event Action<Item, Vector2Int> OnAdded;
    public event Action<Item, Vector2Int> OnRemoved;
    public event Action<Item, Vector2Int> OnMoved;
    public event Action OnCleared;

    public int Width => _width;
    public int Height => _height;
    public int Count => _itemsPositionMap.Count;

    private readonly int _width;
    private readonly int _height;

    private readonly Item[,] _itemsGrid;
    private readonly Dictionary<Item, Vector2Int> _itemsPositionMap = new();

    public Inventory(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero.");
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than zero.");

        _width = width;
        _height = height;
        _itemsGrid = new Item[width, height];
    }

    public Inventory(int width, int height, params KeyValuePair<Item, Vector2Int>[] items) : this(width, height)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items), "Items collection cannot be null.");

        foreach (var item in items)
            AddItem(item.Key, item.Value);
    }

    public Inventory(int width, int height, params Item[] items) : this(width, height)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items), "Items collection cannot be null.");

        foreach (var item in items)
            AddItem(item, FindFreePosition(item.Size, out Vector2Int position) ? position : Vector2Int.zero);
    }

    public Inventory(int width, int height, IEnumerable<KeyValuePair<Item, Vector2Int>> items) : this(width, height)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items), "Items collection cannot be null.");

        foreach (var item in items)
            AddItem(item.Key, item.Value);
    }

    public Inventory(int width, int height, IEnumerable<Item> items) : this(width, height)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items), "Items collection cannot be null.");

        foreach (var item in items)
            AddItem(item, FindFreePosition(item.Size, out Vector2Int position) ? position : Vector2Int.zero);
    }

    public bool CanAddItem(Item item, Vector2Int position)
    {
        if (item == null)
            return false;

        if (position.x < 0 || position.y < 0 || position.x + item.Size.x > _width ||
            position.y + item.Size.y > _height)
            return false;

        if (_itemsPositionMap.ContainsKey(item))
            return false;

        if (item.Size.x <= 0 || item.Size.y <= 0)
            throw new ArgumentException("Item size should be greater than zero.");

        for (int x = 0; x < item.Size.x; x++)
        {
            for (int y = 0; y < item.Size.y; y++)
            {
                if (_itemsGrid[position.x + x, position.y + y] != null)
                    return false;
            }
        }

        return true;
    }

    public bool AddItem(Item item, int posX, int posY)
        => AddItem(item, new Vector2Int(posX, posY));

    public bool CanAddItem(Item item, int posX, int posY)
        => CanAddItem(item, new Vector2Int(posX, posY));

    public bool AddItem(Item item, Vector2Int position)
    {
        if (item == null)
            return false;

        if (Contains(item) || !CanAddItem(item, position))
            return false;

        for (int x = 0; x < item.Size.x; x++)
        {
            for (int y = 0; y < item.Size.y; y++)
            {
                _itemsGrid[position.x + x, position.y + y] = item;
            }
        }

        _itemsPositionMap[item] = position;
        OnAdded?.Invoke(item, position);
        return true;
    }

    public bool CanAddItem(Item item)
    {
        if (item == null || _itemsPositionMap.ContainsKey(item))
            return false;

        return FindFreePosition(item.Size, out _);
    }

    public bool AddItem(Item item)
    {
        if (!CanAddItem(item))
            return false;

        return FindFreePosition(item.Size, out Vector2Int position) && AddItem(item, position);
    }

    public bool FindFreePosition(Vector2Int size, out Vector2Int freePosition)
    {
        if (size.x <= 0 || size.y <= 0)
            throw new ArgumentOutOfRangeException("Size should be greater than zero.");

        for (int y = 0; y <= _height - size.y; y++)
        {
            for (int x = 0; x <= _width - size.x; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (CanAddItem(new Item(size), position))
                {
                    freePosition = position;
                    return true;
                }
            }
        }

        freePosition = Vector2Int.zero;
        return false;
    }

    public bool Contains(Item item)
        => _itemsPositionMap.ContainsKey(item);

    public bool IsFree(int x, int y)
        => IsFree(new Vector2Int(x, y));

    public bool RemoveItem(Item item, out Vector2Int position)
    {
        if (!Contains(item))
        {
            position = Vector2Int.zero;
            return false;
        }

        position = _itemsPositionMap[item];
        return RemoveItem(item);
    }

    public Item GetItem(Vector2Int position)
    {
        if (position.x < 0 || position.x >= _width || position.y < 0 || position.y >= _height)
            throw new IndexOutOfRangeException("Position is out of range.");

        Item item = _itemsGrid[position.x, position.y];
        if (item == null)
            throw new NullReferenceException("Item not found at the specified position.");

        return item;
    }

    public Item GetItem(int x, int y)
        => GetItem(new Vector2Int(x, y));

    public bool TryGetItem(Vector2Int position, out Item item)
    {
        item = null;
        if (position.x < 0 || position.x >= _width || position.y < 0 || position.y >= _height)
            return false;

        item = _itemsGrid[position.x, position.y];
        return item != null;
    }

    public bool TryGetItem(int x, int y, out Item item)
        => TryGetItem(new Vector2Int(x, y), out item);

    public Vector2Int[] GetPositions(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");

        if (!Contains(item))
            throw new KeyNotFoundException("The specified item is not present in the inventory.");

        Vector2Int startPosition = _itemsPositionMap[item];
        List<Vector2Int> positions = new List<Vector2Int>();

        for (int x = 0; x < item.Size.x; x++)
        {
            for (int y = 0; y < item.Size.y; y++)
            {
                positions.Add(new Vector2Int(startPosition.x + x, startPosition.y + y));
            }
        }

        return positions.ToArray();
    }

    public bool TryGetPositions(Item item, out Vector2Int[] positions)
    {
        positions = null;

        if (item == null || !_itemsPositionMap.ContainsKey(item))
            return false;

        Vector2Int startPosition = _itemsPositionMap[item];
        positions = new Vector2Int[item.Size.x * item.Size.y];

        int index = 0;
        for (int x = 0; x < item.Size.x; x++)
        {
            for (int y = 0; y < item.Size.y; y++)
            {
                positions[index++] = new Vector2Int(startPosition.x + x, startPosition.y + y);
            }
        }

        return true;
    }

    public void Clear()
    {
        if (_itemsPositionMap.Count == 0)
            return;

        Array.Clear(_itemsGrid, 0, _itemsGrid.Length);
        _itemsPositionMap.Clear();
        OnCleared?.Invoke();
    }

    public int GetItemCount(string name)
    {
        switch (name)
        {
            case null:
            case "":
                return _itemsPositionMap.Keys.Count(item => item.Name == null);
            default:
                return _itemsPositionMap.Keys
                    .Count(item => item.Name != null && item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }

    public bool MoveItem(Item item, Vector2Int newPosition)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");

        if (!Contains(item))
            return false;

        Vector2Int[] oldPositions = GetPositions(item);

        if (!CanAddItem(item, newPosition))
            return false;

        foreach (Vector2Int pos in oldPositions)
        {
            _itemsGrid[pos.x, pos.y] = null;
        }

        for (int x = 0; x < item.Size.x; x++)
        {
            for (int y = 0; y < item.Size.y; y++)
            {
                _itemsGrid[newPosition.x + x, newPosition.y + y] = item;
            }
        }

        _itemsPositionMap[item] = newPosition;
        OnMoved?.Invoke(item, newPosition);
        return true;
    }

    public void ReorganizeSpace()
    {
        Clear();
        List<Item> items = _itemsPositionMap.Keys.ToList();

        items.Sort((a, b) => (b.Size.x * b.Size.y).CompareTo(a.Size.x * a.Size.y));

        foreach (Item item in items)
        {
            if (FindFreePosition(item.Size, out Vector2Int position))
            {
                AddItem(item, position);
            }
        }
    }

    public void CopyTo(Item[,] matrix)
    {
        if (matrix.GetLength(0) != _width || matrix.GetLength(1) != _height)
            throw new ArgumentException("Matrix size does not match inventory dimensions.");

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                matrix[x, y] = _itemsGrid[x, y];
            }
        }
    }

    public IEnumerator<Item> GetEnumerator()
    {
        return _itemsPositionMap.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool IsOccupied(int x, int y)
        => IsOccupied(new Vector2Int(x, y));

    private bool IsOccupied(Vector2Int position)
    {
        if (position.x < 0 || position.x >= _width || position.y < 0 || position.y >= _height)
            throw new IndexOutOfRangeException("Position is out of range.");

        return _itemsGrid[position.x, position.y] != null;
    }

    private bool IsFree(Vector2Int position)
    {
        if (position.x < 0 || position.x >= _width || position.y < 0 || position.y >= _height)
            throw new IndexOutOfRangeException("Position is out of range.");

        return _itemsGrid[position.x, position.y] == null;
    }

    private bool RemoveItem(Item item)
    {
        if (!Contains(item))
            return false;

        Vector2Int[] positions = GetPositions(item);
        foreach (Vector2Int pos in positions)
        {
            _itemsGrid[pos.x, pos.y] = null;
        }

        _itemsPositionMap.Remove(item);
        OnRemoved?.Invoke(item, positions.First());
        return true;
    }
}
}