using ECS.Components.Input;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class PlayerRotateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<RotatableComponent, RotationInputEventComponent> _playerMoveFilter;

        public void Run()
        {
            foreach (var entity in _playerMoveFilter)
            {
                ref var rotatableComponent = ref _playerMoveFilter.Get1(entity);
                ref var inputComponent = ref _playerMoveFilter.Get2(entity);

                rotatableComponent.transform.right = new Vector3(inputComponent.direction.y, -inputComponent.direction.x, 0f);
            }
        }
    }
}