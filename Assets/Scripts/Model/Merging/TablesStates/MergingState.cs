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
        private CompositeDisposable _disposables;
        public TimeSpan TimerLeft { get; private set; }

        public MergingState(MergeTable context, List<Card> contextCards, Action<BaseState> stateSetter)
            : base(context, contextCards, stateSetter)
        {
            Init();
            
            Observable.Timer(TimerInterval)
                .Repeat()
                .Subscribe(_ => UpdateTimer())
                .AddTo(_disposables);
        }

        public override TimeSpan GetTimer() => TimerLeft;

        private void Init()
        {
            _disposables = new CompositeDisposable();
            SetTotalTime();
        }

        private void SetTotalTime()
        {
            TimerLeft = new TimeSpan();
            
            foreach (var card in Cards)
                TimerLeft += card.MergeTime;
        }

        private void UpdateTimer()
        {
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

        public void Dispose() => _disposables?.Clear();
    }
}