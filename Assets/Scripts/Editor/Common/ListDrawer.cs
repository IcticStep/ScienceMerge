using System;
using System.Collections.Generic;
using ModestTree;
using UnityEditor;
using UnityEngine;
using static Editor.Common.EditorGUIStyles;
// ReSharper disable PossibleMultipleEnumeration

namespace Editor.Common
{
    public class ListDrawer
    {
        private const int DefaultLinesVisible = 30;
        private readonly Action _addingItem;
        private readonly SerializedProperty _listProperty;
        private int _visibleLines;

        public event Action OnStateChange;
        
        private Vector2 _currentScroll;
        private IEnumerable<int> _idsToDraw;

        private bool Filtered => _idsToDraw is not null && !_idsToDraw.IsEmpty();
        private int ListSize => _listProperty.arraySize;

        private static bool AddButton => 
            GUILayout.Button(AddButtonLabel, EditorStyles.miniButtonLeft, SmallButtonStyle);
        private static bool DeleteLastButton =>
            GUILayout.Button(RemoveButtonLabel, EditorStyles.miniButtonRight, SmallButtonStyle);
        private static bool DeleteElementButton =>
            GUILayout.Button(RemoveButtonLabel, EditorStyles.miniButton, SmallButtonStyle);

        public int VisibleLines
        {
            get => _visibleLines;
            set
            {
                if(_visibleLines < 0)
                    throw new ArgumentException(nameof(_visibleLines));
                _visibleLines = value;
            }
        }

        public ListDrawer(SerializedProperty listProperty, int visibleLines = DefaultLinesVisible, Action addingItem = null)
        {
            _listProperty = listProperty;
            _addingItem = addingItem;
            VisibleLines = visibleLines;
        }

        public void Draw(IEnumerable<int> idsToDraw = null)
        {
            _idsToDraw = idsToDraw;

            DrawList();
            DrawManageButtons();
        }

        public void DrawScrollable(IEnumerable<int> idsToDraw = null)
        {
            _idsToDraw = idsToDraw;

            DrawScrollableList();
            DrawManageButtons();
        }

        private void DrawScrollableList() => 
            EditorGUILayoutComposer.DrawScrollable(DrawList, ref _currentScroll, VisibleLines);

        private void DrawList()
        {
            if (!Filtered)
                DrawAllElements();
            else
                DrawFilteredElements();
        }

        private void DrawAllElements()
        {
            for (var i = 0; i < _listProperty.arraySize; i++)
            {
                var element = _listProperty.GetArrayElementAtIndex(i);
                DrawListElement(element, i);
            }
        }

        private void DrawFilteredElements()
        {
            foreach (var index in _idsToDraw)
            {
                var element = _listProperty.GetArrayElementAtIndex(index);
                DrawListElement(element, index);
            }
        }

        private void DrawListElement(SerializedProperty element, int index)
        {
            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                EditorGUILayout.PropertyField(element);
                if (DeleteElementButton)
                    RemoveElement(index);
            });
        }

        private void DrawManageButtons()
        {
            if(Filtered)
                return;
            
            GUILayout.Space(DownButtonsTopMargin);
            const int buttonCount = 2;
            
            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                GUILayout.Space(EditorGUIUtility.currentViewWidth - (buttonCount + 1) * SmallButtonWidth);
                if(AddButton) 
                    _addingItem.Invoke();
                if(DeleteLastButton)
                    RemoveElement(ListSize-1);
            });
        }

        private void RemoveElement(int index)
        {
            var oldSize = _listProperty.arraySize;
            _listProperty.DeleteArrayElementAtIndex(index);
            if(_listProperty.arraySize == oldSize)
                _listProperty.DeleteArrayElementAtIndex(index);
            
            OnStateChange?.Invoke();
            ScrollListToBottom();
        }

        private void ScrollListToBottom() =>
            _currentScroll = new(
                _currentScroll.x,
                EditorGUILayoutComposer.EditorLineHeight * VisibleLines);
    }
}