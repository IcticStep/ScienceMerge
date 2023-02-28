using Configurations;
using Model;
using TMPro;
using UnityEngine;
using Zenject;

namespace View
{
    public class CardView : MonoBehaviour
    {
        [Inject]
        private void Construct(CardsConfiguration cardsConfiguration) => _cardsConfiguration = cardsConfiguration;
        
        [SerializeField] private SpriteRenderer _resourceImage;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _timeText;
        
        private CardsConfiguration _cardsConfiguration;
        private Card _card;
        
        public Card Card
        {
            get => _card;
            set
            {
                _card = value;
                UpdateVisualisation();
            }
        }

        private void UpdateVisualisation()
        {
            var cardSettings = _cardsConfiguration[_card.Id];

            _resourceImage.sprite = cardSettings.Sprite;
            _titleText.text = _card.Name;
            _timeText.text = _card.MergeTime.ToString();
        } 
    }
}