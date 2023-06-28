using System.Collections.Generic;
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
    public class CardsConfigurationWindow : EditorWindow
    {
        private const string EditorName = "Cards Configuration";
        private const string ResourceName = "CardsConfiguration";
        private const string ListPropertyName = "_cardSettings";
        
        private const int MinEditorLines = 2;
        private readonly Vector2 _minEditorSize = new(650, 118);
        
        private SavingSwitcher _savingSwitcher;
        private ListSerializer<CardsConfiguration> _listSerializer;
        private ListDrawer _listDrawer;
        private List<int> _searchResult;
        private string _searchPrompt = "";
        private int _listLines;

        private CardsConfiguration Target => _listSerializer.Target;

        [MenuItem(CardsToolsMenuPath + EditorName)]
        public static void ShowWindow()
        {
            var window = GetWindow<CardsConfigurationWindow>();
            window.Initialize();
        }

        private void OnGUI()
        {
            UpdateSerialized();

            SetTitle();
            DrawSaveSettingsBar();
            DrawSearchBar();
            DrawList();
            
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

        public override void SaveChanges()
        {
            base.SaveChanges();
            Save();
        }

        private void Initialize()
        {
            InitializeSavingSwitcher();
            InitializeListSerializer();
            SetMinEditorSize();
            Load();
            InitListDrawer();
        }

        private void InitializeListSerializer() => 
            _listSerializer = new(ResourceName, ListPropertyName);

        private void UpdateSerialized()
        {
            if(_listSerializer is null)
                InitializeListSerializer();
            _listSerializer!.UpdateSerialized();
        }

        private void SetTitle() => 
            titleContent = new GUIContent(EditorName);

        private void DrawSaveSettingsBar() => 
            _savingSwitcher.DrawSettingsBar();

        private void DrawSearchBar()
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

        private void DrawList()
        {
            if(_listDrawer is null)
                InitListDrawer();
            
            CorrectListSize();

            var filter = GetListFilter();
            _listDrawer!.DrawScrollable(filter);
        }

        private void ApplyChanges()
        {
            var modified = _listSerializer.ApplyModifiedProperties();
            if (!modified || !_savingSwitcher.AutoSave) 
                return;
            
            SaveChanges();
        }

        private void InitializeSavingSwitcher()
        {
            _savingSwitcher = new(this);
            _savingSwitcher.OnSave += SaveChanges;
        }

        private void SetMinEditorSize() => 
            minSize = _minEditorSize;

        private void InitListDrawer()
        {
            _listDrawer = new(_listSerializer.ListProperty, addingItem: AddElement, visibleLines:10);
            _listDrawer.OnStateChange += AddElement;
        }

        private void DrawSortButton()
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(UpdateOrderButtonLabel, EditorStyles.miniButton, MediumButtonStyle))
                Search();
        }

        private IEnumerable<int> Search()
        {
            var settingsList = Target.CardSettingsList;
            return settingsList
                .Where(x => x.Name.ToLowerInvariant().Contains(_searchPrompt))
                .Select(x => x.Id)
                .ToList();
        }

        private void CorrectListSize()
        {
            var windowHeight = rootVisualElement.worldBound.height;
            var lineHeight = EditorGUILayoutComposer.EditorLineHeight;
            
            _listDrawer.VisibleLines = 
                (int)((windowHeight - _minEditorSize.y) / lineHeight + MinEditorLines);
        }

        private void AddElement()
        {
            Target.AddEmptyCard();
            Repaint();
            Save();
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

        private void Save() => 
            _listSerializer.Save();

        private void Load() => 
            _listSerializer.Load();
    }
}