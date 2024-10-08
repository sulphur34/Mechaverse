using ECS.Components;
using Leopotam.Ecs;

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
                    ref var destruction = ref entity.Get<DestructionComponent>();
                    destruction.destroyObject = health.unit;

                    entity.Del<FollowComponent>();
                    entity.Del<TargetableComponent>();
                    continue;
                }

                health.currentValue -= damage.value;

                entity.Del<DamageReceiveComponent>();
            }
        }
    }
}