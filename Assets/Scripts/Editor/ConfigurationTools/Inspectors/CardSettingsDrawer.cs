using System;
using System.Collections.Generic;
using System.Linq;
using Configurations;
using Editor.Common;
using UnityEditor;
using UnityEngine;

namespace Editor.ConfigurationTools.Inspectors
{
    [CustomPropertyDrawer(typeof(CardSettings))]
    public class CardSettingsDrawer : PropertyDrawer
    {
        private const float HeaderPercentsWidth = 0.17f;
        private const string ChangeablePropertyPrefix = "  ";
        
        private readonly PropertySetting _id = new("_id", "Id:", 0.06f, 30);
        private readonly PropertySetting _name = new("_name", "Name", 0.2125f, 50);
        private readonly PropertySetting _mergeSeconds = new("_mergeSeconds", "Merge Seconds", 0.22f, 110);
        private readonly PropertySetting _price = new("_price", "Price", 0.125f, 50);
        private readonly PropertySetting _sprite = new("_sprite", "Sprite", 0.2125f, 50);
        private PropertySetting[] _childProperties;
        
        private float _totalPropertyWidth;
        private Rect _contentPosition;

        private IEnumerable<PropertySetting> ChangeableProperties => 
            _childProperties.Where(x => x != _id);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _totalPropertyWidth = position.width;
            _contentPosition = position;
            
            InitRelativeProperties();
            UpdatePropertyReferences(property);

            DrawHeader();
            DrawIDField();
            DrawChangeableProperties();
        }

        private void InitRelativeProperties() => 
            _childProperties ??= new[] { _id, _name, _mergeSeconds, _price, _sprite };

        private void UpdatePropertyReferences(SerializedProperty property)
        {
            foreach (var relativeProperty in _childProperties)
                relativeProperty.Reference = property.FindPropertyRelative(relativeProperty.Name);
        }

        private void DrawIDField()
        {
            var idContent = new GUIContent($"{_id.LabelText} {_id.Reference.intValue.ToString()}");
            DrawEditorGUIHorizontally(
                position => EditorGUI.LabelField(position, idContent),
                _id.TotalPercentsWidth);
        }

        private void DrawHeader()
        {
            var header = new GUIContent($"[{_id.Reference.intValue}] {_name.Reference.stringValue}");
            DrawEditorGUIHorizontally(
                position => EditorGUI.PrefixLabel(position, header),
                HeaderPercentsWidth);
        }

        private void DrawEditorGUIHorizontally(Action<Rect> drawCall, float width)
        {
            _contentPosition.width = _totalPropertyWidth * width;
            drawCall.Invoke(_contentPosition);
            _contentPosition.x += _contentPosition.width;
        }

        private void DrawChangeableProperties()
        {
            foreach (var relativeProperty in ChangeableProperties)
            {
                EditorGUIUtility.labelWidth = relativeProperty.LabelPixelLength;
                var relativeLabel = new GUIContent(ChangeablePropertyPrefix + relativeProperty.LabelText);

                DrawEditorGUIHorizontally(
                    position => EditorGUI.PropertyField(position, relativeProperty.Reference, relativeLabel),
                    relativeProperty.TotalPercentsWidth);
            }
        }
    }
}