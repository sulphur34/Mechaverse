using Data;
using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class WeaponBuilder : EcsBuilder
    {
        public WeaponBuilder(EcsWorld world) : base(world)
        {
        }

        public EcsEntity Build(WeaponInitConfig weaponInitConfig, Transform container,
            Vector2 position)
        {
            var weaponActor = Object.Instantiate(weaponInitConfig.WeaponActor, container);
            weaponActor.transform.position = position;
            var weapon = _world.NewEntity();

            ref var weaponComponent = ref weapon.Get<WeaponComponent>();
            weaponComponent.ProjectileConfig = weaponInitConfig.ProjectileInitConfig;
            weaponComponent.reloadingSpeed = weaponInitConfig.ReloadingSpeed;
            weaponComponent.shotDelay = weaponInitConfig.ShotDelay;
            weaponComponent.shotForce = weaponInitConfig.ShotForce;
            weaponComponent.shootingPoint = weaponActor.ShootPoint;
            weaponComponent.damageValue = weaponInitConfig.DamageValue;

            ref var ammoComponent = ref weapon.Get<AmmoComponent>();
            ammoComponent.maxCapacity = weaponInitConfig.MagazineCapacity;
            ammoComponent.currentCapacity = ammoComponent.maxCapacity;

            return weapon;
        }
    }
}