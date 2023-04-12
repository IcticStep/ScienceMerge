using System;
using Model.Cards;

namespace Model.Storage
{
    public interface IInventory
    {
        public event Action OnStateChanged;
        public bool HasCard(Card card);
        public bool TryTakeCard(int id, out Card card);
        public void InsertCard(Card card);
    }
}