using System;
using Configurations;
using UnityEngine;
using View;
using Zenject;

namespace Infrastructure
{
    public class ConfigurationsInstaller : MonoInstaller
    {
        [SerializeField] 
        private CardsConfiguration _cardsConfiguration;
        [SerializeField] 
        private InventoryConfiguration _inventoryConfiguration;
        
        public override void InstallBindings()
        {
            BindConfiguration(_cardsConfiguration);
            BindConfiguration(_inventoryConfiguration);
        }

        private void BindConfiguration<T>(T instance) =>
            Container
                .Bind<T>()
                .FromInstance(instance)
                .AsSingle()
                .NonLazy();
    }
}