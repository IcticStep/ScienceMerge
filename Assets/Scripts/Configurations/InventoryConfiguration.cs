using System.Collections.Generic;
using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(fileName = nameof(InventoryConfiguration), menuName = "ScriptableObjects/"+nameof(InventoryConfiguration))]
    public class InventoryConfiguration : ScriptableObject
    {
        [SerializeField]
        private List<InventoryCardRule> _startCards;

        public IEnumerable<InventoryCardRule> StartCards => _startCards;
    }
}