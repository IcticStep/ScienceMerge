using System;
using System.Collections.Generic;
using System.Linq;
using Model.Cards;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configurations
{
    [Serializable]
    public class MergeRule
    {
        [SerializeField] private List<Card> _cards;
        
        [field: SerializeField]
        public int ResultID { get; private set; }

        public IReadOnlyList<int> CardsID => _cards.Select(card => card.Id).ToList();

        public override bool Equals(object obj)
        {
            if (obj is not MergeRule other)
                return false;

            if (CardsID.Count != other.CardsID.Count)
                return false;

            return CardsID.All(card => other.CardsID.Contains(card));
        }

        public override int GetHashCode() => HashCode.Combine(_cards, ResultID);
    }
}