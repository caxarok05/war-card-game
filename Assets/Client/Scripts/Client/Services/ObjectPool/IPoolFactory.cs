using UnityEngine;

namespace Client.Scripts.Client
{
    public interface IPoolFactory<T> where T : MonoBehaviour
    {
        void CreateParent();
        T CreatePrefab();
    }
}