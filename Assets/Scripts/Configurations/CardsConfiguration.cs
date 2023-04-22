using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(fileName = nameof(CardsConfiguration), menuName = "ScriptableObjects/"+nameof(CardsConfiguration))]
    public class CardsConfiguration : ScriptableObject
    {
        [SerializeField]
        private List<CardSettings> _cardSetting;

        public IReadOnlyList<CardSettings> CardSettingsList => _cardSetting;

        public CardSettings this[int id] => 
            CardSettingsList.FirstOrDefault(card => card.Id == id);
    }
}
