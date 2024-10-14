using ECS.Components;
using ECS.Components.Movement;
using ECS.Data;
using ECS.MonoBehaviours;
using EntityActors;
using Leopotam.Ecs;
using UnityEngine;
using Utilitiy;

namespace Systems
{
    public class ProjectileBuilder : EcsBuilder
    {
        private ProjectileActor _projectileActor;
        private EcsEntity _projectileEntity;

        public ProjectileBuilder(EcsWorld world) : base(world)
        {
        }

        public void Build(WeaponComponent weapon)
        {
            _projectileActor =
                Object.Instantiate(weapon.projectileData.ProjectilePrefab, weapon.shootingPoint.position, weapon.shootingPoint.rotation);
            _projectileEntity = _world.NewEntity();
            _projectileActor.GetComponent<ColliderObserver>().Initialize(_world, _projectileEntity);

            _projectileEntity.Get<CollisionDestructionComponent>();

            ref var projectileComponent = ref _projectileEntity.Get<ProjectileComponent>();
            projectileComponent.projectile = _projectileActor.gameObject;

            ref var particleComponent = ref _projectileEntity.Get<InstanceCollisionParticleComponent>();
            particleComponent.particleSystem = _projectileActor.CollisionParticleSystem;

            ref var movableComponent = ref _projectileEntity.Get<RigidbodyInstantMovableComponent>();
            movableComponent.rigidbody = _projectileActor.Rigidbody2D;
            movableComponent.velocity = weapon.shootingPoint.transform.up * weapon.shotForce;

            ref var damageInflictComponent = ref _projectileEntity.Get<DamageInflictComponent>();
            damageInflictComponent.value = weapon.damageValue;
        }

        public void BuildHoming(WeaponComponent weapon)
        {
            Build(weapon);

            _projectileEntity.Get<FollowComponent>();

            ref var movableComponent = ref _projectileEntity.Get<RigidbodyMovableComponent>();
            movableComponent.rigidbody = _projectileActor.Rigidbody2D;
            movableComponent.transform = _projectileActor.transform;
            movableComponent.movingData = weapon.projectileData.MovingData;

            ref var rotatableComponent = ref _projectileEntity.Get<RigidbodyRotatableComponent>();
            rotatableComponent.rigidbody = _projectileActor.Rigidbody2D;
            rotatableComponent.rotationData = weapon.projectileData.RotationData;

            ref var trackerComponent = ref _projectileEntity.Get<TrackerComponent>();
            trackerComponent.searchRadius = weapon.projectileData.TrackerRange;
            trackerComponent.selfTeam = Teams.Player;
            trackerComponent.selfTransform = _projectileActor.transform;
        }
    }
}