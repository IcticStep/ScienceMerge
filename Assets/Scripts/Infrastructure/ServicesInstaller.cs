using Model;
using Zenject;

namespace Infrastructure
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindService<CardCreator>();
            BindService<Inventory>();
        }
        
        private void BindService<T>() =>
            Container
                .Bind(typeof(T))
                .AsSingle()
                .NonLazy();
    }
}