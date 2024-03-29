﻿using System.Collections.Generic;
using System.Linq;
using Model.Cards;
using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(fileName = nameof(MergeConfiguration), menuName = "ScriptableObjects/"+nameof(MergeConfiguration))]
    public class MergeConfiguration : ScriptableObject
    {
        [SerializeField] 
        private List<MergeRule> _mergeRules;

        public IReadOnlyList<MergeRule> MergeRules => _mergeRules;

        public int GetResultCardID(IReadOnlyList<Card> cards)
        {
            var rule = _mergeRules
                .Where(rule => rule.CardsID.Count == cards.Count)
                .FirstOrDefault(rule => cards.All(card => rule.CardsID.Contains(card.Id)));

            return rule?.ResultID ?? -1;
        }
    }
}