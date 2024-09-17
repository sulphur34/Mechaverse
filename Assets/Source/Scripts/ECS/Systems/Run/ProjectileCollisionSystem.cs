using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class ProjectileCollisionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CollisionEnterComponent, ProjectileComponent> _filter = null;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var projectile = ref _filter.Get2(i);

                Object.Destroy(projectile.rigidbody2D.gameObject);
                _filter.GetEntity(i).Destroy();
            }
        }
    }
}