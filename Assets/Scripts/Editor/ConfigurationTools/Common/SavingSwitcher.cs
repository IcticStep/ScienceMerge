using System;
using Editor.Common;
using UnityEditor;

namespace Editor.ConfigurationTools.Common
{
    public class SavingSwitcher
    {
        public SavingSwitcher(EditorWindow window) => 
            _window = window;
        
        private const string AutoSaveSettingName = "CardsConfiguration_AutoSave";
        private const string NotificationText = "Saved!";
        private const double NotificationFadeTime = 0.3;

        public event Action OnSave;
        private readonly EditorWindow _window;

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
                    HandleSaving();
            }
        }

        private void HandleSaving()
        {
            OnSave?.Invoke();
            ShowSavedNotification();
        }

        private void ShowSavedNotification() => 
            _window.ShowNotification(
                new(NotificationText),
                NotificationFadeTime);
    }
}