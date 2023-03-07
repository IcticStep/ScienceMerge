using System;
using UnityEngine;

namespace Configurations
{
    [Serializable]
    public class InventoryCardRule
    {
        [field: SerializeField]
        public int CardId { get; private set; }
        [field: SerializeField]
        public int Count { get; private set; }
        [field: SerializeField]
        public bool ForceInfinity { get; private set; }
    }
}