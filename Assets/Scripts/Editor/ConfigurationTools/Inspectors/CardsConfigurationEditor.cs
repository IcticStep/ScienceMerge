using System.Collections.Generic;
using System.Linq;
using Configurations;
using Editor.Common;
using UnityEditor;

namespace Editor.ConfigurationTools.Inspectors
{
    [CustomEditor(typeof(CardsConfiguration))]
    public class CardsConfigurationEditor : SearchableListEditor<CardsConfiguration>
    {
        private const string EditorHeader = "Cards configuration";
        private const string ListPropertyPath = "_cardSettings";
        private CardsConfiguration _target;

        private void OnEnable()
        {
            _target = target as CardsConfiguration;
            Initialize(EditorHeader, ListPropertyPath, Reorder);
        }

        protected override void AddEmptyElement() => _target.AddEmptyCard();

        protected override IEnumerable<int> Search(string searchPrompt)
        {
            var settingsList = _target.CardSettingsList;
            return settingsList
                .Where((x) => x.Name.ToLowerInvariant().Contains(searchPrompt))
                .Select(x => x.Id)
                .ToList();
        }

        private void Reorder() => _target.ReorderByID();
    }
}