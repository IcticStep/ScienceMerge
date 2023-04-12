using System;
using Model.Cards;
using Model.Merging;
using Model.Storage;
using Zenject;

namespace Model
{
    public class Hand : IDisposable
    {
        [Inject]
        public Hand(Inventory inventory, MergeTablesContainer mergeTablesContainer)
        {
            _inventory = inventory;
            _mergeTablesContainer = mergeTablesContainer;
            
            _mergeTablesContainer.OnAnyCardRewarded += AddCardToInventory;
        }

        private readonly Inventory _inventory;
        private readonly MergeTablesContainer _mergeTablesContainer;

        public Card Card { get; set; }

        public bool HasCard => Card is not null;

        public void InsertCardIntoMergeTable(MergeTable mergeTable)
        {
            if (!HasCard)
                throw new InvalidOperationException("Tried to insert non-existing card!");

            _inventory.RemoveCard(Card);
            mergeTable.AddCard(Card);
            Card = default;
        }

        private void AddCardToInventory(Card card) => _inventory.InsertCard(card);

        public void Dispose() => _mergeTablesContainer.OnAnyCardRewarded -= AddCardToInventory;
    }
}