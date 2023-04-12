using Configurations;
using UnityEngine;
using Zenject;

namespace Infrastructure.Global
{
    public class ConfigurationsInstaller : MonoInstaller
    {
        [SerializeField] private CardsConfiguration _cardsConfiguration;
        [SerializeField] private InventoryConfiguration _inventoryConfiguration;
        [SerializeField] private MergeConfiguration _mergeConfiguration;
        
        public override void InstallBindings()
        {
            BindConfiguration(_cardsConfiguration);
            BindConfiguration(_inventoryConfiguration);
            BindConfiguration(_mergeConfiguration);
        }

        private void BindConfiguration<T>(T instance) =>
            Container
                .Bind<T>()
                .FromInstance(instance)
                .AsSingle()
                .NonLazy();
    }
}