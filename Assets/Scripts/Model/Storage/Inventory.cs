using System;
using System.Collections.Generic;
using System.Linq;
using Configurations;
using Model.Cards;
using Zenject;

namespace Model.Storage
{
    [Serializable]
    public class Inventory : IInventory
    {
        [Inject]
        public Inventory(InventoryConfiguration inventoryConfiguration, CardCreator cardCreator)
        {
            (_inventoryConfiguration, _cardCreator) = (inventoryConfiguration, cardCreator);
            SetStartCards();
        }

        public event Action OnStateChanged;
        
        private readonly List<InventoryCell> _cells = new();
        private readonly InventoryConfiguration _inventoryConfiguration;
        private readonly CardCreator _cardCreator;
        
        public IReadOnlyList<InventoryCell> Cells => _cells;

        public bool HasCard(int cardId) => 
            Cells.Any(cell => cell.Card.Id == cardId);

        public bool HasCard(Card card) => HasCard(card.Id);

        public bool TryTakeCard(int id, out Card card)
        {
            card = FindCellWithCard(id)?.Card;
            
            return card is not null;
        }

        public void RemoveCard(Card card)
        {
            var cell = FindCellWithCard(card.Id);
            cell.RemoveCard();
            OnStateChanged?.Invoke();
        }

        public void InsertCard(Card card)
        {
            var cell = FindCellWithCard(card.Id);
            if(cell is not null)
            {
                cell.TryInsertCard(card);
                OnStateChanged?.Invoke();
                return;
            }
            
            _cells.Add(new(card));
            OnStateChanged?.Invoke();
        }

        private InventoryCell FindCellWithCard(int id) =>
            Cells.FirstOrDefault(cell => cell.Card.Id == id);

        private void SetStartCards()
        {
            var cardsRules = _inventoryConfiguration.StartCards;
            foreach (var cardRule in cardsRules)
            {
                var card = _cardCreator.InstantiateCard(cardRule.CardId);
                _cells.Add(new InventoryCell(card, cardRule.Count, cardRule.ForceInfinity));
            }
            OnStateChanged?.Invoke();
        }
    }
}