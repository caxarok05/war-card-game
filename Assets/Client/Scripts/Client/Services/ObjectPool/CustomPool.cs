using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client.Scripts.Client
{
    public class CustomPool<T> where T : MonoBehaviour
    {
        private readonly IPoolFactory<T> _poolFactory;
        private readonly List<T> _objects;

        public CustomPool(IPoolFactory<T> poolFactory, int prewarmObjects)
        {
            _poolFactory = poolFactory;
            _objects = new List<T>();

            _poolFactory.CreateParent();

            for (int i = 0; i < prewarmObjects; i++)
            {
                var obj = _poolFactory.CreatePrefab();
                obj.gameObject.SetActive(false);
                _objects.Add(obj);
            }
        }

        public T Get()
        {
            T obj;
            if (_objects.Count == 0)
            {
                Debug.LogError("Not find active object in pool.");
                return default;
            }

            if (_objects.FirstOrDefault(x => !x.isActiveAndEnabled) == null)
            {
                obj = _objects.FirstOrDefault(x => x.isActiveAndEnabled);
                Debug.LogError("Reset last object in pool");
            }
            else
                obj = _objects.FirstOrDefault(x => !x.isActiveAndEnabled) ?? Create();

            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        public void ReleaseAll()
        {
            foreach (var obj in _objects)
                obj.gameObject.SetActive(false);
        }
        
        private T Create()
        {
            var obj = _poolFactory.CreatePrefab();
            _objects.Add(obj);
            return obj;
        }
    }
}