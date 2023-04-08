using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Configurations;
using Model.Cards;
using Model.Merging.TablesStates;
using Zenject;

namespace Model.Merging
{
    public class MergeTable
    {
        [Inject]
        public MergeTable(MergeConfiguration mergeConfiguration, CardCreator cardCreator)
        {
            _state = new CardsInputState(this, _cards, SetState);
            MergeConfiguration = mergeConfiguration;
            CardCreator = cardCreator;
        }

        public event Action OnDataChanged;
        public event Action<Card> OnCardRewarded;
        
        private readonly List<Card> _cards = new();
        private const int MaxCards = 2;
        private BaseState _state;

        public string StateName => _state.ToString();
        
        public MergeConfiguration MergeConfiguration { get; }
        public CardCreator CardCreator { get; }
        public IReadOnlyList<Card> Cards => _cards;
        public bool FullOfCards => _cards.Count == MaxCards;

        public void Refresh() => OnDataChanged?.Invoke();
        
        public void AddCard(Card card) => _state.AddCard(card);
        
        public void HandleTouch() => _state.HandleTouch();
        
        public TimeSpan GetTimer() => _state.GetTimer();

        public void RewardWithCard(Card card) => OnCardRewarded?.Invoke(card);

        private void SetState(BaseState newState) => _state = newState;
    }
}