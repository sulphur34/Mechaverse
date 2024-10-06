using System;
using ECS.Components.Input;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class ShootInputSystem : IEcsRunSystem
    {
        EcsFilter<ShootInputComponent> _filter;


        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var shootInputComponent = ref _filter.Get1(index);

                shootInputComponent.isActive = Input.GetMouseButton(0);
            }
        }
    }
}