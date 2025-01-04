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
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Width and height must be greater than zero.");

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

            if (item.Size.x > _width || item.Size.y > _height)
                throw new ArgumentException();

            if (item.Size.x <= 0 || item.Size.y <= 0)
                throw new ArgumentException();

            if (_itemsPositionMap.ContainsKey(item))
                return false;

            if (position.x < 0 || position.y < 0 || position.x + item.Size.x > _width ||
                position.y + item.Size.y > _height)
                return false;

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

            if (Contains(item))
                return false;

            if (!CanAddItem(item, position))
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
            => item != null && _itemsPositionMap.ContainsKey(item);

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
                throw new NullReferenceException();

            if (!Contains(item))
                throw new KeyNotFoundException();

            Vector2Int startPosition = _itemsPositionMap[item];
            List<Vector2Int> positions = new();

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
            return _itemsPositionMap.Keys.Count(item => item.Name == name);
        }

        public bool MoveItem(Item item, Vector2Int newPosition)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (!IsValidPosition(newPosition))
                return false;

            if (!_itemsPositionMap.TryGetValue(item, out Vector2Int oldPosition))
                return false;

            if (!CanPlaceItemAtPosition(item, newPosition))
                return false;

            RemoveItemFromGrid(item, oldPosition);
            PlaceItemAtPosition(item, newPosition);
            OnMoved?.Invoke(item, newPosition);
            return true;
        }

        public void ReorganizeSpace()
        {
            List<Item> itemsList = _itemsPositionMap.Keys.ToList();

            foreach (var item in itemsList)
            {
                RemoveItem(item);
            }

            itemsList.Sort((itemA, itemB) =>
            {
                var areaA = itemA.Size.x * itemA.Size.y;
                var areaB = itemB.Size.x * itemB.Size.y;
                return areaB.CompareTo(areaA);
            });

            foreach (var item in itemsList)
            {
                bool itemPlaced = false;
                for (var y = 0; y < _height && !itemPlaced; y++)
                {
                    for (var x = 0; x < _width && !itemPlaced; x++)
                    {
                        var position = new Vector2Int(x, y);
                        if (CanAddItem(item, position))
                        {
                            AddItem(item, position);
                            itemPlaced = true;
                        }
                    }
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

        public bool IsOccupied(int x, int y)
            => IsOccupied(new Vector2Int(x, y));

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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

        private void ClearGrid()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _itemsGrid[x, y] = null;
                }
            }
        }

        private void ClearItemFromGrid(Item item, Vector2Int position)
        {
            for (int x = 0; x < item.Size.x; x++)
            {
                for (int y = 0; y < item.Size.y; y++)
                {
                    _itemsGrid[position.x + x, position.y + y] = null;
                }
            }
        }

        private void PlaceItemInGrid(Item item, Vector2Int position)
        {
            for (int x = 0; x < item.Size.x; x++)
            {
                for (int y = 0; y < item.Size.y; y++)
                {
                    _itemsGrid[position.x + x, position.y + y] = item;
                }
            }
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
        
        private bool IsValidPosition(Vector2Int position) =>
            position.x >= 0 && position.x < _width &&
            position.y >= 0 && position.y < _height;

        private void RemoveItemFromGrid(Item item, Vector2Int position)
        {
            int itemWidth = item.Size.x;
            var itemHeight = item.Size.y;

            for (var x = position.x; x < position.x + itemWidth; x++)
            {
                for (var y = position.y; y < position.y + itemHeight; y++)
                {
                    _itemsGrid[x, y] = null;
                }
            }

            _itemsPositionMap.Remove(item);
        }

        private void PlaceItemAtPosition(Item item, Vector2Int position)
        {
            int itemWidth = item.Size.x;
            int itemHeight = item.Size.y;

            for (var x = position.x; x < position.x + itemWidth; x++)
            {
                for (var y = position.y; y < position.y + itemHeight; y++)
                {
                    _itemsGrid[x, y] = item;
                }
            }

            _itemsPositionMap[item] = position;
        }

        private bool CanPlaceItemAtPosition(Item item, Vector2Int position)
        {
            var itemWidth = item.Size.x;
            var itemHeight = item.Size.y;

            if (position.x + itemWidth > _width || position.y + itemHeight > _height)
                return false;

            for (var x = position.x; x < position.x + itemWidth; x++)
            {
                for (var y = position.y; y < position.y + itemHeight; y++)
                {
                    if (_itemsGrid[x, y] != null && !_itemsGrid[x, y].Equals(item))
                        return false;
                }
            }

            return true;
        }
    }
}