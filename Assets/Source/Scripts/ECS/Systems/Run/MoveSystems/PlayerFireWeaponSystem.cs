using ECS.Components;
using ECS.Components.Input;
using Leopotam.Ecs;

namespace Systems
{
    public struct PlayerFireWeaponSystem : IEcsRunSystem
    {
        EcsFilter<ShootInputComponent, WeaponComponent>.Exclude<ReloadComponent, RechargeComponent> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var shootInputComponent = ref _filter.Get1(index);
                ref var entity = ref _filter.GetEntity(index);

                if (shootInputComponent.isActive)
                {
                    entity.Get<ShotComponent>();
                }
            }
        }
    }
}