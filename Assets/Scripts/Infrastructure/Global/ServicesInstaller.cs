using Model;
using Model.Cards;
using Model.Merging;
using Model.Storage;
using Zenject;

namespace Infrastructure.Global
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindService<CardCreator>();
            BindService<Inventory>();
            BindService<MergeTablesContainer>();
            BindService<Hand>();
        }
        
        private void BindService<T>() =>
            Container
                .Bind(typeof(T))
                .AsSingle()
                .NonLazy();
    }
}