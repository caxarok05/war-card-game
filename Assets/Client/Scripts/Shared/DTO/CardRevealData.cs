namespace Client.Scripts.Shared
{
    public sealed class CardRevealData
    {
        public PlayerId Owner { get; }
        public Card Card { get; }
        public bool IsFaceUp { get; }
        public bool IsWarCard { get; }

        public CardRevealData(PlayerId owner, Card card, bool isFaceUp, bool isWarCard)
        {
            Owner = owner;
            Card = card;
            IsFaceUp = isFaceUp;
            IsWarCard = isWarCard;
        }
    }
}