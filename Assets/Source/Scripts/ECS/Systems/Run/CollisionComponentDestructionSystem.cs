using ECS.Components;
using Leopotam.Ecs;

namespace Systems
{
    public class CollisionComponentDestructionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CollisionEnterComponent, CollisionDestructionComponent> _filter = null;

        public void Run()
        {
            foreach (var index in _filter)
            {
                _filter.GetEntity(index).Del<CollisionEnterComponent>();
            }
        }
    }
}