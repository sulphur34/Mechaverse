using EntityActors;
using UnityEngine;

namespace ECS.Components
{
    public struct WeaponComponent
    {
        public ProjectileActor projectile;
        public Transform shootingPoint;
        public float shotDelay;
        public float shotForce;
        public float reloadingSpeed;
        public float damageValue;
    }
}