using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class HealthDamageSystem : IEcsRunSystem
    {
        private EcsFilter<HealthComponent, DamageReceiveComponent> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var health = ref _filter.Get1(index);
                ref var damage = ref _filter.Get2(index);
                var entity = _filter.GetEntity(index);

                if (health.currentValue <= health.minValue)
                {
                    Debug.Log("Dead");
                    entity.Del<TargetableComponent>();
                    continue;
                }

                health.currentValue -= damage.value;
                Debug.Log("Damaged by - " + damage.value + " Current health - " + health.currentValue );

                entity.Del<DamageReceiveComponent>();
            }
        }
    }
}