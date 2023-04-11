using System;
using System.Collections.Generic;
using System.Linq;
using Model.Cards;
using UniRx;
using UnityEngine;

namespace Model.Merging.TablesStates
{
    public class MergingState : BaseState, IDisposable
    {
        public readonly TimeSpan TimerInterval = TimeSpan.FromSeconds(1);
        public TimeSpan TimerLeft { get; private set; }
        
        private IDisposable _timerObject = new CompositeDisposable();

        public MergingState(MergeTable context, List<Card> contextCards, Action<BaseState> stateSetter)
            : base(context, contextCards, stateSetter) { }

        public override void Start()
        {
            InitTimer();
            LaunchTimer();
        }

        private void LaunchTimer()
        {
            _timerObject = Observable
                .Timer(TimerInterval)
                .Repeat()
                .Subscribe(_ => UpdateTimer());
        }

        public override TimeSpan GetTimer() => TimerLeft;

        private void InitTimer()
        {
            foreach (var card in Cards)
                TimerLeft += card.MergeTime;
        }

        private void UpdateTimer()
        {
            Debug.Log($"Timer updated. {TimerLeft}");
            TimerLeft -= TimerInterval;
            ChangeStateIfDone();
            Refresh();
        }

        private void ChangeStateIfDone()
        {
            if (TimerLeft > TimeSpan.Zero)
                return;
            
            SetState(new RewardingState(Context, Cards, StateSetter));
            Dispose();
        }

        public void Dispose() => _timerObject.Dispose();
    }
}