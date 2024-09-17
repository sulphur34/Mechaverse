using ECS.Components;
using Leopotam.Ecs;

namespace Systems
{
    public class SelfCollisionParticleSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CollisionEnterComponent, SelfCollisionParticleComponent> _collisionEnteredFilter = null;

        public void Run()
        {
            foreach (var i in _collisionEnteredFilter)
            {
                ref var collisionEnter = ref _collisionEnteredFilter.Get1(i);
                ref var collisionParticle = ref _collisionEnteredFilter.Get2(i);

                var particleSystem = collisionParticle.particleSystem;
                particleSystem.gameObject.transform.position = collisionEnter.collisionPoint;
                particleSystem.Play();
            }
        }
    }
}