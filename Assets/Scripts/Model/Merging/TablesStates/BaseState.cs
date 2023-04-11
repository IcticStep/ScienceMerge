using System;
using System.Collections.Generic;
using Model.Cards;
using UnityEngine;

namespace Model.Merging.TablesStates
{
    public abstract class BaseState
    {
        protected BaseState(MergeTable context, List<Card> contextCards, Action<BaseState> stateSetter)
        {
            Context = context;
            Cards = contextCards;
            StateSetter = stateSetter;
        }

        protected readonly MergeTable Context;
        protected readonly List<Card> Cards;
        protected readonly Action<BaseState> StateSetter;

        protected CardCreator CardCreator => Context.CardCreator;
        
        public virtual void Start() { }
        public virtual void AddCard(Card card) { }
        public virtual void HandleTouch() { }
        public virtual TimeSpan GetTimer() => TimeSpan.MinValue;

        protected void Refresh() => Context.Refresh();
        protected void SetState(BaseState state)
        {
            Debug.Log($"State changed from {Context.StateName} to {state}");
            StateSetter.Invoke(state);
        }
    }
}