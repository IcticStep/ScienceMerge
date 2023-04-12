using System.Collections.Generic;
using Model.Storage;
using UnityEngine;
using View.Cards;
using Zenject;

namespace View.Storage
{
    public class InventoryView : MonoBehaviour
    {
        [Inject]
        private void Construct(Inventory inventory, DiContainer diContainer) =>
            (_inventory, _diContainer) = (inventory, diContainer);

        [SerializeField] 
        private CardView _cardViewPrefab;
        
        private Inventory _inventory;
        private DiContainer _diContainer;
        private readonly List<CardView> _views = new();

        private void OnEnable() => _inventory.OnStateChanged += UpdateView;

        private void OnDisable() => _inventory.OnStateChanged -= UpdateView;

        private void Start() => UpdateView();

        private void UpdateView()
        {
            var inventoryCells = _inventory.Cells;
            
            GetEnoughViews(inventoryCells);

            for (var i = 0; i < inventoryCells.Count; i++)
            {
                _views[i].Card = inventoryCells[i].Card;
                _views[i].SetCountText(inventoryCells[i].Count);                
            }
        }

        private void GetEnoughViews(IReadOnlyList<InventoryCell> inventoryCells)
        {
            if (inventoryCells.Count == _views.Count) return;

            AddMissingViews(inventoryCells);
            DestroyExtraViews(inventoryCells);
        }

        private void AddMissingViews(IReadOnlyList<InventoryCell> inventoryCells)
        {
            while (inventoryCells.Count > _views.Count)
            {
                var view = _diContainer.InstantiatePrefabForComponent<CardView>(_cardViewPrefab, transform);
                _views.Add(view);
            }
        }

        private void DestroyExtraViews(IReadOnlyList<InventoryCell> inventoryCells)
        {
            while (inventoryCells.Count < _views.Count)
            {
                Destroy(_views[^1].gameObject);
                _views.RemoveAt(_views.Count - 1);
            }
        }
    }
}