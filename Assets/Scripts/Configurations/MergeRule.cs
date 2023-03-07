using System;
using UnityEngine;

namespace Configurations
{
    [Serializable]
    public class MergeRule
    {
        [field: SerializeField]
        public int Card1ID { get; private set; }
        [field: SerializeField]
        public int Card2ID { get; private set; }
        [field: SerializeField]
        public int ResultID { get; private set; }
    }
}