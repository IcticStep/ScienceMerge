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
        
        public IEnumerable<InventoryCell> Cells => _cells;
        
        public bool HasCard(Card card) => 
            Cells.Any(cell => cell.Card == card);
        public bool HasCard(int cardId) => 
            Cells.Any(cell => cell.Card.Id == cardId);

        public bool TryTakeCard(int id, out Card card)
        {
            card = FindCellWithCard(id).Card;
            
            return card is not null;
        }

        private InventoryCell FindCellWithCard(int id) =>
            Cells.First(cell => cell.Card.Id == id);

        public void RemoveCard(Card card)
        {
            var cell = FindCellWithCard(card.Id);
        }

        public void InsertCard(Card card)
        {
            throw new NotImplementedException();
            OnStateChanged?.Invoke();
        }

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