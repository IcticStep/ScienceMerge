using System;
using System.Collections.Generic;
using System.Linq;
using Model.Cards;
using UniRx;

namespace Model.Merging.TablesStates
{
    public class RewardingState : BaseState
    {
        public RewardingState(MergeTable context, List<Card> contextCards, Action<BaseState> stateSetter)
            : base(context, contextCards, stateSetter) { }

        public override void Start()
        {
            SetResult();
            Refresh();
        }

        public override void HandleTouch()
        {
            Context.RewardWithCard(Cards.First());
            Cards.Clear();
            SetState(new CardsInputState(Context, Cards, StateSetter));
            Refresh();
        }

        private void SetResult()
        {
            var result = CardCreator.InstantiateCardByMerge(Cards);
            Cards.Clear();

            if (result is null)
            {
                SetState(new CardsInputState(Context, Cards, StateSetter));
                Refresh();
                return;
            }

            Cards.Add(result);
            Refresh();
        }
    }
}