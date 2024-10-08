using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class ProjectileCollisionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CollisionEnterComponent, ProjectileComponent> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var projectile = ref _filter.Get2(i);
                ref var entity = ref _filter.GetEntity(i);

                ref var destruction = ref entity.Get<DestructionComponent>();
                destruction.destroyObject = projectile.projectile;
            }
        }
    }
}