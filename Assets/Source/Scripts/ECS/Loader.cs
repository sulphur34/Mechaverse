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
        [SerializeField] private WeaponInitData _weaponInitData;
        [SerializeField] private TurretInitData _turretInitData;

        [SerializeField] private Transform _spawnPoint;

        private EcsWorld _world;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;

        private void Start()
        {
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);

            _updateSystems.Add(new GameInitSystem(_playerInitData, _enemyInitData, _turretInitData, _spawnPoint, _weaponInitData));
            _updateSystems.Add(new MoveParticleSystem());
            _updateSystems.Add(new DetectionSystem());
            _updateSystems.Add(new AutofireWeaponSystem());
            _updateSystems.Add(new RechargingSystem());
            _updateSystems.Add(new ReloadSystem());

            _fixedUpdateSystems.Add(new PlayerRotationInputSystem());
            _fixedUpdateSystems.Add(new TrackingSystem());
            _fixedUpdateSystems.Add(new TrackingFollowSystem());
            _fixedUpdateSystems.Add(new PlayerMoveInputSystem());
            _fixedUpdateSystems.Add(new PlayerRigidbodyMoveSystem());
            _fixedUpdateSystems.Add(new PlayerRigidbodyRotationSystem());
            _fixedUpdateSystems.Add(new RigidbodyFollowRotateSystem());
            _fixedUpdateSystems.Add(new FollowMoveSystem());
            _fixedUpdateSystems.Add(new FollowRotateSystem());
            _fixedUpdateSystems.Add(new CollisionParticleSystem());
            _fixedUpdateSystems.Add(new ShootingSystem());
            _fixedUpdateSystems.Add(new ProjectileCollisionSystem());
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