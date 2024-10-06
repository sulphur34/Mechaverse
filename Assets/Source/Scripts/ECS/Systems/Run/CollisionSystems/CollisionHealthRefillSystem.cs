using ECS.Components;
using Leopotam.Ecs;

namespace Systems
{
    public class CollisionHealthRefillSystem : IEcsRunSystem
    {
        private EcsFilter<HealthRefillComponent, CollisionEnterComponent> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var healthRefillComponent = ref _filter.Get1(index);
                ref var collisionComponent = ref _filter.Get2(index);

                var otherEntity = collisionComponent.other;

                if (otherEntity.Has<HealthComponent>())
                {
                    ref var damageReceiveComponent = ref otherEntity.Get<HealthReceiveComponent>();
                    damageReceiveComponent.receiveAmount = healthRefillComponent.refillAmount;
                }
            }
        }
    }
}