using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Configurations
{
    [Serializable]
    public class MergeRule
    {
        [field: SerializeField] 
        private List<int> _cardsID;
        
        [field: SerializeField]
        public int ResultID { get; private set; }

        public IReadOnlyList<int> CardsID => _cardsID;

        public override bool Equals(object obj)
        {
            if (obj is not MergeRule other)
                return false;

            if (CardsID.Count != other.CardsID.Count)
                return false;

            return CardsID.All(card => other.CardsID.Contains(card));
        }

        public override int GetHashCode() => HashCode.Combine(_cardsID, ResultID);
    }
}