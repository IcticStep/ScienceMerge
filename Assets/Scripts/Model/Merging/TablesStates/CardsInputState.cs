using System;
using System.Collections.Generic;
using Model.Cards;

namespace Model.Merging.TablesStates
{
    public class CardsInputState : BaseState
    {
        public CardsInputState(MergeTable context, List<Card> contextCards, Action<BaseState> stateSetter) 
            : base(context, contextCards, stateSetter) { }

        public override void AddCard(Card card)
        {
            if (Context.FullOfCards)
                throw new InvalidOperationException("Cards overfloating!");
            
            Cards.Add(card);
            ChangeStateIfDone();
            Refresh();
        }

        private void ChangeStateIfDone()
        {
            if (Context.FullOfCards)
                SetState(new MergingState(Context, Cards, StateSetter));
        }
    }
}