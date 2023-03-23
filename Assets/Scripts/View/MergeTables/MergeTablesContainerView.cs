using Model;
using UnityEngine;
using Zenject;

namespace View.MergeTables
{
    public class MergeTablesContainerView : MonoBehaviour
    {
        [Inject]
        private void Construct(MergeTablesContainer mergeTablesContainer, DiContainer diContainer) =>
            (_mergeTablesContainer, _diContainer) = (mergeTablesContainer, diContainer);

        [SerializeField] 
        private MergeTableView _mergeTableViewPrefab;
        
        private MergeTablesContainer _mergeTablesContainer;
        private DiContainer _diContainer;
        
        private void OnEnable() => _mergeTablesContainer.OnStateChanged += UpdateView;

        private void OnDisable() => _mergeTablesContainer.OnStateChanged -= UpdateView;

        private void Start() => UpdateView();

        private void UpdateView()
        {
            var mergeTables = _mergeTablesContainer.MergeTables;
            foreach (var mergeTable in mergeTables)
            {
                var view = _diContainer
                    .InstantiatePrefabForComponent<MergeTableView>(
                        _mergeTableViewPrefab, transform);
                
                view.MergeTable = mergeTable;
            }
        }
    }
}