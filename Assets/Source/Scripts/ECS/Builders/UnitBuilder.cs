using Data;
using ECS.Components;
using ECS.Components.Input;
using ECS.Components.Movement;
using ECS.Data;
using ECS.MonoBehaviours;
using EntityActors;
using Leopotam.Ecs;
using Unity.VisualScripting;
using UnityEngine;
using Utilitiy;

namespace Systems
{
    public class UnitBuilder : EcsBuilder
    {
        private UnitActor _unitActor;
        private EcsEntity _entity;

        public UnitBuilder(EcsWorld world) : base(world)
        {
        }

        public UnitActor BuildPlayer(UnitInitData unitInitData, Vector3 spawnPoint)
        {
            BuildUnit(unitInitData, spawnPoint);

            ref var cameraComponent = ref _entity.Get<CameraComponent>();
            cameraComponent.camera = Camera.main;
            cameraComponent.defaultPosition = Camera.main.transform.position;
            cameraComponent.distanceRate = 2f;

            ref var targetableComponent = ref _entity.Get<TargetableComponent>();
            targetableComponent.transform = _unitActor.transform;
            targetableComponent.team = Teams.Player;

            _entity.Get<PlayerComponent>();
            _entity.Get<RotationInputEventComponent>();
            _entity.Get<MoveInputEventComponent>();
            _entity.Get<PlayerComponent>();

            return _unitActor;
        }

        public UnitActor BuildEnemy(UnitInitData unitInitData, Vector3 spawnPoint, Transform target)
        {
            BuildUnit(unitInitData, spawnPoint);

            ref var targetableComponent = ref _entity.Get<TargetableComponent>();
            targetableComponent.transform = _unitActor.transform;
            targetableComponent.team = Teams.Enemy;

            // ref var enemyMovableComponent = ref _entity.Get<MovableComponent>();
            // enemyMovableComponent.moveSpeed = unitInitData.DefaultSpeed;
            // enemyMovableComponent.transform = _unitActor.transform;

            // ref var enemyRotatableComponent = ref _entity.Get<RotatableComponent>();
            // enemyRotatableComponent.transform = _unitActor.transform;

            ref var enemyAnimationsComponent = ref _entity.Get<AnimatedCharacterComponent>();
            enemyAnimationsComponent.animator = _unitActor.Animator;

            ref var followComponent = ref _entity.Get<FollowComponent>();
            followComponent.target = target;

            ref var enemyMoveParticleComponent = ref _entity.Get<MoveParticleComponent>();
            enemyMoveParticleComponent.particleSystem = _unitActor.MoveParticleSystem;

            return _unitActor;
        }

        private void BuildUnit(UnitInitData unitInitData, Vector3 spawnPoint)
        {
            _unitActor = Object.Instantiate(unitInitData.UnitPrefab, spawnPoint, Quaternion.identity);
            _entity = _world.NewEntity();

            CollidersInteractionsRemover.Remove(_unitActor.InternalColliders);

            _entity.Get<CollisionDestructionComponent>();

            var colliderObserver = _unitActor.GetComponent<ColliderObserver>();
            colliderObserver.Initialize(_world, _entity);
            var rigidbody = _unitActor.Rigidbody2D;

            ref var movableComponent = ref _entity.Get<RigidbodyMovableComponent>();
            movableComponent.movingData = unitInitData.MovingData;
            movableComponent.rigidbody = rigidbody;
            movableComponent.transform = _unitActor.transform;

            ref var rotatableComponent = ref _entity.Get<RigidbodyRotatableComponent>();
            rotatableComponent.rigidbody = rigidbody;
            rotatableComponent.rotationData = unitInitData.RotationData;

            ref var animationsComponent = ref _entity.Get<AnimatedCharacterComponent>();
            animationsComponent.animator = _unitActor.Animator;

            ref var moveParticleComponent = ref _entity.Get<MoveParticleComponent>();
            moveParticleComponent.particleSystem = _unitActor.MoveParticleSystem;

            ref var collisionParticleComponent = ref _entity.Get<SelfCollisionParticleComponent>();
            collisionParticleComponent.particleSystem = _unitActor.CollisionParticleSystem;

            ref var healthComponent = ref _entity.Get<HealthComponent>();
            healthComponent.maxValue = unitInitData.HealthValue;
            healthComponent.currentValue = unitInitData.HealthValue;
            healthComponent.unit = _unitActor.gameObject;
        }
    }
}