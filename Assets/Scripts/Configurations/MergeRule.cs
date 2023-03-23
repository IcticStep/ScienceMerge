using System;
using System.Collections.Generic;
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
    }
}