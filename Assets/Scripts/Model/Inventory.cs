using System;
using System.Collections.Generic;
using Configurations;
using Zenject;

namespace Model
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
        
        public bool HasCard(Card card) => throw new NotImplementedException();

        public bool TryTakeCard(int id, out Card card) => throw new NotImplementedException();

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