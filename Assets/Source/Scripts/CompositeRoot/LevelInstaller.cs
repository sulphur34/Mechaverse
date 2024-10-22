using Data;
using ECS;
using ECS.Data;
using ECS.MonoBehaviours;
using Leopotam.Ecs;
using Systems;
using UnityEngine;
using Zenject;

namespace CompositeRoot
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameData _gameData;
        [SerializeField] private SpawnPoint _spawnPoint;
        [SerializeField] private Loader _loader;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameData);
            Container.BindInstance(_spawnPoint);
            Container.Bind<EcsWorld>().AsSingle();
            Container.Bind<GameInitSystem>().AsSingle();

            // Container.Inject(_loader);
        }
    }
}