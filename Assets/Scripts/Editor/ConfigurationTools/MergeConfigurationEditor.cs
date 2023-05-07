using System.Collections.Generic;
using System.Linq;
using Configurations;
using Editor.Common;
using UnityEditor;
using UnityEngine;

namespace Editor.ConfigurationTools
{
    [CustomEditor(typeof(MergeConfiguration))]
    public class MergeConfigurationEditor : SearchableListEditor<MergeConfiguration>
    {
        private const string EditorHeader = "Merge configuration";
        private const string ListPropertyPath = "_mergeRules";
        private MergeConfiguration _target;
        
        private void OnEnable()
        {
            _target = target as MergeConfiguration;
            Initialize(EditorHeader, ListPropertyPath);
        }

        protected override IEnumerable<int> Search(string searchPrompt)
        {
            var rulesList = _target.MergeRules;
            return rulesList.Select((_, index) => index);
        }

        protected override void AddEmptyElement() => _target.AddEmptyCard();
    }
}