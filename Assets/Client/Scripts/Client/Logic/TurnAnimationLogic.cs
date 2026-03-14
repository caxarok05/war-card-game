using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;

namespace Client.Scripts.Client
{
    public sealed class TurnAnimationLogic
    {
        private readonly CustomPool<CardPresenter> _cardPool;
        private readonly GameBoardPresenter _gameBoardPresenter;
        private readonly TableLayoutLogic _tableLayoutLogic;

        public TurnAnimationLogic(
            CustomPool<CardPresenter> cardPool,
            GameBoardPresenter gameBoardPresenter,
            TableLayoutLogic tableLayoutLogic)
        {
            _cardPool = cardPool;
            _gameBoardPresenter = gameBoardPresenter;
            _tableLayoutLogic = tableLayoutLogic;
        }

        public async UniTask PlayAsync(
            DrawResponse response,
            CancellationToken cancellationToken)
        {
            _cardPool.ReleaseAll();

            int playerIndex = 0;
            int opponentIndex = 0;

            for (int i = 0; i < response.RevealedCards.Count; i++)
            {
                CardRevealData reveal = response.RevealedCards[i];

                int tableIndex;
                DeckPresenter deckPresenter;

                if (reveal.Owner == PlayerId.Player)
                {
                    tableIndex = playerIndex;
                    playerIndex++;
                    deckPresenter = _gameBoardPresenter.PlayerDeckPresenter;
                }
                else
                {
                    tableIndex = opponentIndex;
                    opponentIndex++;
                    deckPresenter = _gameBoardPresenter.OpponentDeckPresenter;
                }

                CardPresenter cardPresenter = _cardPool.Get();

                cardPresenter.gameObject.SetActive(true);
                cardPresenter.SetPositionAndRotation(
                    deckPresenter.GetSpawnPoint(),
                    deckPresenter.GetSpawnRotation());

                cardPresenter.SetSortingOrder(
                    _tableLayoutLogic.GetSortingOrder(reveal.Owner, tableIndex));

                if (reveal.IsFaceUp)
                {
                    cardPresenter.ShowFront(reveal.Card);
                }
                else
                {
                    cardPresenter.ShowBack();
                }

                await cardPresenter.MoveToAsync(
                    _tableLayoutLogic.GetCardPosition(
                        reveal.Owner,
                        tableIndex,
                        _gameBoardPresenter.TablePresenter),
                    _tableLayoutLogic.GetCardRotation(
                        reveal.Owner,
                        tableIndex,
                        _gameBoardPresenter.TablePresenter),
                    cancellationToken);
            }
        }
    }
}