using System.Linq;
using Model;
using Model.MergeTable;
using UnityEngine;
using View.Cards;

namespace View.MergeTables
{
    public class MergeTableView : MonoBehaviour
    {
        [SerializeField] private CardView[] _cardViews;
        private MergeTable _mergeTable;

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

        public void AddCard(Card card)
        {
            _mergeTable.AddCard(card);
        }

        private void OnDisable()
        {
            if (MergeTable is not null)
                MergeTable.OnStateChanged -= UpdateView;
        }

        // TODO: analyze code
        private void UpdateView()
        {
            var cards = MergeTable.Cards;
            
            for (var i = 0; i < _cardViews.Length; i++)
            {
                if (i >= cards.Count)
                {
                    _cardViews[i].Card = null;
                    continue;
                }
                
                _cardViews[i].Card = cards?[i];
            }
        }
    }
}