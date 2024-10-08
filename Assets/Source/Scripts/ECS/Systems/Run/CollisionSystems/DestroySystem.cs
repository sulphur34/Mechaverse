using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class DestroySystem : IEcsRunSystem
    {
        private readonly EcsFilter<DestructionComponent> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var objectDestructionComponent = ref _filter.Get1(i);

                Object.Destroy(objectDestructionComponent.destroyObject);
                _filter.GetEntity(i).Destroy();
            }
        }
    }
}