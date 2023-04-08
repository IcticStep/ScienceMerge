using System;
using System.Collections.Generic;
using Model.Cards;
using Zenject;

namespace Model.Merging
{
    public class MergeTablesContainer
    {
        [Inject]
        public MergeTablesContainer(DiContainer diContainer)
        {
            _diContainer = diContainer;
            CreateStartTables();
        }

        public event Action OnStateChanged;
        public event Action<Card> OnAnyCardRewarded;

        private const int StartTableAmount = 2;
        private readonly DiContainer _diContainer;
        private readonly List<MergeTable> _mergeTables = new();

        public IReadOnlyList<MergeTable> MergeTables => _mergeTables;

        private void CreateStartTables()
        {
            for (var i = 0; i < StartTableAmount; i++)
                AddNewTable();
        }

        public void AddNewTable()
        {
            var table = _diContainer.Instantiate<MergeTable>();
            table.OnCardRewarded += RewardWithCard;
            _mergeTables.Add(table);
            OnStateChanged?.Invoke();
        }

        private void RewardWithCard(Card card) => OnAnyCardRewarded?.Invoke(card);
    }
}