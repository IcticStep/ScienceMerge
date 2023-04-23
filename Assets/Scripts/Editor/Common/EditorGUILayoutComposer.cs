using System;
using UnityEditor;
using UnityEngine;

namespace Editor.Common
{
    public static class EditorGUILayoutComposer
    {
        private const float EditorLineHeightMultiplayer = 1.35f;
        public static readonly float EditorLineHeight = EditorStyles.label.lineHeight * EditorLineHeightMultiplayer;
        
        public static void DrawScrollable(Action drawCall, ref Vector2 position, int showLines = 10)
        {
            var height = EditorLineHeight * showLines;
            position = EditorGUILayout.BeginScrollView(position, EditorStyles.helpBox, GUILayout.Height(height));
            drawCall.Invoke();
            EditorGUILayout.EndScrollView();
        }
        
        public static void DrawHorizontally(Action drawCall)
        {
            EditorGUILayout.BeginHorizontal();
            drawCall.Invoke();
            EditorGUILayout.EndHorizontal();
        }
    }
}