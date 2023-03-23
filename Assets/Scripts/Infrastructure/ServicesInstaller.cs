using Model;
using Model.Inventory;
using Zenject;

namespace Infrastructure
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindService<CardCreator>();
            BindService<Inventory>();
            BindService<Hand>();
            BindService<MergeTablesContainer>();
        }
        
        private void BindService<T>() =>
            Container
                .Bind(typeof(T))
                .AsSingle()
                .NonLazy();
    }
}