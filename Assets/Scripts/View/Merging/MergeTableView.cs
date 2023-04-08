using Model.Cards;
using Model.Merging;
using UnityEngine;
using View.Cards;

namespace View.Merging
{
    [RequireComponent(typeof(RectTransform))]
    public class MergeTableView : MonoBehaviour
    {
        [SerializeField] private CardView[] _cardViews;
        
        private MergeTable _mergeTable;
        public RectTransform RectTransform { get; private set; }

        public MergeTable MergeTable
        {
            get => _mergeTable;
            set
            {
                if (MergeTable is not null)
                    MergeTable.OnStateChanged -= UpdateView;

                _mergeTable = value;
                if(value is null)
                    return;

                MergeTable!.OnStateChanged += UpdateView;
                UpdateView();
            }
        }

        private void Awake() => RectTransform = GetComponent<RectTransform>();

        private void OnDisable()
        {
            if (MergeTable is not null)
                MergeTable.OnStateChanged -= UpdateView;
        }

        private void UpdateView()
        {
            var cards = MergeTable.Cards;

            for (var i = 0; i < _cardViews.Length; i++)
            {
                if (i >= cards.Count)
                {
                    _cardViews[i].Disable();
                    continue;
                }
                
                _cardViews[i].Card = cards[i];
            }
        }

        public void AddCard(Card card)
        {
            _mergeTable.AddCard(card);
        }
    }
}