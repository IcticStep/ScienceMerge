using System.Collections.Generic;
using Configurations;
using Model.Cards;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Editor.ConfigurationTools.Inspectors
{
    [CustomPropertyDrawer(typeof(Card))]
    public class CardDrawer : PropertyDrawer
    {
        [Inject]
        private void Construct(CardsConfiguration cardsConfiguration) =>
            _cardsConfiguration = cardsConfiguration;

        private CardsConfiguration _cardsConfiguration;
        private IReadOnlyList<CardSettings> CardsSetting => _cardsConfiguration.CardSettingsList;
        private Card _target;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }
    }
}