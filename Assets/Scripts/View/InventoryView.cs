using Model;
using Model.Inventory;
using UnityEngine;
using View.Cards;
using Zenject;

namespace View
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

        private void OnEnable() => _inventory.OnStateChanged += UpdateView;

        private void OnDisable() => _inventory.OnStateChanged -= UpdateView;

        private void Start() => UpdateView();

        private void UpdateView()
        {
            var inventoryCells = _inventory.Cells;
            foreach (var inventoryCell in inventoryCells)
            {
                var view = _diContainer.InstantiatePrefabForComponent<CardView>(_cardViewPrefab, transform);
                view.Card = inventoryCell.Card;
            }
        }
    }
}