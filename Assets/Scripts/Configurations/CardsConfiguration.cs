using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configurations
{
    [CreateAssetMenu(fileName = nameof(CardsConfiguration), menuName = "ScriptableObjects/"+nameof(CardsConfiguration))]
    public class CardsConfiguration : ScriptableObject
    {
        [FormerlySerializedAs("_cardSetting")] [SerializeField]
        private List<CardSettings> _cardSettings;

        public IReadOnlyList<CardSettings> CardSettingsList => _cardSettings;

        public CardSettings this[int id] => 
            CardSettingsList.FirstOrDefault(card => card.Id == id);

        public void AddEmptyCard() => 
            _cardSettings.Add(new CardSettings(GetFreeId()));

        public void ReorderByID() =>
            _cardSettings = _cardSettings
                .OrderBy(card => card.Id)
                .ToList();

        private int GetFreeId()
        {
            var ids = _cardSettings
                .Select(card => card.Id)
                .OrderBy(id => id)
                .ToArray();

            for (var i = 0; i < ids.Length; i++)
                if (ids[i] != i) return i;

            return ids.Length;
        }
    }
}
