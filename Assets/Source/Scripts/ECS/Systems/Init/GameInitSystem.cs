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
            var playerActor = CreatePlayer();
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
                CreateEnemy(enemySpawnPosition, playerActor.transform);
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
                    var weapon = CreateWeapon(_turretWeaponInitData,turretActor.WeaponActor, turretActor.transform, position);
                    turretComponent.weapons.Add(weapon);
                }
            }
        }

        private EcsEntity CreateWeapon(WeaponInitData weaponInitData, WeaponActor weaponPrefab, Transform container, Vector2 position)
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

        private UnitActor CreatePlayer()
        {
            var unitActor = Object.Instantiate(_playerInitData.UnitPrefab, _spawnPoint.position, _spawnPoint.rotation);
            var player = _world.NewEntity();

            RemoveCollidersInteractions(unitActor.InternalColliders);

            player.Get<PlayerComponent>();
            player.Get<RotationInputEventComponent>();
            player.Get<MoveInputEventComponent>();
            player.Get<PlayerComponent>();
            player.Get<CollisionDestructionComponent>();

            ref var cameraComponent = ref player.Get<CameraComponent>();
            cameraComponent.camera = Camera.main;
            cameraComponent.defaultPosition = Camera.main.transform.position;
            cameraComponent.distanceRate = 2f;

            ref var targetableComponent = ref player.Get<TargetableComponent>();
            targetableComponent.transform = unitActor.transform;
            targetableComponent.team = Teams.Player;

            var colliderObserver = unitActor.GetComponent<ColliderObserver>();
            colliderObserver.Initialize(_world, player);
            var rigidbody = unitActor.Rigidbody2D;

            ref var movableComponent = ref player.Get<RigidbodyMovableComponent>();
            movableComponent.movingData = _playerInitData.MovingData;
            movableComponent.rigidbody = rigidbody;
            movableComponent.transform = unitActor.transform;

            ref var rotatableComponent = ref player.Get<RigidbodyRotatableComponent>();
            rotatableComponent.rigidbody = rigidbody;
            rotatableComponent.rotationData = _playerInitData.RotationData;

            ref var animationsComponent = ref player.Get<AnimatedCharacterComponent>();
            animationsComponent.animator = unitActor.Animator;

            ref var moveParticleComponent = ref player.Get<MoveParticleComponent>();
            moveParticleComponent.particleSystem = unitActor.MoveParticleSystem;

            ref var collisionParticleComponent = ref player.Get<SelfCollisionParticleComponent>();
            collisionParticleComponent.particleSystem = unitActor.CollisionParticleSystem;

            ref var healthComponent = ref player.Get<HealthComponent>();
            healthComponent.maxValue = _playerInitData.HealthValue;
            healthComponent.currentValue = _playerInitData.HealthValue;

            return unitActor;
        }

        private void CreateEnemy(Vector3 atPosition, Transform target)
        {
            var unitActor = Object.Instantiate(_enemyInitData.UnitPrefab, atPosition, Quaternion.identity);

            var enemy = _world.NewEntity();

            RemoveCollidersInteractions(unitActor.InternalColliders);

            enemy.Get<CollisionDestructionComponent>();

            ref var targetableComponent = ref enemy.Get<TargetableComponent>();
            targetableComponent.transform = unitActor.transform;
            targetableComponent.team = Teams.Enemy;

            var colliderObserver = unitActor.GetComponent<ColliderObserver>();
            colliderObserver.Initialize(_world, enemy);

            ref var enemyMovableComponent = ref enemy.Get<MovableComponent>();
            enemyMovableComponent.moveSpeed = _enemyInitData.DefaultSpeed;
            enemyMovableComponent.transform = unitActor.transform;

            ref var enemyRotatableComponent = ref enemy.Get<RotatableComponent>();
            enemyRotatableComponent.transform = unitActor.transform;

            ref var enemyAnimationsComponent = ref enemy.Get<AnimatedCharacterComponent>();
            enemyAnimationsComponent.animator = unitActor.Animator;

            ref var followComponent = ref enemy.Get<FollowComponent>();
            followComponent.target = target;

            ref var enemyMoveParticleComponent = ref enemy.Get<MoveParticleComponent>();
            enemyMoveParticleComponent.particleSystem = unitActor.MoveParticleSystem;

            ref var healthComponent = ref enemy.Get<HealthComponent>();
            healthComponent.maxValue = _enemyInitData.HealthValue;
            healthComponent.currentValue = _enemyInitData.HealthValue;
        }

        private void RemoveCollidersInteractions(Collider2D[] colliders)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                for (int j = 0; j < colliders.Length; j++)
                {
                    Physics2D.IgnoreCollision(colliders[i], colliders[j]);
                }
            }
        }
    }
}