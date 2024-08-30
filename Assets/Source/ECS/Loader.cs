using Data;
using Leopotam.Ecs;
using Systems;
using UnityEngine;

namespace ECS
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private UnitInitData _playerInitData;
        [SerializeField] private UnitInitData _enemyInitData;
        [SerializeField] private Transform _spawnPoint;

        private EcsWorld _world;
        private EcsSystems _systems;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _systems.Add(new GameInitSystem(_playerInitData, _enemyInitData, _spawnPoint));
            _systems.Add(new PlayerInputSystem());
            _systems.Add(new PlayerMoveSystem());
            _systems.Add(new PlayerRotateSystem());
            _systems.Add(new FollowMoveSystem());
            _systems.Add(new FollowRotateSystem());
            _systems.Add(new MoveParticleSystem());
            //_systems.Add(new AnimatedCharacterSystem());

            _systems.Init();
        }

        private void Update()
        {
            _systems.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _world?.Destroy();
        }
    }
}