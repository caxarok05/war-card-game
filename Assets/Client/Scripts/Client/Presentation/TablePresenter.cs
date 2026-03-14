using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class TablePresenter : MonoBehaviour
    {
        [SerializeField] private List<Transform> _playerCardPoints;
        [SerializeField] private List<Transform> _opponentCardPoints;

        public Vector3 GetPlayerCardPoint(int index)
        {
            return _playerCardPoints[Mathf.Clamp(index, 0, _playerCardPoints.Count - 1)].position;
        }

        public Quaternion GetPlayerCardRotation(int index)
        {
            return _playerCardPoints[Mathf.Clamp(index, 0, _playerCardPoints.Count - 1)].rotation;
        }

        public Vector3 GetOpponentCardPoint(int index)
        {
            return _opponentCardPoints[Mathf.Clamp(index, 0, _opponentCardPoints.Count - 1)].position;
        }

        public Quaternion GetOpponentCardRotation(int index)
        {
            return _opponentCardPoints[Mathf.Clamp(index, 0, _opponentCardPoints.Count - 1)].rotation;
        }

        public void ResetTable()
        {
            // table cleanup is driven by logic through pool release
        }
    }
}