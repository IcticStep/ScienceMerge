using System;

namespace Model.Inventory
{
    public class InventoryCell
    {
        public InventoryCell(Card card, int count = 1, bool forceInfinity = false) => 
            (Card, _count, _forceInfinity) = (card, count, forceInfinity);

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

            _count++;
            return true;
        }
        
        public bool TryTakeCard(out Card card)
        {
            if (!HasCards)
            {
                card = null;
                return false;
            }

            UpdateState();
            card = Card;
            return true;
        }

        public void UpdateState()
        {
            _count--;
            
            OnCountChanged?.Invoke();
            
            if(!HasCards)
                OnEmpty?.Invoke();
        }
    }
}