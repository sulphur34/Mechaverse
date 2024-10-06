using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class OnCollisionObjectDestroySystem : IEcsRunSystem
    {
        private readonly EcsFilter<CollisionEnterComponent, CollisionObjectDestructionComponent> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var objectDestructionComponent = ref _filter.Get2(i);

                Object.Destroy(objectDestructionComponent.destroyObject);
                _filter.GetEntity(i).Destroy();
            }
        }
    }
}