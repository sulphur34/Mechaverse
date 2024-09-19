using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class InstanceCollisionParticleSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CollisionEnterComponent, InstanceCollisionParticleComponent> _filter = null;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var collisionEnter = ref _filter.Get1(index);
                ref var collisionParticle = ref _filter.Get2(index);

                var particleSystem = Object.Instantiate(
                    collisionParticle.particleSystem,
                    collisionEnter.collisionPoint,
                    Quaternion.identity);
            }
        }
    }
}