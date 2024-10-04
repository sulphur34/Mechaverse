using Data;
using ECS.Components;
using ECS.MonoBehaviours;
using Leopotam.Ecs;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems
{
    public class PickUpBuilder
    {
        private EcsWorld _world;

        public PickUpBuilder(EcsWorld world)
        {
            _world = world;
        }

        public void Build(PickUpsInitData initData, Vector3 spawnPoint)
        {
            var pickUpSpawnPosition = spawnPoint +
                                      new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0f);
            var pickUpActor =
                Object.Instantiate(initData.PickUpActor, pickUpSpawnPosition, Quaternion.identity);
            var pickUp = _world.NewEntity();
            pickUpActor.GetComponent<ColliderObserver>().Initialize(_world, pickUp);

            ref var healthRefillComponent = ref pickUp.Get<HealthRefillComponent>();
            healthRefillComponent.refillAmount = initData.HealthRestoreValue;

            ref var collisionParticleComponent = ref pickUp.Get<InstanceCollisionParticleComponent>();
            collisionParticleComponent.particleSystem = pickUpActor.CollisionParticleSystem;

            ref var collisionObjectDestructionComponent = ref pickUp.Get<CollisionObjectDestructionComponent>();
            collisionObjectDestructionComponent.destroyObject = pickUpActor.gameObject;
        }
    }
}