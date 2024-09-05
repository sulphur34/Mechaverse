using System;
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
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;

        private void Start()
        {
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);

            _updateSystems.Add(new GameInitSystem(_playerInitData, _enemyInitData, _spawnPoint));
            _updateSystems.Add(new MoveParticleSystem());

            _fixedUpdateSystems.Add(new PlayerRotationInputSystem());
            _fixedUpdateSystems.Add(new PlayerMoveInputSystem());
            _fixedUpdateSystems.Add(new PlayerRotateSystem());
            _fixedUpdateSystems.Add(new PlayerRigidbodyMoveSystem());
            _fixedUpdateSystems.Add(new FollowMoveSystem());
            _fixedUpdateSystems.Add(new FollowRotateSystem());
            //_updateSystems.Add(new PlayerMoveSystem());
            //_systems.Add(new AnimatedCharacterSystem());

            _updateSystems.Init();
            _fixedUpdateSystems.Init();
        }

        private void Update()
        {
            _updateSystems.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystems.Run();
        }

        private void OnDestroy()
        {
            _updateSystems?.Destroy();
            _world?.Destroy();
        }
    }
}