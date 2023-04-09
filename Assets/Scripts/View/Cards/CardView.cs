using Configurations;
using Model.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace View.Cards
{
    public class CardView : MonoBehaviour
    {
        [Inject]
        private void Construct(CardsConfiguration cardsConfiguration) => _cardsConfiguration = cardsConfiguration;

        [Header("Visualisation(choose any)")]
        [SerializeField] private Image _resourceImage;
        [SerializeField] private SpriteRenderer _resourceRenderer;

        [Header("Texts(choose all)")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private TMP_Text _cardsCountText;
        
        private const string InfinityLabel = "∞";

        private CardsConfiguration _cardsConfiguration;
        private Card _card;
        private EventTrigger _eventTrigger;

        public Card Card
        {
            get => _card;
            set
            {
                _card = value;
                UpdateVisualisation();
            }
        }

        public void Disable() => Card = null;

        public void SetCount(int count)
        {
            _cardsCountText.gameObject.SetActive(true);
            _cardsCountText.text = (count == int.MaxValue) ? InfinityLabel : count.ToString();
        }
        
        public void HideCount() => _cardsCountText.gameObject.SetActive(false);

        private void UpdateVisualisation()
        {
            if (_card is null)
            {
                MakeInvisible();
                return;
            }

            MakeVisible();
            
            var cardSettings = _cardsConfiguration[_card.Id];

            if (_resourceImage != null)
                _resourceImage.sprite = cardSettings.Sprite;
            if (_resourceRenderer != null)
                _resourceRenderer.sprite = cardSettings.Sprite;

            _titleText.text = _card.Name;
            _timeText.text = _card.MergeTime.ToString();
        }

        private void MakeVisible() => gameObject.SetActive(true);

        private void MakeInvisible() => gameObject.SetActive(false);
    }
}