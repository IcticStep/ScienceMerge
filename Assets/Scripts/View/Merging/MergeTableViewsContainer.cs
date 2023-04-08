using System.Collections.Generic;
using System.Linq;
using Infrastructure.Cameras;
using Model.Merging;
using UnityEngine;
using Zenject;

namespace View.Merging
{
    public class MergeTableViewsContainer : MonoBehaviour
    {
        [Inject]
        private void Construct(MergeTablesContainer mergeTablesContainer, DiContainer diContainer, 
            [Inject(Id = CamerasIDs.HandCamera)] Camera handCamera) =>
            (_mergeTablesContainer, _diContainer, _handCamera) =
            (mergeTablesContainer, diContainer, handCamera);


        [SerializeField] 
        private MergeTableView _mergeTableViewPrefab;

        private readonly List<MergeTableView> _mergeTableViews = new();
        private MergeTablesContainer _mergeTablesContainer;
        private DiContainer _diContainer;
        private Camera _handCamera;

        [field:SerializeField]
        public Canvas MergeCanvas { get; private set; }
        public IReadOnlyList<MergeTableView> MergeTableViews => _mergeTableViews;

        private void OnEnable() => _mergeTablesContainer.OnStateChanged += UpdateView;
        private void OnDisable() => _mergeTablesContainer.OnStateChanged -= UpdateView;

        private void Awake() => 
            MergeCanvas.GetComponent<RectTransform>();

        private void Start() => UpdateView();

        public IEnumerable<(MergeTableView TableView, float Distance)> GetDistancesToTables(Vector2 cardScreenPosition)
        {
            return _mergeTableViews
                .Select(table => 
                    (Table: table,
                     Distance: Vector2.Distance(GetUIElementScreenPosition(table.RectTransform), cardScreenPosition)))
                .OrderBy(x => x.Distance);
        }

        private Vector2 GetUIElementScreenPosition(Transform rectTransform) => 
            _handCamera.WorldToScreenPoint(rectTransform.position);

        private void UpdateView()
        {
            var mergeTables = _mergeTablesContainer.MergeTables;
            foreach (var mergeTable in mergeTables)
            {
                var view = _diContainer
                    .InstantiatePrefabForComponent<MergeTableView>(
                        _mergeTableViewPrefab, transform);
                
                view.MergeTable = mergeTable;
                _mergeTableViews.Add(view);
            }
        }
    }
}