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
        public int Count => _itemsGrid.Values.Distinct().Count();

        private readonly int _width;
        private readonly int _height;

        private readonly Dictionary<Vector2Int, Item> _itemsGrid = new();

        public Inventory(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public Inventory(int width, int height, params KeyValuePair<Item, Vector2Int>[] items) : this(width, height)
        {
            foreach (var item in items)
                AddItem(item.Key, item.Value);
        }

        public Inventory(int width, int height, params Item[] items) : this(width, height)
        {
            foreach (var item in items)
                AddItem(item, FindFreePosition(item.Size, out Vector2Int position) ? position : Vector2Int.zero);
        }

        public Inventory(int width, int height, IEnumerable<KeyValuePair<Item, Vector2Int>> items) : this(width, height)
        {
            foreach (var item in items)
                AddItem(item.Key, item.Value);
        }

        public Inventory(int width, int height, IEnumerable<Item> items) : this(width, height)
        {
            foreach (var item in items)
                AddItem(item, FindFreePosition(item.Size, out Vector2Int position) ? position : Vector2Int.zero);
        }

        public bool CanAddItem(Item item, Vector2Int position)
        {
            if (position.x < 0 || position.y < 0 || position.x + item.Size.x > _width ||
                position.y + item.Size.y > _height)
                return false;

            for (int x = 0; x < item.Size.x; x++)
            {
                for (int y = 0; y < item.Size.y; y++)
                {
                    if (_itemsGrid.ContainsKey(new Vector2Int(position.x + x, position.y + y)))
                        return false;
                }
            }

            return true;
        }

        public bool CanAddItem(Item item, int posX, int posY)
            => CanAddItem(item, new Vector2Int(posX, posY));

        public bool AddItem(Item item, Vector2Int position)
        {
            if (Contains(item) || !CanAddItem(item, position))
                return false;

            for (int x = 0; x < item.Size.x; x++)
            {
                for (int y = 0; y < item.Size.y; y++)
                {
                    _itemsGrid[new Vector2Int(position.x + x, position.y + y)] = item;
                }
            }

            OnAdded?.Invoke(item, position);
            return true;
        }

        public bool AddItem(Item item, int posX, int posY)
            => AddItem(item, new Vector2Int(posX, posY));

        public bool CanAddItem(Item item)
            => FindFreePosition(item.Size, out _);

        public bool AddItem(Item item)
        {
            if (item == null)
                return false;

            return FindFreePosition(item.Size, out Vector2Int position) && AddItem(item, position);
        }

        public bool FindFreePosition(Vector2Int size, out Vector2Int freePosition)
        {
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
            => _itemsGrid.ContainsValue(item);

        public bool IsOccupied(Vector2Int position)
            => _itemsGrid.ContainsKey(position);

        public bool IsOccupied(int x, int y)
            => IsOccupied(new Vector2Int(x, y));
             public bool IsFree(Vector2Int position)
            => !IsOccupied(position);

        public bool IsFree(int x, int y)
            => IsFree(new Vector2Int(x, y));

        public bool RemoveItem(Item item)
        {
            if (!Contains(item))
                return false;

            Vector2Int[] positions = GetPositions(item);
            foreach (Vector2Int pos in positions)
            {
                _itemsGrid.Remove(pos);
            }

            OnRemoved?.Invoke(item, positions.First());
            return true;
        }

        public bool RemoveItem(Item item, out Vector2Int position)
        {
            if (!Contains(item))
            {
                position = Vector2Int.zero;
                return false;
            }

            position = GetPositions(item).First();
            return RemoveItem(item);
        }

        public Item GetItem(Vector2Int position)
            => _itemsGrid.TryGetValue(position, out Item item) ? item : null;

        public Item GetItem(int x, int y)
            => GetItem(new Vector2Int(x, y));

        public bool TryGetItem(Vector2Int position, out Item item)
            => _itemsGrid.TryGetValue(position, out item);

        public bool TryGetItem(int x, int y, out Item item)
            => TryGetItem(new Vector2Int(x, y), out item);

        public Vector2Int[] GetPositions(Item item)
        {
            return _itemsGrid
                .Where(kvp => kvp.Value.Equals(item))
                .Select(kvp => kvp.Key)
                .ToArray();
        }

        public bool TryGetPositions(Item item, out Vector2Int[] positions)
        {
            if (Contains(item))
            {
                positions = GetPositions(item);
                return true;
            }

            positions = null;
            return false;
        }

        public void Clear()
        {
            _itemsGrid.Clear();
            OnCleared?.Invoke();
        }

        public int GetItemCount(string name)
        {
            return _itemsGrid.Values
                .Where(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Distinct()
                .Count();
        }

        public bool MoveItem(Item item, Vector2Int newPosition)
        {
            if (!Contains(item) || !CanAddItem(item, newPosition))
                return false;

            Vector2Int[] oldPositions = GetPositions(item);
            foreach (Vector2Int pos in oldPositions)
            {
                _itemsGrid.Remove(pos);
            }

            AddItem(item, newPosition);
            OnMoved?.Invoke(item, newPosition);
            return true;
        }

        public void ReorganizeSpace()
        {
            List<Item> items = _itemsGrid.Values.Distinct().ToList();
            Clear();

            foreach (Item item in items)
            {
                AddItem(item);
            }
        }

        public void CopyTo(Item[,] matrix)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    matrix[x, y] = GetItem(new Vector2Int(x, y));
                }
            }
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _itemsGrid.Values.Distinct().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}