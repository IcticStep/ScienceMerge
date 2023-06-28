using UnityEditor;
using UnityEngine;

namespace Editor.ConfigurationTools.Common
{
    public class ListSerializer<T>
        where T : ScriptableObject
    {
        public ListSerializer(string resourceName, string listPropertyName)
        {
            _resourceName = resourceName;
            _listPropertyName = listPropertyName;
            Initialize();
        }

        private readonly string _resourceName;
        private readonly string _listPropertyName;

        private T _target;
        private SerializedObject _serializedObject;
        private SerializedProperty _listProperty;

        public SerializedProperty ListProperty
        {
            get => _listProperty.Copy();
            private set => _listProperty = value;
        }

        public T Target => _target;

        public void UpdateSerialized()
        {
            if (Target is null)
            {
                Initialize();
                return;
            }

            CreateSerializedObjectIfNone();
            
            ListProperty = _serializedObject.FindProperty(_listPropertyName);
            _serializedObject.Update();
        }

        public void Save()
        {
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void Load() => 
            _target = Resources.Load<T>(_resourceName);

        public bool ApplyModifiedProperties() => 
            _serializedObject.ApplyModifiedProperties();

        private void Initialize()
        {
            Load();
            CreateAssetIfNotExist();
            CreateSerializedObjectIfNone();
            UpdateSerialized();
        }

        private void CreateAssetIfNotExist()
        {
            if(Target is not null) 
                return;
            
            _target = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(Target, $"{ConfigurationPaths.CardsToolsDataPath}{_resourceName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private void CreateSerializedObjectIfNone() => 
            _serializedObject ??= new SerializedObject(Target);
    }
}