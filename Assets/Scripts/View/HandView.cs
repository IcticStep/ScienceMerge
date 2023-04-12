using System.Linq;
using Model;
using Model.Cards;
using Model.Merging;
using UnityEngine;
using View.Cards;
using View.Merging;
using Zenject;

namespace View
{
    public class HandView : MonoBehaviour
    {
        [Inject]
        private void Construct(DiContainer diContainer, Hand hand, MergeTableContainerView mergeTableContainerView) => 
            (_diContainer, _hand, _mergeTableContainerView) = (diContainer, hand, mergeTableContainerView);

        [SerializeField] private CardView _draggingCardPrefab;
        [SerializeField] private float _tablePutRange = 300;
        
        private DiContainer _diContainer;
        private Hand _hand;
        private MergeTableContainerView _mergeTableContainerView;
        private CardView _draggingCard;
        private Transform _cardTransform;

        private void Awake()
        {
            _draggingCard = _diContainer
                .InstantiatePrefabForComponent<CardView>(_draggingCardPrefab);
            _cardTransform = _draggingCard.transform;
            _draggingCard.gameObject.SetActive(false);
        }

        public void StartHoldingCard(Card card, Vector2 position)
        {
            _hand.Card = card;
            SetDraggingCardView(card);
            UpdateHoldingPosition(position);
        }

        public void UpdateHoldingPosition(Vector2 position) => 
            _cardTransform.position = position;

        public void DropCard(Vector2 screenPosition)
        {
            var mergeTableInRange = GetMergeTableInCardRange(screenPosition);
            
            if (mergeTableInRange is not null)
                _hand.InsertCardIntoMergeTable(mergeTableInRange);
            RemoveCardFromHand();
        }

        private MergeTable GetMergeTableInCardRange(Vector2 screenPosition)
        {
            var distances = _mergeTableContainerView
                .GetDistancesToTables(screenPosition);
            var closest = distances.First();
            
            return (closest.Distance <= _tablePutRange) 
                ? closest.TableView.MergeTable 
                : null;
        }

        private void RemoveCardFromHand()
        {
            _hand.Card = default;
            _draggingCard.gameObject.SetActive(false);
        }

        private void SetDraggingCardView(Card card)
        {
            _draggingCard.gameObject.SetActive(true);
            _draggingCard.Card = card;
        }
    }
}