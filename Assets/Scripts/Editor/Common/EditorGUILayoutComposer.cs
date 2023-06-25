using System;
using UnityEditor;
using UnityEngine;

namespace Editor.Common
{
    public static class EditorGUILayoutComposer
    {
        private const int DefaultLinesVisible = 30;
        private const float EditorLineHeightMultiplayer = 1.35f;

        public static readonly float EditorLineHeight = EditorStyles.label.lineHeight * EditorLineHeightMultiplayer;

        public static void DrawScrollable(Action drawCall, ref Vector2 position, int showLines = DefaultLinesVisible)
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

        public static void DrawToggling(Action drawCall, bool enabled)
        {
            EditorGUI.BeginDisabledGroup(!enabled);
            drawCall.Invoke();
            EditorGUI.EndDisabledGroup();
        }

        public static bool DrawMiniButton(string label) =>
            DrawMiniButton(label, EditorStyles.miniButton);
        
        public static bool DrawMiniButton(string label, GUIStyle style) =>
            GUILayout.Button(label, style, EditorGUIStyles.SmallButtonStyle);

        public static bool DrawMediumButton(string label) =>
            DrawMediumButton(label, EditorStyles.miniButton);
        
        public static bool DrawMediumButton(string label, GUIStyle style) =>
            GUILayout.Button(label, style, EditorGUIStyles.MediumButtonStyle);
    }
}