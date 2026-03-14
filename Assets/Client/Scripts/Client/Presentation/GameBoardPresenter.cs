using System;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class GameBoardPresenter : MonoBehaviour
    {
        [field: SerializeField] public DeckPresenter PlayerDeckPresenter { get; private set; }
        [field: SerializeField] public DeckPresenter OpponentDeckPresenter { get; private set; }
        [field: SerializeField] public TablePresenter TablePresenter { get; private set; }
        [field: SerializeField] public TapInputPresenter TapInputPresenter { get; private set; }

        public event Action InputPerformed
        {
            add => TapInputPresenter.InputPerformed += value;
            remove => TapInputPresenter.InputPerformed -= value;
        }
        
        public bool IsDestroyed => this == null;

        public void ResetBoard()
        {
            TablePresenter.ResetTable();
        }

        public void UpdateDeckCounts(int playerCount, int opponentCount)
        {
            PlayerDeckPresenter.SetCount(playerCount);
            OpponentDeckPresenter.SetCount(opponentCount);

            // TODO: update UI HUD counters if needed.
        }

        public void SetInputEnabled(bool isEnabled)
        {
            TapInputPresenter.SetInputEnabled(isEnabled);
        }
    }
}