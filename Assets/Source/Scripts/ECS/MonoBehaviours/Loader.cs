using System;
using Data;
using ECS.Data;
using Leopotam.Ecs;
using Systems;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private UnitInitConfig _playerInitConfig;
        [SerializeField] private UnitInitConfig _enemyInitConfig;
        [SerializeField] private WeaponInitConfig _turretWeaponInitConfig;
        [SerializeField] private WeaponInitConfig _mainWeaponInitConfig;
        [SerializeField] private TurretInitConfig _turretInitConfig;
        [SerializeField] private PickUpsInitConfig _pickUpsInitConfig;

        [SerializeField] private Transform _spawnPoint;

        private EcsWorld _world;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;
        private EcsSystems _lateUpdateSystems;

        private void Start()
        {
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);

            _updateSystems.Add(new GameInitSystem(
                _playerInitConfig,
                _enemyInitConfig,
                _turretInitConfig,
                _spawnPoint,
                _turretWeaponInitConfig,
                _pickUpsInitConfig, _mainWeaponInitConfig));
            _updateSystems.Add(new MoveParticleSystem());
            _updateSystems.Add(new DetectionSystem());
            _updateSystems.Add(new AutofireWeaponSystem());
            _updateSystems.Add(new RechargingSystem());
            _updateSystems.Add(new ReloadSystem());
            _updateSystems.Add(new CameraFollowSystem());
            _updateSystems.Add(new ShootInputSystem());
            _updateSystems.Add(new PlayerFireWeaponSystem());

            _fixedUpdateSystems.Add(new PlayerRotationInputSystem());
            _fixedUpdateSystems.Add(new TrackingSystem());
            _fixedUpdateSystems.Add(new TrackingFollowSystem());
            _fixedUpdateSystems.Add(new PlayerMoveInputSystem());
            _fixedUpdateSystems.Add(new PlayerRigidbodyMoveSystem());
            _fixedUpdateSystems.Add(new PlayerRigidbodyRotationSystem());
            _fixedUpdateSystems.Add(new RigidbodyFollowRotateSystem());
            _fixedUpdateSystems.Add(new RigidbodyFollowMoveSystem());
            //_fixedUpdateSystems.Add(new FollowMoveSystem());
            //_fixedUpdateSystems.Add(new FollowRotateSystem());
            _fixedUpdateSystems.Add(new SelfCollisionParticleSystem());
            _fixedUpdateSystems.Add(new InstanceCollisionParticleSystem());
            _fixedUpdateSystems.Add(new ShootingSystem());
            _fixedUpdateSystems.Add(new RigidbodyInstantMoveSystem());
            _fixedUpdateSystems.Add(new CollisionDamageSystem());
            _fixedUpdateSystems.Add(new HealthDamageSystem());
            _fixedUpdateSystems.Add(new CameraDistanceSystem());
            //_updateSystems.Add(new PlayerMoveSystem());
            //_systems.Add(new AnimatedCharacterSystem());
            _fixedUpdateSystems.Add(new ProjectileCollisionSystem());
            _fixedUpdateSystems.Add(new CollisionComponentDestructionSystem());
            _fixedUpdateSystems.Add(new DestroySystem());

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