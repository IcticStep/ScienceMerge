using UnityEngine;

namespace Editor.Common
{
    public static class EditorGUIStyles
    {
        public const string AddButtonLabel = "+";
        public const string RemoveButtonLabel = "-";
        public const string ClearSearchButtonLabel = "X";
        public const string UpdateOrderButtonLabel = "Sort";
        public const float SmallButtonWidth = 40f;
        public const float MediumButtonWidth = 60f;
        public const float DownButtonsTopMargin = -5f;
        
        public static readonly GUILayoutOption SmallButtonStyle = GUILayout.Width(SmallButtonWidth);
        public static readonly GUILayoutOption MediumButtonStyle = GUILayout.Width(MediumButtonWidth);
    }
}