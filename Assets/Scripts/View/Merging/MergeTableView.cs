using System;
using Model.Cards;
using Model.Merging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Cards;

namespace View.Merging
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Button))]
    public class MergeTableView : MonoBehaviour
    {
        [SerializeField] private CardView[] _cardViews;
        [SerializeField] private TextMeshProUGUI _timerText;

        public RectTransform RectTransform { get; private set; }
        private MergeTable _mergeTable;
        private Button _button;

        public MergeTable MergeTable
        {
            get => _mergeTable;
            set
            {
                if (MergeTable is not null)
                    MergeTable.OnDataChanged -= UpdateView;

                _mergeTable = value;
                if(value is null)
                    return;

                MergeTable!.OnDataChanged += UpdateView;
                UpdateView();
            }
        }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(HandleTouch);
        }

        private void OnDisable()
        {
            if (MergeTable is not null)
                MergeTable.OnDataChanged -= UpdateView;
            
            _button.onClick.RemoveListener(HandleTouch);
        }

        private void UpdateView()
        {
            UpdateCards();
            UpdateTimer();
        }

        private void UpdateCards()
        {
            var cards = MergeTable.Cards;

            for (var i = 0; i < _cardViews.Length; i++)
            {
                var noCards = i >= cards.Count;
                if (noCards)
                {
                    _cardViews[i].Disable();
                    continue;
                }

                _cardViews[i].Card = cards[i];
            }
        }

        private void UpdateTimer()
        {
            var time = _mergeTable.GetTimer();
            if (time == TimeSpan.MinValue)
            {
                HideTimer();
                return;                
            }
            
            _timerText.text = time.ToString();
            ShowTimer();
        }

        private void ShowTimer() => _timerText.gameObject.SetActive(true);

        private void HideTimer() => _timerText.gameObject.SetActive(false);

        private void HandleTouch() => _mergeTable.HandleTouch();
    }
}