using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static Editor.Common.EditorGUIStyles;
using Object = UnityEngine.Object;

namespace Editor.Common
{
    public abstract class SearchableListEditor<T> : UnityEditor.Editor
    where T : Object
    {
        private T _target;
        private SerializedProperty _listProperty;
        private ListDrawer _listDrawer;
        private List<int> _searchResult;
        private string _editorHeader;
        private string _listPropertyPath;
        private string _searchPrompt = "";
        [CanBeNull] private Action _sorting = null;
        
        public bool Initialized { get; private set; }

        public override void OnInspectorGUI()
        {
            if (!Initialized)
                return;
            
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

        protected abstract IEnumerable<int> Search(string searchPrompt);
        protected abstract void AddEmptyElement();

        protected void Initialize(string editorHeader, string listPropertyPath,
            [CanBeNull] Action itemsSorting = null)
        {
            _editorHeader = editorHeader;
            _listPropertyPath = listPropertyPath;
            _sorting = itemsSorting;
            
            Initialized = true;
        }
        
        private void InitTargets()
        {
            _target = (T)Convert.ChangeType(target, typeof(T));
            _listProperty = serializedObject.FindProperty(_listPropertyPath);
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
                DrawHeaderLabel();
                DrawSortButton();
            });
        }

        private void DrawSortButton()
        {
            if(_sorting is null)
                return;
            
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(UpdateOrderButtonLabel, EditorStyles.miniButton, MediumButtonStyle))
                _sorting.Invoke();
        }

        private void DrawHeaderLabel()
        {
            if(string.IsNullOrEmpty(_editorHeader))
                return;
            GUILayout.Label(_editorHeader);
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
                    _searchResult = Search(_searchPrompt).ToList();
                
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
            AddEmptyElement();
            Repaint();
            ForceSave();
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