using ECS.Components.Movement;
using Leopotam.Ecs;

namespace Systems
{
    public class RigidbodyInstantMoveSystem : IEcsRunSystem
    {
        private EcsFilter<RigidbodyInstantMovableComponent> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var entity = ref _filter.GetEntity(index);
                ref var move = ref _filter.Get1(index);
                move.rigidbody.AddForce(move.velocity);
                entity.Del<RigidbodyInstantMovableComponent>();
            }
        }
    }
}