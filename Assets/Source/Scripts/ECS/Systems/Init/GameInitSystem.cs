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

        public GameInitSystem(
            UnitInitData playerInitData,
            UnitInitData enemyInitData,
            TurretInitData turretInitData,
            Transform spawnPoint)
        {
            _playerInitData = playerInitData;
            _enemyInitData = enemyInitData;
            _turretInitData = turretInitData;
            _spawnPoint = spawnPoint;
        }

        public void Init()
        {
            var playerActor = CreatePlayer();
            CreateTurret(playerActor);

            for (int i = 0; i < 10; i++)
            {
                var enemySpawnPosition = _spawnPoint.position +
                                         new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0f);
                CreateEnemy(enemySpawnPosition, playerActor.transform);
            }
        }

        private void CreateTurret(UnitActor unitActor)
        {
            var turretActor = Object.Instantiate(
                _turretInitData.TurretPrefab,
                unitActor.TurretPlaceholder.transform.position,
                Quaternion.identity);

            var turret = _world.NewEntity();
            turretActor.HingeJoint.connectedBody = unitActor.Rigidbody2D;
            turretActor.HingeJoint.anchor = Vector2.zero;
            turretActor.HingeJoint.connectedAnchor = unitActor.TurretPlaceholder.transform.position;

            ref var trackerComponent = ref turret.Get<TrackerComponent>();
            trackerComponent.searchRadius = 100f;
            trackerComponent.selfTeam = Teams.Player;
            trackerComponent.selfTransform = turretActor.transform;

            ref var rotatableComponent = ref turret.Get<RigidbodyRotatableComponent>();
            rotatableComponent.rigidbody = turretActor.Rigidbody2D;
            rotatableComponent.rotationData = _playerInitData.RotationData;

            turret.Get<FollowComponent>();
        }

        private UnitActor CreatePlayer()
        {
            var unitActor = Object.Instantiate(_playerInitData.UnitPrefab, _spawnPoint.position, Quaternion.identity);
            var player = _world.NewEntity();
            player.Get<PlayerComponent>();
            player.Get<RotationInputEventComponent>();
            player.Get<MoveInputEventComponent>();
            player.Get<PlayerComponent>();

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

            ref var collisionParticleComponent = ref player.Get<CollisionParticleComponent>();
            collisionParticleComponent.particleSystem = unitActor.CollisionParticleSystem;

            return unitActor;
        }

        private void CreateEnemy(Vector3 atPosition, Transform target)
        {
            var unitActor = Object.Instantiate(_enemyInitData.UnitPrefab, atPosition, Quaternion.identity);

            var enemy = _world.NewEntity();

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
        }
    }
}