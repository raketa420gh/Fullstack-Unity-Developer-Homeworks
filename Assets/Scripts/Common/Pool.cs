using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class Pool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        private T _prefab;
        
        [SerializeField]
        private bool _autoExpand;
        
        [SerializeField]
        private Transform _container;
        
        [SerializeField] 
        private int _poolCapacity;
        
        private List<T> _pool;

        public void CreatePool()
        {
            CreatePool(_poolCapacity);
        }

        public virtual T GetFromPool()
        {
            return GetFreeElement();
        }

        private bool HasFreeElement(out T element)
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

        private T GetFreeElement()
        {
            if (HasFreeElement(out T element))
                return element;

            if (_autoExpand)
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
            T createdObject = Instantiate(_prefab, _container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            _pool.Add(createdObject);

            return createdObject;
        }
    }
}