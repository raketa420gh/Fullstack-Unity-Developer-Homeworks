using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable NotResolvedInText

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
        public int Count => _itemsGrid.Count;

        private int _width;
        private int _height;

        private readonly Dictionary<Vector2Int, Item> _itemsGrid = new();

        public Inventory(in int width, in int height)
        {
            _width = width;
            _height = height;
        }

        public Inventory(
            in int width,
            in int height,
            params KeyValuePair<Item, Vector2Int>[] items) : this(width, height)
        {
            foreach (KeyValuePair<Item, Vector2Int> item in items)
                AddItem(item.Key, item.Value);
        }

        public Inventory(
            in int width,
            in int height,
            params Item[] items) : this(width, height)
        {
            foreach (Item item in items)
                AddItem(item, FindFreePosition(item.Size, out Vector2Int position) ? position : Vector2Int.zero);
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<KeyValuePair<Item, Vector2Int>> items) : this(width, height)
        {
            foreach (KeyValuePair<Item, Vector2Int> item in items)
                AddItem(item.Key, item.Value);
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<Item> items) : this(width, height)
        {
            foreach (Item item in items)
                AddItem(item, FindFreePosition(item.Size, out Vector2Int position) ? position : Vector2Int.zero);
        }

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(in Item item, in Vector2Int position)
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

        public bool CanAddItem(in Item item, in int posX, in int posY)
            => CanAddItem(item, new Vector2Int(posX, posY));

        /// <summary>
        /// Adds an item on a specified position if not exists
        /// </summary>
        public bool AddItem(in Item item, in Vector2Int position)
        {
            if (!CanAddItem(item, position))
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

        public bool AddItem(in Item item, in int posX, in int posY)
            => AddItem(item, new Vector2Int(posX, posY));

        /// <summary>
        /// Checks for adding an item on a free position
        /// </summary>
        public bool CanAddItem(in Item item)
            => FindFreePosition(item.Size, out _);

        /// <summary>
        /// Adds an item on a free position
        /// </summary>
        public bool AddItem(in Item item)
            => FindFreePosition(item.Size, out Vector2Int position) && AddItem(item, position);

        /// <summary>
        /// Returns a free position for a specified item
        /// </summary>
        public bool FindFreePosition(in Vector2Int size, out Vector2Int freePosition)
        {
            for (int x = 0; x <= _width - size.x; x++)
            {
                for (int y = 0; y <= _height - size.y; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    Item tempItem = new Item(size);
                    if (CanAddItem(tempItem, position))
                    {
                        freePosition = position;
                        return true;
                    }
                }
            }

            freePosition = Vector2Int.zero;
            return false;
        }

        /// <summary>
        /// Checks if a specified item exists
        /// </summary>
        public bool Contains(in Item item)
            => _itemsGrid.ContainsValue(item);

        /// <summary>
        /// Checks if a specified position is occupied
        /// </summary>
        public bool IsOccupied(in Vector2Int position)
            => _itemsGrid.ContainsKey(position);

        public bool IsOccupied(in int x, in int y)
            => IsOccupied(new Vector2Int(x, y));

        /// <summary>
        /// Checks if a position is free
        /// </summary>
        public bool IsFree(in Vector2Int position)
            => !IsOccupied(position);

        public bool IsFree(in int x, in int y)
            => IsFree(new Vector2Int(x, y));

        /// <summary>
        /// Removes a specified item if exists
        /// </summary>
        public bool RemoveItem(in Item item)
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

        public bool RemoveItem(in Item item, out Vector2Int position)
        {
            if (!Contains(item))
            {
                position = Vector2Int.zero;
                return false;
            }

            position = GetPositions(item).First();
            return RemoveItem(item);
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(in Vector2Int position)
            => _itemsGrid.TryGetValue(position, out Item item) ? item : null;

        public Item GetItem(in int x, in int y)
            => GetItem(new Vector2Int(x, y));

        public bool TryGetItem(in Vector2Int position, out Item item)
            => _itemsGrid.TryGetValue(position, out item);

        public bool TryGetItem(in int x, in int y, out Item item)
            => TryGetItem(new Vector2Int(x, y), out item);

        /// <summary>
        /// Returns matrix positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(Item item)
        {
            return _itemsGrid
                .Where(kvp => kvp.Value.Equals(item))
                .Select(kvp => kvp.Key)
                .ToArray();
        }

        public bool TryGetPositions(in Item item, out Vector2Int[] positions)
        {
            if (Contains(item))
            {
                positions = GetPositions(item);
                return true;
            }

            positions = null;
            return false;
        }

        /// <summary>
        /// Clears all inventory items
        /// </summary>
        public void Clear()
        {
            _itemsGrid.Clear();
            OnCleared?.Invoke();
        }

        /// <summary>
        /// Returns a count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            return _itemsGrid.Values
                .Where(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Distinct()
                .Count();
        }

        /// <summary>
        /// Moves a specified item to a target position if it exists
        /// </summary>
        public bool MoveItem(in Item item, in Vector2Int newPosition)
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

        /// <summary>
        /// Reorganizes inventory space to make the free area uniform
        /// </summary>
        public void ReorganizeSpace()
        {
            List<Item> items = _itemsGrid.Values.Distinct().ToList();
            Clear();

            foreach (Item item in items)
            {
                AddItem(item);
            }
        }

        /// <summary>
        /// Copies inventory items to a specified matrix
        /// </summary>
        public void CopyTo(in Item[,] matrix)
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