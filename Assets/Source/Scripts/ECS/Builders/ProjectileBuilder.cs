using ECS.Components;
using ECS.Components.Movement;
using ECS.MonoBehaviours;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class ProjectileBuilder : EcsBuilder
    {
        public ProjectileBuilder(EcsWorld world) : base(world)
        {
        }

        public void Build(WeaponComponent weapon)
        {
            var projectile =
                Object.Instantiate(weapon.projectile, weapon.shootingPoint.position, weapon.shootingPoint.rotation);
            var projectileEntity = _world.NewEntity();
            projectile.GetComponent<ColliderObserver>().Initialize(_world, projectileEntity);

            projectileEntity.Get<CollisionDestructionComponent>();

            ref var projectileComponent = ref projectileEntity.Get<ProjectileComponent>();
            projectileComponent.projectile = projectile.gameObject;

            ref var particleComponent = ref projectileEntity.Get<InstanceCollisionParticleComponent>();
            particleComponent.particleSystem = projectile.CollisionParticleSystem;

            ref var movableComponent = ref projectileEntity.Get<RigidbodyInstantMovableComponent>();
            movableComponent.rigidbody = projectile.Rigidbody2D;
            movableComponent.velocity = weapon.shootingPoint.transform.up * weapon.shotForce;

            ref var collisionObjectDestructionComponent = ref projectileEntity.Get<CollisionObjectDestructionComponent>();
            collisionObjectDestructionComponent.destroyObject = projectile.gameObject;

            ref var damageInflictComponent = ref projectileEntity.Get<DamageInflictComponent>();
            damageInflictComponent.value = weapon.damageValue;
        }


    }
}