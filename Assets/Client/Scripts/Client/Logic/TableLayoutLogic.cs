using Client.Scripts.Shared;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class TableLayoutLogic
    {
        public Vector3 GetCardPosition(
            PlayerId owner,
            int visualIndex,
            TablePresenter tablePresenter)
        {
            if (owner == PlayerId.Player)
            {
                return tablePresenter.GetPlayerCardPoint(visualIndex);
            }

            return tablePresenter.GetOpponentCardPoint(visualIndex);
        }

        public Quaternion GetCardRotation(
            PlayerId owner,
            int visualIndex,
            TablePresenter tablePresenter)
        {
            if (owner == PlayerId.Player)
            {
                return tablePresenter.GetPlayerCardRotation(visualIndex);
            }

            return tablePresenter.GetOpponentCardRotation(visualIndex);
        }

        public int GetSortingOrder(PlayerId owner, int sequenceIndex)
        {
            return owner == PlayerId.Player
                ? 100 + sequenceIndex
                : 200 + sequenceIndex;
        }
    }
}