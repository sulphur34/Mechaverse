using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class RechargingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<RechargeComponent> _filter;
        private EcsWorld _ecsWorld;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var rechgarge = ref _filter.Get1(index);

                rechgarge.timeLeft += Time.deltaTime;

                if(rechgarge.timeLeft >= rechgarge.rechargeDuration)
                    _filter.GetEntity(index).Del<RechargeComponent>();
            }
        }
    }
}