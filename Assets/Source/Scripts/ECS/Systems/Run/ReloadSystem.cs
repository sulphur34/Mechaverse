using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class ReloadSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ReloadComponent, AmmoComponent> _filter;
        private EcsWorld _ecsWorld;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var reload = ref _filter.Get1(index);
                ref var ammo = ref _filter.Get2(index);

                reload.timeLeft += Time.deltaTime;

                if (reload.timeLeft >= reload.reloadDuration)
                {
                    ammo.currentCapacity = ammo.maxCapacity;
                    _filter.GetEntity(index).Del<ReloadComponent>();
                }
            }
        }
    }
}