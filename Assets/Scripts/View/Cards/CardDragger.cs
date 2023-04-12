using Model.Cards;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace View.Cards
{
    [RequireComponent(typeof(CardView))]
    public class CardDragger : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [Inject]
        private void Construct(HandView handView) =>
            (_handView) = (handView);

        private Camera _mainCamera;
        private CardView _currentUICard;
        private Card _cardData;
        private HandView _handView;

        private void Awake()
        {
            _currentUICard = GetComponent<CardView>();
            _mainCamera = Camera.main;
        }

        private void Start() => _cardData = _currentUICard.Card;

        public void OnBeginDrag(PointerEventData eventData)
        {
            var position = GetDragWorldPosition(eventData);
            _handView.StartHoldingCard(_cardData, position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var position = GetDragWorldPosition(eventData);
            _handView.UpdateHoldingPosition(position);
        }

        public void OnEndDrag(PointerEventData eventData) => 
            _handView.DropCard(eventData.position);

        private Vector2 GetDragWorldPosition(PointerEventData eventData) =>
            _mainCamera.ScreenToWorldPoint(eventData.position);
    }
}