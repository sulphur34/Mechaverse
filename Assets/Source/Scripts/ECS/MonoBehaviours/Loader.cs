using System;
using Data;
using ECS.Data;
using Leopotam.Ecs;
using Systems;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace ECS
{
    public class Loader : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;
        private EcsSystems _lateUpdateSystems;
        private GameInitSystem _gameInitSystem;

        [Inject]
        public void Construct(EcsWorld world, GameInitSystem gameInitSystem)
        {
            _world = world;
            _updateSystems = new EcsSystems(world);
            _fixedUpdateSystems = new EcsSystems(world);
            _gameInitSystem = gameInitSystem;
        }

        private void Start()
        {
            _updateSystems.Add(_gameInitSystem);
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