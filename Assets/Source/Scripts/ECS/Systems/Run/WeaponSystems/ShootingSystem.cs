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

                var projectileBuilder = new ProjectileBuilder(_ecsWorld);

                projectileBuilder.Build(weapon);

                ref var entity = ref _filter.GetEntity(index);
                entity.Del<ShotComponent>();
                ammo.currentCapacity--;

                ref var recharge = ref entity.Get<RechargeComponent>();
                recharge.rechargeDuration = weapon.shotDelay;

                if (ammo.currentCapacity <= 0)
                {
                    ref var reload = ref entity.Get<ReloadComponent>();
                    reload.reloadDuration = weapon.reloadingSpeed;
                }
            }
        }
    }
}