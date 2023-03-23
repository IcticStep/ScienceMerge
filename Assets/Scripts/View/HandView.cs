using Model;
using UnityEngine;
using View.Cards;
using Zenject;

namespace View
{
    public class HandView : MonoBehaviour
    {
        [Inject]
        private void Construct(DiContainer container, Hand hand) => 
            (_container, _hand) = (container, hand);

        [SerializeField] private CardView _draggingCardPrefab;
        
        private DiContainer _container;
        private Hand _hand;
        private CardView _draggingCard;
        private Transform _cardTransform;

        private void Awake()
        {
            _draggingCard = _container
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

        public void DropCard()
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