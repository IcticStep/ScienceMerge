using System.Collections.Generic;
using System.Linq;
using Configurations;
using Editor.Common;
using UnityEditor;
using UnityEngine;
using static Editor.Common.EditorGUIStyles;

namespace Editor.ConfigurationTools
{
    [CustomEditor(typeof(CardsConfiguration))]
    public class CardsConfigurationEditor : UnityEditor.Editor
    {
        private const string EditorHeader = "Cards configuration";
        private const string ListPropertyPath = "_cardSettings";

        private ListDrawer _listDrawer;
        private CardsConfiguration _target;
        private SerializedProperty _listProperty;
        private string _searchPrompt = "";
        private List<int> _searchResult;

        public override void OnInspectorGUI()
        {
            InitTargets();
            if(_target is null) return;

            serializedObject.Update();

            DrawHeaderPanel();
            DrawSearchBar();
            InitListDrawer();
            DrawList();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void OnValidate() => ForceSave();

        private void InitTargets()
        {
            _target = target as CardsConfiguration;
            _listProperty = serializedObject.FindProperty(ListPropertyPath);
        }

        private void InitListDrawer()
        {
            if(_listDrawer is not null)
                return;
            
            _listDrawer = new(_listProperty, addingItem: AddElement);
            _listDrawer.OnStateChange += AddElement;
        }

        private void DrawHeaderPanel()
        {
            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                GUILayout.Label(EditorHeader);
                GUILayout.FlexibleSpace();
                if(GUILayout.Button(UpdateOrderButtonLabel, EditorStyles.miniButton, MediumButtonStyle))
                    _target.ReorderByID();
            });
        }

        private void DrawSearchBar()
        {
            var searchFieldStyle =
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 1.635f * SmallButtonWidth);

            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                EditorGUI.BeginChangeCheck();
                _searchPrompt = GUILayout.TextField(_searchPrompt, EditorStyles.toolbarSearchField, searchFieldStyle)
                    .ToLowerInvariant();
                if(EditorGUI.EndChangeCheck())
                    Search();
                
                if(GUILayout.Button(ClearSearchButtonLabel, EditorStyles.toolbarButton, SmallButtonStyle))
                    ClearSearch();
            });
        }

        private void DrawList()
        {
            var filter = GetListFilter();
            _listDrawer.DrawScrollable(filter);
        }

        private void AddElement()
        {
            _target.AddEmptyCard();
            Repaint();
            ForceSave();
        }

        private void Search()
        {
            var settingsList = _target.CardSettingsList;
            _searchResult = settingsList
                .Where((x) => x.Name.ToLowerInvariant().Contains(_searchPrompt))
                .Select(x => x.Id)
                .ToList();
        }

        private void ClearSearch()
        {
            _searchPrompt = "";
            _searchResult = default;
        }

        private IEnumerable<int> GetListFilter()
        {
            if (string.IsNullOrEmpty(_searchPrompt) || _searchResult is null)
                return null;
            return _searchResult;
        }

        private void ForceSave()
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssetIfDirty(_target);
        }
    }
}