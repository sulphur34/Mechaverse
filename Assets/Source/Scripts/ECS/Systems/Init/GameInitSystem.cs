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
            var unitBuilder = new UnitBuilder(_world);
            var playerActor = unitBuilder.BuildPlayer(_playerInitData, _spawnPoint.position);
            foreach (var weaponPlace in playerActor.FrontWeaponsPlaceholders)
            {
                var weapon = CreateWeapon(
                    _mainWeaponInitData,
                    _mainWeaponInitData.WeaponActor,
                    weaponPlace,
                    weaponPlace.position);
                weapon.Get<ShootInputComponent>();
            }

            CreateTurret(playerActor);

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

        private void CreateTurret(UnitActor unitActor)
        {
            foreach (var placeholder in unitActor.TurretPlaceholders)
            {
                var turretActor = Object.Instantiate(
                    _turretInitData.TurretPrefab,
                    placeholder.transform.position,
                    Quaternion.identity);

                var turret = _world.NewEntity();
                turretActor.HingeJoint.connectedBody = unitActor.Rigidbody2D;
                turretActor.HingeJoint.anchor = Vector2.zero;
                turretActor.HingeJoint.connectedAnchor = placeholder.transform.position;

                ref var trackerComponent = ref turret.Get<TrackerComponent>();
                trackerComponent.searchRadius = _turretInitData.TrackerRange;
                trackerComponent.selfTeam = Teams.Player;
                trackerComponent.selfTransform = turretActor.transform;

                ref var rotatableComponent = ref turret.Get<RigidbodyRotatableComponent>();
                rotatableComponent.rigidbody = turretActor.Rigidbody2D;
                rotatableComponent.rotationData = _playerInitData.RotationData;

                ref var detectionComponent = ref turret.Get<DetectionComponent>();
                detectionComponent.angle = _turretInitData.DetectAngle;
                detectionComponent.radius = _turretInitData.DetectRadius;

                turret.Get<FollowComponent>();

                ref var turretComponent = ref turret.Get<TurretComponent>();

                turretComponent.weapons = new List<EcsEntity>();
                foreach (var position in turretActor.WeaponPositions)
                {
                    var weapon = CreateWeapon(_turretWeaponInitData, turretActor.WeaponActor, turretActor.transform,
                        position);
                    turretComponent.weapons.Add(weapon);
                }
            }
        }

        private EcsEntity CreateWeapon(WeaponInitData weaponInitData, WeaponActor weaponPrefab, Transform container,
            Vector2 position)
        {
            var weaponActor = Object.Instantiate(weaponPrefab, container);
            weaponActor.transform.position = position;
            var weapon = _world.NewEntity();

            ref var weaponComponent = ref weapon.Get<WeaponComponent>();
            weaponComponent.projectile = weaponInitData.ProjectilePrefab;
            weaponComponent.reloadingSpeed = weaponInitData.ReloadingSpeed;
            weaponComponent.shotDelay = weaponInitData.ShotDelay;
            weaponComponent.shotForce = weaponInitData.ShotForce;
            weaponComponent.shootingPoint = weaponActor.ShootPoint;
            weaponComponent.damageValue = weaponInitData.DamageValue;

            ref var ammoComponent = ref weapon.Get<AmmoComponent>();
            ammoComponent.maxCapacity = weaponInitData.MagazineCapacity;
            ammoComponent.currentCapacity = ammoComponent.maxCapacity;

            return weapon;
        }
    }
}