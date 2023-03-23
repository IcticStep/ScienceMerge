using UnityEngine;
using View;
using Zenject;

namespace Infrastructure
{
    public class MergeSceneInstaller : MonoInstaller
    {
        [SerializeField] private HandView _handView;
        public override void InstallBindings()
        {
            BindHandView();
        }

        private void BindHandView() =>
            Container
                .Bind<HandView>()
                .FromInstance(_handView)
                .AsSingle();
    }
}