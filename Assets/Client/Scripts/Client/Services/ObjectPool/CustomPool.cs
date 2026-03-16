using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Client
{
    public class CustomPool<T> where T : MonoBehaviour
    {
        private readonly IPoolFactory<T> _poolFactory;
        private readonly List<T> _objects;

        public int Count => _objects.Count;

        public CustomPool(IPoolFactory<T> poolFactory, int prewarmObjects)
        {
            _poolFactory = poolFactory;
            _objects = new List<T>(prewarmObjects);

            _poolFactory.CreateParent();
            Prewarm(prewarmObjects);
        }

        public T Get()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                T obj = _objects[i];
                if (!obj.gameObject.activeSelf)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            T created = Create();
            created.gameObject.SetActive(true);
            return created;
        }

        public void Release(T obj)
        {
            if (obj == null)
            {
                return;
            }

            obj.gameObject.SetActive(false);
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] != null)
                {
                    _objects[i].gameObject.SetActive(false);
                }
            }
        }

        private void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T created = Create();
                created.gameObject.SetActive(false);
            }
        }

        private T Create()
        {
            T obj = _poolFactory.CreatePrefab();
            _objects.Add(obj);
            return obj;
        }
    }
}