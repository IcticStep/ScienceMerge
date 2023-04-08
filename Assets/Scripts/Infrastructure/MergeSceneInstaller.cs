using Infrastructure.Cameras;
using UnityEngine;
using View;
using View.Merging;
using Zenject;

namespace Infrastructure
{
    public class MergeSceneInstaller : MonoInstaller
    {
        [SerializeField] private HandView _handView;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Camera _handCamera;
        [SerializeField] private MergeTableViewsContainer _mergeTableViewsContainer;

        public override void InstallBindings()
        {
            BindCamera();
            BindMainCamera();
            BindSingleInstance(_handView);
            BindSingleInstance(_mergeTableViewsContainer);
        }

        private void BindSingleInstance<T>(T instance) =>
            Container
                .Bind<T>()
                .FromInstance(instance)
                .AsSingle();

        private void BindCamera() =>
            Container
                .Bind<Camera>()
                .WithId(CamerasIDs.HandCamera)
                .FromInstance(_handCamera);

        private void BindMainCamera() =>
            Container
                .Bind<Camera>()
                .WithId(CamerasIDs.MainCamera)
                .FromInstance(_mainCamera);
    }
}