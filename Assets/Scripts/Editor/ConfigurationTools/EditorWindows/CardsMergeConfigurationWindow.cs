using System;
using System.Linq;
using Configurations;
using Editor.Common;
using Editor.ConfigurationTools.Common;
using UnityEditor;
using UnityEngine;
using static Editor.ConfigurationTools.Common.ConfigurationPaths;
using static Editor.Common.EditorGUIStyles;

namespace Editor.ConfigurationTools.EditorWindows
{
    public class CardsMergeConfigurationWindow : EditorWindow
    {
        private const string EditorName = "Merge Configuration";
        private const string ResourceName = "MergeConfiguration";
        private const string CardsConfigurationResourceName = "CardsConfiguration";
        private const string MergeRulesPropertyName = "_mergeRules";
        private readonly Vector2 _minEditorSize = new(650, 118);
        private const int MinEditorLines = 2;
        private const int SideDeadAreaSize = 55;

        private MergeConfiguration _mergeConfiguration;
        private CardsConfiguration _cardsConfiguration;
        private SerializedObject _serializedMergeConfiguration;
        private SavingSwitcher _savingSwitcher;
        private Vector2 _listScroll = Vector2.zero;

        private static bool AddButton => 
            GUILayout.Button(AddButtonLabel, EditorStyles.miniButtonLeft, SmallButtonStyle);

        private static bool DeleteLastButton =>
            GUILayout.Button(RemoveButtonLabel, EditorStyles.miniButtonRight, SmallButtonStyle);

        [MenuItem(CardsToolsMenuPath + EditorName)]
        public static void ShowWindow()
        {
            var window = GetWindow<CardsMergeConfigurationWindow>();
            window.Initialize();
        }

        private void OnGUI()
        {
            UpdateSerialized();

            SetTitle();
            DrawSaveSettingsBar();
            DrawList();
            DrawManageButtons();
            
            ApplyChanges();
        }

        private void OnEnable()
        {
            if(_savingSwitcher is null)
                InitializeSavingSwitcher();
            _savingSwitcher!.LoadSettings();
        }

        private void OnDisable() => 
            _savingSwitcher.SaveSettings();

        private void Initialize()
        {
            Load();
            CreateAssetIfNotExist();
            CreateSerializedObjectIfNone();
            UpdateSerialized();
        }

        private void InitializeSavingSwitcher()
        {
            _savingSwitcher = new(this);
            _savingSwitcher.OnSave += SaveChanges;
        }

        private void DrawSaveSettingsBar() => 
            _savingSwitcher.DrawSettingsBar();

        private void DrawList()
        {
            EditorGUILayoutComposer.DrawScrollable(() =>
            {
                var mergeRulesProperty = _serializedMergeConfiguration.FindProperty(MergeRulesPropertyName);
                for (var i = 0; i < mergeRulesProperty.arraySize; i++)
                    DrawMergeRule(i);
            }, ref _listScroll, GetListScrollableSize());
        }

        private int GetListScrollableSize()
        {
            var windowHeight = rootVisualElement.worldBound.height;
            var lineHeight = EditorGUILayoutComposer.EditorLineHeight;

            return (int)((windowHeight - _minEditorSize.y) / lineHeight + MinEditorLines);
        }

        private void DrawMergeRule(int index)
        {
            var mergeRule = _mergeConfiguration.MergeRules[index];

            EditorGUILayoutComposer.DrawNonScrollable(() =>
            {
                EditorGUILayoutComposer.DrawHorizontally(() =>
                {
                    DrawMergeRuleCardList(mergeRule);
                    DrawMergeRuleResult(index, mergeRule);
                });
            }, Math.Max(mergeRule.CardsID.Count, 2));
        }

        private void DrawMergeRuleCardList(MergeRule mergeRule) =>
            EditorGUILayoutComposer.DrawVertically(() =>
            {
                if (!mergeRule.CardsID.Any())
                {
                    GUILayout.Label("-");
                    GUILayout.Label("");
                    return;
                }
                
                foreach (var cardId in mergeRule.CardsID)
                    GUILayout.Label(GetNameByCardId(cardId));
                if(mergeRule.CardsID.Count == 1)
                    GUILayout.Label("");
            }, GetMergeRuleUiPartWidth());

        private void DrawMergeRuleResult(int index, MergeRule mergeRule) =>
            EditorGUILayoutComposer.DrawVertically(() =>
            {
                GUILayout.Label(GetNameByCardId(mergeRule.ResultID));
                DrawModifyButton(index);
            }, GetMergeRuleUiPartWidth());

        private void DrawCardUi(MergeRule mergeRule, int cardId)
        {
            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                GUILayout.Label(GetNameByCardId(cardId));
                EditorGUILayoutComposer.DrawMediumButton("Change");
            });
        }
        
        private static GUILayoutOption GetMergeRuleUiPartWidth() => 
            GUILayout.Width((EditorGUIUtility.currentViewWidth - SideDeadAreaSize) / 2);

        private void DrawManageButtons()
        {
            GUILayout.Space(DownButtonsTopMargin);
            const int buttonCount = 2;

            EditorGUILayoutComposer.DrawHorizontally(() =>
            {
                GUILayout.Space(EditorGUIUtility.currentViewWidth - (buttonCount + 1) * SmallButtonWidth);
                if (AddButton)
                {
                    _mergeConfiguration.AddEmptyCard();
                    _savingSwitcher.SaveSettings();
                }
                if (DeleteLastButton)
                {
                    _mergeConfiguration.DeleteLast();
                    _savingSwitcher.SaveSettings();
                }
            });
        }

        private void DrawModifyButton(int index)
        {
            if (GUILayout.Button("Modify"))
            {
            }
        }

        private string GetNameByCardId(int id) => 
            _cardsConfiguration[id].Name;

        private void UpdateSerialized()
        {
            if (_mergeConfiguration is null)
            {
                Initialize();
                return;
            }

            CreateSerializedObjectIfNone();
            _serializedMergeConfiguration.Update();
        }

        private void ApplyChanges()
        {
            var modified = _serializedMergeConfiguration.ApplyModifiedProperties();
            if (!modified || !_savingSwitcher.AutoSave) 
                return;
            
            SaveChanges();
        }
        
        public override void SaveChanges()
        {
            base.SaveChanges();
            Save();
        }
        
        private void Save()
        {
            EditorUtility.SetDirty(_mergeConfiguration);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private void Load()
        {
            _mergeConfiguration = Resources.Load<MergeConfiguration>(ResourceName);
            _cardsConfiguration = Resources.Load<CardsConfiguration>(CardsConfigurationResourceName);
        }

        private void CreateAssetIfNotExist()
        {
            if(_mergeConfiguration is not null) 
                return;
            
            _mergeConfiguration = CreateInstance<MergeConfiguration>();
            AssetDatabase.CreateAsset(_mergeConfiguration, 
                $"{CardsToolsDataPath}{ResourceName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private void CreateSerializedObjectIfNone() => 
            _serializedMergeConfiguration ??= new SerializedObject(_mergeConfiguration);
        
        private void SetTitle() => 
            titleContent = new GUIContent(EditorName);
    }
}