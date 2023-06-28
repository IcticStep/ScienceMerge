using System;
using Editor.Common;
using UnityEditor;

namespace Editor.ConfigurationTools.Common
{
    public class SavingSwitcher
    {
        public SavingSwitcher(Action onSave) 
            => OnSave = onSave;

        private const string AutoSaveSettingName = "CardsConfiguration_AutoSave";
        private Action OnSave;

        public bool AutoSave { get; private set; }
        
        public void LoadSettings() => 
            AutoSave = EditorPrefs.GetBool(AutoSaveSettingName);

        public void SaveSettings() => 
            EditorPrefs.SetBool(AutoSaveSettingName, AutoSave);
        
        public void DrawSettingsBar()
        {
            EditorGUILayoutComposer.DrawHorizontally(DrawBar);
            
            void DrawBar()
            {
                AutoSave = EditorGUILayout.Toggle("Auto-save", AutoSave);
                EditorGUILayoutComposer.DrawToggling(DrawSaveButton, enabled:!AutoSave);
            }

            void DrawSaveButton()
            {
                var saving = EditorGUILayoutComposer.DrawMediumButton("Save");
                if(saving)
                    OnSave();
            }
        }
    }
}