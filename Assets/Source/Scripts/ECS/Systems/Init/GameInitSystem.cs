using System.Collections.Generic;
using Data;
using ECS.Components;
using ECS.Components.Input;
using ECS.Components.Movement;
using ECS.MonoBehaviours;
using EntityActors;
using Leopotam.Ecs;
using UnityEngine;
using Utilitiy;

namespace Systems
{
    public class GameInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;

        private readonly UnitInitData _playerInitData;
        private readonly UnitInitData _enemyInitData;

        private readonly Transform _spawnPoint;
        private readonly TurretInitData _turretInitData;
        private readonly WeaponInitData _turretWeaponInitData;
        private readonly PickUpsInitData _pickUpsInitData;
        private readonly WeaponInitData _mainWeaponInitData;
        private WeaponBuilder _weaponBuilder;

        public GameInitSystem(UnitInitData playerInitData,
            UnitInitData enemyInitData,
            TurretInitData turretInitData,
            Transform spawnPoint,
            WeaponInitData turretWeaponInitData,
            PickUpsInitData pickUpsInitData,
            WeaponInitData mainWeaponInitData)
        {
            _playerInitData = playerInitData;
            _enemyInitData = enemyInitData;
            _turretInitData = turretInitData;
            _spawnPoint = spawnPoint;
            _turretWeaponInitData = turretWeaponInitData;
            _mainWeaponInitData = mainWeaponInitData;
            _pickUpsInitData = pickUpsInitData;
        }

        public void Init()
        {
            _weaponBuilder = new WeaponBuilder(_world);
            var unitBuilder = new UnitBuilder(_world);
            var playerActor = unitBuilder.BuildPlayer(_playerInitData, _spawnPoint.position);
            var turretBuilder = new TurretBuilder(_world);

            foreach (var weaponPlace in playerActor.FrontWeaponsPlaceholders)
            {
                var weapon = _weaponBuilder.Build(
                    _mainWeaponInitData,
                    weaponPlace,
                    weaponPlace.position);
                weapon.Get<ShootInputComponent>();
            }

            foreach (var placeholder in playerActor.TurretPlaceholders)
            {
                turretBuilder.CreateTurret(_turretInitData, placeholder, playerActor.Rigidbody2D);
            }

            for (int i = 0; i < 20; i++)
            {
                var enemySpawnPosition = _spawnPoint.position +
                                         new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0f);
                unitBuilder.BuildEnemy(_enemyInitData, enemySpawnPosition, playerActor.transform);
                CreatePickUps();
            }
        }

        private void CreatePickUps()
        {
            var pickUpSpawnPosition = _spawnPoint.position +
                                      new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0f);
            var pickUpActor =
                Object.Instantiate(_pickUpsInitData.PickUpActor, pickUpSpawnPosition, Quaternion.identity);
            var pickUp = _world.NewEntity();
            pickUpActor.GetComponent<ColliderObserver>().Initialize(_world, pickUp);

            ref var healthRefillComponent = ref pickUp.Get<HealthRefillComponent>();
            healthRefillComponent.refillAmount = _pickUpsInitData.HaalthRestoreValue;

            ref var collisionParticleComponent = ref pickUp.Get<InstanceCollisionParticleComponent>();
            collisionParticleComponent.particleSystem = pickUpActor.CollisionParticleSystem;

            ref var collisionObjectDestructionComponent = ref pickUp.Get<CollisionObjectDestructionComponent>();
            collisionObjectDestructionComponent.destroyObject = pickUpActor.gameObject;
        }


    }
}