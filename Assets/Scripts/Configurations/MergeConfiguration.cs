using System.Collections.Generic;
using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(fileName = nameof(MergeConfiguration), menuName = "ScriptableObjects/"+nameof(MergeConfiguration))]
    public class MergeConfiguration : ScriptableObject
    {
        [SerializeField] 
        private List<MergeRule> _mergeRules;

        public IEnumerable<MergeRule> MergeRules => _mergeRules;
    }
}