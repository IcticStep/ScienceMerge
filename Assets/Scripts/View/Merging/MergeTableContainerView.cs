using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Cameras;
using Model.Cards;
using Model.Merging;
using UnityEngine;
using Zenject;

namespace View.Merging
{
    public class MergeTableContainerView : MonoBehaviour
    {
        [Inject]
        private void Construct(MergeTablesContainer mergeTablesContainer, DiContainer diContainer, 
            [Inject(Id = CamerasIDs.HandCamera)] Camera handCamera)
        {
            _mergeTablesContainer = mergeTablesContainer;
            _diContainer = diContainer;
            _handCamera = handCamera;
        }

        [SerializeField] 
        private MergeTableView _mergeTableViewPrefab;
        
        private readonly List<MergeTableView> _mergeTableViews = new();
        private MergeTablesContainer _mergeTablesContainer;
        private DiContainer _diContainer;
        private Camera _handCamera;

        public IReadOnlyList<MergeTableView> MergeTableViews => _mergeTableViews;

        private void OnEnable() => _mergeTablesContainer.OnStateChanged += UpdateView;
        private void OnDisable() => _mergeTablesContainer.OnStateChanged -= UpdateView;

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
            GetEnoughViews(mergeTables);

            for (var i = 0; i < mergeTables.Count; i++)
                _mergeTableViews[i].MergeTable = mergeTables[i];
        }

        private void GetEnoughViews(IReadOnlyList<MergeTable> tables)
        {
            if (tables.Count == _mergeTableViews.Count) return;

            AddMissingViews(tables);
            DestroyExtraViews(tables);
        }

        private void AddMissingViews(IReadOnlyList<MergeTable> tables)
        {
            while (tables.Count > _mergeTableViews.Count)
            {
                var view = _diContainer
                    .InstantiatePrefabForComponent<MergeTableView>(
                        _mergeTableViewPrefab, transform);
                _mergeTableViews.Add(view);
            }
        }

        private void DestroyExtraViews(IReadOnlyList<MergeTable> tables)
        {
            while (tables.Count < _mergeTableViews.Count)
            {
                Destroy(_mergeTableViews[^1]);
                _mergeTableViews.RemoveAt(_mergeTableViews.Count - 1);
            }
        }
    }
}