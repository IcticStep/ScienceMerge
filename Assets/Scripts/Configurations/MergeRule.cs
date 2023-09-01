using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Configurations
{
    [Serializable]
    public class MergeRule
    {
        [SerializeField] private List<int> _cards = new();
        [SerializeField] private int _resultID;
        
        public int ResultID => _resultID;

        public IReadOnlyList<int> CardsID => _cards.ToList();

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