using System;
using Model.Cards;

namespace Model.Storage
{
    public class InventoryCell
    {
        public InventoryCell(Card card, int count = 1, bool forceInfinity = false)
        {
            Card = card;
            _count = count;
            _forceInfinity = forceInfinity;
            
            if(_forceInfinity)
                _count = int.MaxValue;
        }

        private int _count;
        private readonly bool _forceInfinity;

        public event Action OnEmpty;
        public event Action OnCountChanged;
        public Card Card { get; private set; }
        public int Count => _forceInfinity ? int.MaxValue : _count;
        public bool HasCards => Count >= 0;

        public bool TryInsertCard(in Card card)
        {
            if (Card.Id != card.Id)
                return false;

            if(!_forceInfinity) _count++;
            UpdateState();
            return true;
        }
        
        public bool TryTakeCard(out Card card)
        {
            if (!HasCards)
            {
                card = null;
                return false;
            }
            
            if(!_forceInfinity) _count--;
            
            UpdateState();
            card = Card;
            return true;
        }

        public void RemoveCard()
        {
            if (!HasCards)
                throw new InvalidOperationException("There is no cards.");
            
            if(!_forceInfinity) _count--;
            UpdateState();
        }

        public void UpdateState()
        {
            if(_forceInfinity)
                return;
            
            OnCountChanged?.Invoke();
            
            if(!HasCards)
                OnEmpty?.Invoke();
        }
    }
}