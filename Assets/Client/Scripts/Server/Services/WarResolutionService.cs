using System.Collections.Generic;
using Client.Scripts.Shared;

namespace Client.Scripts.Shared
{
    public sealed class WarResolutionService
    {
        private readonly DrawPileRefillService _drawPileRefillService;
        private readonly GameOutcomeEvaluator _gameOutcomeEvaluator;

        public WarResolutionService(
            DrawPileRefillService drawPileRefillService,
            GameOutcomeEvaluator gameOutcomeEvaluator)
        {
            _drawPileRefillService = drawPileRefillService;
            _gameOutcomeEvaluator = gameOutcomeEvaluator;
        }

        public DrawResponse ResolveTurn(WarGameState state)
        {
            var revealedCards = new List<CardRevealData>();
            state.Table.Clear();

            if (!TryDrawFaceUp(state.Player, PlayerId.Player, false, state, revealedCards, out Card playerFaceUp))
            {
                return FinishGame(state, GameOutcomeType.OpponentWon, revealedCards, false);
            }

            if (!TryDrawFaceUp(state.Opponent, PlayerId.Opponent, false, state, revealedCards, out Card opponentFaceUp))
            {
                return FinishGame(state, GameOutcomeType.PlayerWon, revealedCards, false);
            }

            if (playerFaceUp.Rank > opponentFaceUp.Rank)
            {
                AwardTableCards(state.Player, state);
                return BuildResponse(state, revealedCards, RoundOutcomeType.PlayerWon, false);
            }

            if (opponentFaceUp.Rank > playerFaceUp.Rank)
            {
                AwardTableCards(state.Opponent, state);
                return BuildResponse(state, revealedCards, RoundOutcomeType.OpponentWon, false);
            }

            return ResolveWar(state, revealedCards);
        }

        private DrawResponse ResolveWar(WarGameState state, List<CardRevealData> revealedCards)
        {
            while (true)
            {
                if (!TryPlaceWarCards(state.Player, PlayerId.Player, state, revealedCards))
                {
                    return FinishGame(state, GameOutcomeType.OpponentWon, revealedCards, true);
                }

                if (!TryPlaceWarCards(state.Opponent, PlayerId.Opponent, state, revealedCards))
                {
                    return FinishGame(state, GameOutcomeType.PlayerWon, revealedCards, true);
                }

                Card playerWarFaceUp = GetLastFaceUpCard(revealedCards, PlayerId.Player);
                Card opponentWarFaceUp = GetLastFaceUpCard(revealedCards, PlayerId.Opponent);

                if (playerWarFaceUp.Rank > opponentWarFaceUp.Rank)
                {
                    AwardTableCards(state.Player, state);
                    return BuildResponse(state, revealedCards, RoundOutcomeType.PlayerWon, true);
                }

                if (opponentWarFaceUp.Rank > playerWarFaceUp.Rank)
                {
                    AwardTableCards(state.Opponent, state);
                    return BuildResponse(state, revealedCards, RoundOutcomeType.OpponentWon, true);
                }
            }
        }

        private bool TryPlaceWarCards(
            PlayerState playerState,
            PlayerId playerId,
            WarGameState state,
            List<CardRevealData> revealedCards)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!TryDrawFaceDown(playerState, playerId, state, revealedCards))
                {
                    return false;
                }
            }

            return TryDrawFaceUp(playerState, playerId, true, state, revealedCards, out _);
        }

        private bool TryDrawFaceUp(
            PlayerState playerState,
            PlayerId playerId,
            bool isWarCard,
            WarGameState state,
            List<CardRevealData> revealedCards,
            out Card drawnCard)
        {
            _drawPileRefillService.RefillIfNeeded(playerState);

            if (playerState.DrawPile.Count == 0)
            {
                drawnCard = default;
                return false;
            }

            drawnCard = playerState.DrawPile.Dequeue();
            state.Table.Add(drawnCard);
            revealedCards.Add(new CardRevealData(playerId, drawnCard, true, isWarCard));
            return true;
        }

        private bool TryDrawFaceDown(
            PlayerState playerState,
            PlayerId playerId,
            WarGameState state,
            List<CardRevealData> revealedCards)
        {
            _drawPileRefillService.RefillIfNeeded(playerState);

            if (playerState.DrawPile.Count == 0)
            {
                return false;
            }

            Card drawnCard = playerState.DrawPile.Dequeue();
            state.Table.Add(drawnCard);
            revealedCards.Add(new CardRevealData(playerId, drawnCard, false, true));
            return true;
        }

        private Card GetLastFaceUpCard(List<CardRevealData> revealedCards, PlayerId playerId)
        {
            for (int i = revealedCards.Count - 1; i >= 0; i--)
            {
                CardRevealData reveal = revealedCards[i];
                if (reveal.Owner == playerId && reveal.IsFaceUp)
                {
                    return reveal.Card;
                }
            }

            return default;
        }

        private void AwardTableCards(PlayerState winner, WarGameState state)
        {
            List<Card> wonCards = state.Table.TakeAll();
            winner.WonPile.AddRange(wonCards);
        }

        private DrawResponse BuildResponse(
            WarGameState state,
            List<CardRevealData> revealedCards,
            RoundOutcomeType roundOutcome,
            bool isWar)
        {
            GameOutcomeType gameOutcome = _gameOutcomeEvaluator.Evaluate(state);

            if (gameOutcome != GameOutcomeType.None)
            {
                return FinishGame(state, gameOutcome, revealedCards, isWar);
            }

            return new DrawResponse(
                revealedCards,
                roundOutcome,
                GameOutcomeType.None,
                isWar,
                state.Player.TotalCardCount,
                state.Opponent.TotalCardCount);
        }

        private DrawResponse FinishGame(
            WarGameState state,
            GameOutcomeType gameOutcome,
            List<CardRevealData> revealedCards,
            bool isWar)
        {
            state.Finish(gameOutcome);

            return new DrawResponse(
                revealedCards,
                RoundOutcomeType.None,
                gameOutcome,
                isWar,
                state.Player.TotalCardCount,
                state.Opponent.TotalCardCount);
        }
    }
}