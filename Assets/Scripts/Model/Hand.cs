using System;
using Model.Cards;
using Model.Merging;
using Model.Storage;
using Zenject;

namespace Model
{
    public class Hand
    {
        [Inject]
        public Hand(Inventory inventory) => 
            (_inventory) = (inventory);

        private Inventory _inventory;
        private Card _card;

        public Card Card
        {
            get => _card;
            set => _card = value;
        }

        public bool HasCard => Card is not null;

        public void InsertCardIntoMergeTable(MergeTable mergeTable)
        {
            if (!HasCard)
                throw new InvalidOperationException("Tried to insert non-existing card!");

            _inventory.RemoveCard(Card);
            mergeTable.AddCard(Card);
            Card = default;
        }
    }
}