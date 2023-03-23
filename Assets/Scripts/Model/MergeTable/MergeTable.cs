using System;
using System.Collections.Generic;

namespace Model.MergeTable
{
    public class MergeTable
    {
        public event Action OnStateChanged;
        
        private readonly List<Card> _cards = new();
        private const int MaxCards = 2;

        public IReadOnlyList<Card> Cards => _cards;
        public bool CanReceiveCards => _cards.Count < MaxCards;

        public void AddCard(Card card)
        {
            if (!CanReceiveCards)
                throw new InvalidOperationException("Impossible to put card over limit.");

            _cards.Add(card);
        }
    }
}