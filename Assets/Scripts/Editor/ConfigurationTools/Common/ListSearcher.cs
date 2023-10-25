using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Common;
using UnityEditor;
using UnityEngine;
using static Editor.Common.EditorGUIStyles;

namespace Editor.ConfigurationTools.Common
{
    public class ListSearcher
    {
        public ListSearcher(Func<string, IEnumerable<int>> searchMethod) =>
            _searchMethod = searchMethod;

        private readonly Func<string, IEnumerable<int>> _searchMethod;
        
        private List<int> _searchResult;
        private string _searchPrompt = "";

        public IEnumerable<int> ListFilter
        {
            get
            {
                if (string.IsNullOrEmpty(_searchPrompt) || _searchResult is null)
                    return null;
                return _searchResult;
            }
        }

        public void DrawSearchBar()
        {
            const float averageLengthOfButton = 1.435f;
            const int buttonCount = 2;
            
            var searchFieldStyle =
                GUILayout.Width(EditorGUIUtility.currentViewWidth - buttonCount *(averageLengthOfButton * SmallButtonWidth));

            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                EditorGUI.BeginChangeCheck();
                _searchPrompt = GUILayout.TextField(_searchPrompt, EditorStyles.toolbarSearchField, searchFieldStyle)
                    .ToLowerInvariant();
                if(EditorGUI.EndChangeCheck())
                    _searchResult = Search().ToList();
                
                if(GUILayout.Button(ClearSearchButtonLabel, EditorStyles.toolbarButton, SmallButtonStyle))
                    ClearSearch();
                
                DrawSortButton();
            });
        }

        private void DrawSortButton()
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(UpdateOrderButtonLabel, EditorStyles.miniButton, MediumButtonStyle))
                Search();
        }

        private IEnumerable<int> Search() =>
            _searchResult = 
                _searchMethod.Invoke(_searchPrompt).ToList();
        
        private void ClearSearch()
        {
            _searchPrompt = "";
            _searchResult = default;
        }
    }
}