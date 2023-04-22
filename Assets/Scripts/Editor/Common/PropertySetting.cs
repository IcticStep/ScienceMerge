using UnityEditor;

namespace Editor.Common
{
    public class PropertySetting
    {
        public PropertySetting(SerializedProperty reference, string name, string labelText,
            float totalPercentsWidth, float labelPixelLength)
        {
            Reference = reference;
            Name = name;
            LabelText = labelText;
            TotalPercentsWidth = totalPercentsWidth;
            LabelPixelLength = labelPixelLength;
        }

        public PropertySetting(string className, string labelText, float totalPercentsLength, float labelPixelLenght)
            : this(null, className, labelText, totalPercentsLength, labelPixelLenght) { }

        public SerializedProperty Reference { get; set; }
        public readonly string Name;
        public readonly string LabelText;
        public readonly float TotalPercentsWidth;
        public readonly float LabelPixelLength;
    }
}