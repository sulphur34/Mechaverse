using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class HealthRefillSystem : IEcsRunSystem
    {
        private EcsFilter<HealthComponent, HealthReceiveComponent> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var health = ref _filter.Get1(index);
                ref var receive = ref _filter.Get2(index);
                var entity = _filter.GetEntity(index);

                if (health.currentValue <= health.minValue)
                {
                    entity.Del<FollowComponent>();
                    entity.Del<TargetableComponent>();
                    continue;
                }

                health.currentValue += receive.receiveAmount;
                health.currentValue = Mathf.Clamp(health.currentValue, health.minValue, health.maxValue);

                entity.Del<DamageReceiveComponent>();
            }
        }
    }
}