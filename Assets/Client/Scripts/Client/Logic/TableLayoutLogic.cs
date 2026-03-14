using System.Collections.Generic;
using Client.Scripts.Shared;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class TableLayoutLogic
    {
        public Vector3 GetCardPosition(
            PlayerId owner,
            int index,
            TablePresenter tablePresenter)
        {
            if (owner == PlayerId.Player)
            {
                return tablePresenter.GetPlayerCardPoint(index);
            }

            return tablePresenter.GetOpponentCardPoint(index);
        }

        public Quaternion GetCardRotation(
            PlayerId owner,
            int index,
            TablePresenter tablePresenter)
        {
            if (owner == PlayerId.Player)
            {
                return tablePresenter.GetPlayerCardRotation(index);
            }

            return tablePresenter.GetOpponentCardRotation(index);
        }

        public int GetSortingOrder(PlayerId owner, int index)
        {
            return owner == PlayerId.Player
                ? 100 + index
                : 200 + index;
        }
    }
}