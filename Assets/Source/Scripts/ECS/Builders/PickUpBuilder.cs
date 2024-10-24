using Data;
using ECS.Components;
using ECS.MonoBehaviours;
using Leopotam.Ecs;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems
{
    public class PickUpBuilder : EcsBuilder
    {
        public PickUpBuilder(EcsWorld world) : base(world)
        {
        }

        public void Build(PickUpsInitConfig initConfig, Vector3 spawnPoint)
        {
            var pickUpSpawnPosition = spawnPoint +
                                      new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0f);
            var pickUpActor =
                Object.Instantiate(initConfig.PickUpActor, pickUpSpawnPosition, Quaternion.identity);
            var pickUp = _world.NewEntity();
            pickUpActor.GetComponent<ColliderObserver>().Initialize(_world, pickUp);

            ref var healthRefillComponent = ref pickUp.Get<HealthRefillComponent>();
            healthRefillComponent.refillAmount = initConfig.HealthRestoreValue;

            ref var collisionParticleComponent = ref pickUp.Get<InstanceCollisionParticleComponent>();
            collisionParticleComponent.particleSystem = pickUpActor.CollisionParticleSystem;

            ref var collisionObjectDestructionComponent = ref pickUp.Get<DestructionComponent>();
            collisionObjectDestructionComponent.destroyObject = pickUpActor.gameObject;
        }
    }
}