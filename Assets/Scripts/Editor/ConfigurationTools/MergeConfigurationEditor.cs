using Configurations;
using Editor.Common;
using UnityEditor;
using UnityEngine;

namespace Editor.ConfigurationTools
{
    [CustomEditor(typeof(MergeConfiguration))]
    public class MergeConfigurationEditor : UnityEditor.Editor
    {
        private const string EditorHeader = "Merge configuration";
        private const string ListPropertyPath = "_mergeRules";
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}