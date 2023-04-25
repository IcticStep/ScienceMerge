using System.Collections.Generic;
using System.Linq;
using Configurations;
using Editor.Common;
using UnityEditor;
using UnityEngine;

namespace Editor.ConfigurationTools
{
    [CustomEditor(typeof(CardsConfiguration))]
    public class CardsConfigurationEditor : UnityEditor.Editor
    {
        private const string EditorHeader = "Cards configuration";
        private const string ListPropertyPath = "_cardSettings";
        private const float SmallButtonWidth = 40f;
        private const float MediumButtonWidth = 60f;
        private const float DownButtonsTopMargin = -5f;
        private const int VisibleListLines = 31;

        private const string AddButtonLabel = "+";
        private const string RemoveButtonLabel = "-";
        private const string ClearSearchButtonLabel = "X";
        private const string UpdateOrderButtonLabel = "Reorder";
        private readonly GUILayoutOption _smallButtonStyle = GUILayout.Width(SmallButtonWidth);
        private readonly GUILayoutOption _mediumButtonStyle = GUILayout.Width(MediumButtonWidth);

        private CardsConfiguration _target;
        private SerializedProperty _listProperty;
        private Vector2 _currentScroll;
        private string _searchPrompt = "";
        private List<int> _searchResult;

        private int LastElementListIndex => _listProperty.arraySize - 1;

        public override void OnInspectorGUI()
        {
            Init();
            if(_target is null) return;

            serializedObject.Update();

            DrawHeaderPanel();
            DrawSearchBar();
            DrawList();
            DrawManageButtons();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void OnValidate() => ForceSave();

        private void Init()
        {
            _target = target as CardsConfiguration;
            _listProperty = serializedObject.FindProperty(ListPropertyPath);
        }

        private void DrawHeaderPanel()
        {
            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                GUILayout.Label(EditorHeader);
                GUILayout.FlexibleSpace();
                if(GUILayout.Button(UpdateOrderButtonLabel, EditorStyles.miniButton, _mediumButtonStyle))
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
                
                if(GUILayout.Button(ClearSearchButtonLabel, EditorStyles.toolbarButton, _smallButtonStyle))
                    ClearSearch();
            });
        }

        private void DrawList()
        {
            EditorGUILayoutComposer.DrawScrollable(() =>
                {
                    if (string.IsNullOrEmpty(_searchPrompt) || _searchResult is null)
                        DrawListElements(_listProperty);
                    else
                        DrawListElements(_listProperty, _searchResult);
                },
                ref _currentScroll,
                VisibleListLines);
        }
        
        private void DrawListElements(SerializedProperty list)
        {
            for (var i = 0; i < list.arraySize; i++)
            {
                var element = list.GetArrayElementAtIndex(i);
                DrawListElement(element, i);
            }
        }
        
        private void DrawListElements(SerializedProperty list, IEnumerable<int> elementIndexes)
        {
            foreach (var index in elementIndexes)
            {
                var element = list.GetArrayElementAtIndex(index);
                DrawListElement(element, index);
            }
        }

        private void DrawListElement(SerializedProperty element, int index)
        {
            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                EditorGUILayout.PropertyField(element);
                if(GUILayout.Button(RemoveButtonLabel, EditorStyles.miniButton, _smallButtonStyle)) 
                    RemoveElement(index);
            });
        }

        private void DrawManageButtons()
        {
            if(!string.IsNullOrEmpty(_searchPrompt))
                return;
            
            GUILayout.Space(DownButtonsTopMargin);
            const int buttonCount = 2;
            
            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                GUILayout.Space(EditorGUIUtility.currentViewWidth - (buttonCount + 1) * SmallButtonWidth);
                if(GUILayout.Button(AddButtonLabel, EditorStyles.miniButtonLeft, _smallButtonStyle)) 
                    AddElement();
                if(GUILayout.Button(RemoveButtonLabel, EditorStyles.miniButtonRight, _smallButtonStyle)) 
                    RemoveLastElement();
            });
        }

        private void AddElement()
        {
            _target.AddEmptyCard();
            Repaint();
            ForceSave();
            ScrollListToBottom();
        }

        private void RemoveLastElement() => RemoveElement(LastElementListIndex);

        private void RemoveElement(int index)
        {
            var oldSize = _listProperty.arraySize;
            _listProperty.DeleteArrayElementAtIndex(index);
            if(_listProperty.arraySize == oldSize)
                _listProperty.DeleteArrayElementAtIndex(index);
            
            ForceSave();
            ScrollListToBottom();
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

        private void ForceSave()
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssetIfDirty(_target);
        }

        private void ScrollListToBottom() =>
            _currentScroll = new(
                _currentScroll.x,
                EditorGUILayoutComposer.EditorLineHeight * VisibleListLines);
    }
}