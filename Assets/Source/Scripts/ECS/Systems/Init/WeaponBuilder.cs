using Data;
using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class WeaponBuilder
    {
        private EcsWorld _world;

        public WeaponBuilder(EcsWorld world)
        {
            _world = world;
        }

        public EcsEntity Build(WeaponInitData weaponInitData, Transform container,
            Vector2 position)
        {
            var weaponActor = Object.Instantiate(weaponInitData.WeaponActor, container);
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