using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class Pool<T> where T : MonoBehaviour
    {
        public T Prefab { get; }
        public bool AutoExpand { get; set; }
        public Transform Container { get; }

        public List<T> PooledObjects => _pool;

        private List<T> _pool;

        public Pool(T prefab, int count)
        {
            Prefab = prefab;
            Container = null;

            CreatePool(count);
        }

        public Pool(T prefab, int count, Transform container)
        {
            Prefab = prefab;
            Container = container;

            CreatePool(count);
        }

        public bool HasFreeElement(out T element)
        {
            foreach (var mono in _pool)
            {
                if (!mono.gameObject.activeInHierarchy)
                {
                    element = mono;
                    mono.gameObject.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }

        public T GetFreeElement()
        {
            if (HasFreeElement(out T element))
                return element;

            if (AutoExpand)
                return CreateObject(true);

            return null;
        }

        private void CreatePool(int count)
        {
            _pool = new List<T>();

            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            T createdObject = Object.Instantiate(Prefab, Container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            _pool.Add(createdObject);

            return createdObject;
        }
    }
}