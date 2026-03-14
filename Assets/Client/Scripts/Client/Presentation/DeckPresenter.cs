using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class DeckPresenter : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _deckVisualRoot;

        private int _count;

        public Vector3 GetSpawnPoint()
        {
            return _spawnPoint.position;
        }

        public Quaternion GetSpawnRotation()
        {
            return _spawnPoint.rotation;
        }

        public void SetCount(int count)
        {
            _count = count;

            // TODO: update deck UI/count visuals if needed.
        }
    }
}