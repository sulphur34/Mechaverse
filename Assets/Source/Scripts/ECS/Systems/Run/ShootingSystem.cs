using ECS.Components;
using ECS.MonoBehaviours;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class ShootingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<WeaponComponent, ShotComponent, AmmoComponent> _filter;
        private EcsWorld _ecsWorld;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var weapon = ref _filter.Get1(index);
                ref var ammo = ref _filter.Get3(index);

                var projectile =
                    Object.Instantiate(weapon.projectile, weapon.shootingPoint.position, weapon.shootingPoint.rotation);
                var projectileEntity = _ecsWorld.NewEntity();
                projectile.GetComponent<ColliderObserver>().Initialize(_ecsWorld,projectileEntity);
                ref var projectileComponent = ref projectileEntity.Get<ProjectileComponent>();
                projectileComponent.rigidbody2D = projectile.Rigidbody2D;
                projectileComponent.rigidbody2D.AddForce(weapon.projectile.transform.up * weapon.shotForce);
                ref var entity = ref _filter.GetEntity(index);

                entity.Del<ShotComponent>();
                ammo.currentCapacity--;

                if (ammo.currentCapacity > 1)
                {
                    ref var recharge = ref entity.Get<RechargeComponent>();
                    recharge.rechargeDuration = weapon.shotDelay;
                }
                else
                {
                   ref var reload = ref entity.Get<ReloadComponent>();
                   reload.reloadDuration = weapon.reloadingSpeed;
                }
            }
        }
    }
}