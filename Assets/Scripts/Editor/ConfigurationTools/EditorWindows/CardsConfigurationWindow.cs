using System.Collections.Generic;
using System.Linq;
using Configurations;
using Editor.Common;
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
        
        private CardsConfiguration _target;
        private SerializedObject _serializedObject;
        private SerializedProperty _listProperty;
        private ListDrawer _listDrawer;
        private List<int> _searchResult;
        private string _searchPrompt = "";

        [MenuItem(CardsToolsPath + EditorName)]
        public static void ShowWindow()
        {
            var window = GetWindow<CardsConfigurationWindow>();
            window.Initialize();
        }

        private void OnGUI()
        {
            SetTitle();
            UpdateSerialized();

            InitListDrawer();
            DrawSearchBar();
            DrawList();
            
            ApplyChanges();
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            Save();
        }

        private void Initialize()
        {
            LoadAsset();
            CreateAssetIfNotExist();
            CreateSerializedObjectIfNone();
            UpdateSerialized();
        }

        private void CreateAssetIfNotExist()
        {
            if(_target is not null) 
                return;
            
            _target = CreateInstance<CardsConfiguration>();
            AssetDatabase.CreateAsset(_target , $"Assets/Configuration/Resources/{ResourceName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void UpdateSerialized()
        {
            if (_target is null)
            {
                Initialize();
                return;
            }

            CreateSerializedObjectIfNone();
            
            _listProperty = _serializedObject.FindProperty(ListPropertyName);
            _serializedObject.Update();
        }

        private void SetTitle() => 
            titleContent = new GUIContent(EditorName);

        private void InitListDrawer()
        {
            _listDrawer = new(_listProperty, addingItem: AddElement);
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
            var settingsList = _target.CardSettingsList;
            return settingsList
                .Where((x) => x.Name.ToLowerInvariant().Contains(_searchPrompt))
                .Select(x => x.Id)
                .ToList();
        }

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
            var filter = GetListFilter();
            _listDrawer.DrawScrollable(filter);
        }

        private void AddElement()
        {
            _target.AddEmptyCard();
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

        private void ApplyChanges()
        {
            var modified = _serializedObject.ApplyModifiedProperties();
            if (modified)
                SaveChanges();
        }

        private void CreateSerializedObjectIfNone() => 
            _serializedObject ??= new SerializedObject(_target);

        private void LoadAsset() => 
            _target = Resources.Load<CardsConfiguration>(ResourceName);

        private void Save()
        {
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}