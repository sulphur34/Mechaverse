using ECS.Components.Input;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class PlayerMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MovableComponent, MoveInputEventComponent> _playerMoveFilter;

        public void Run()
        {
            foreach (var entity in _playerMoveFilter)
            {
                ref var movableComponent = ref _playerMoveFilter.Get1(entity);
                ref var inputComponent = ref _playerMoveFilter.Get2(entity);

                movableComponent.transform.Translate(inputComponent.direction *
                                                     Time.deltaTime * movableComponent.moveSpeed, Space.Self);
                movableComponent.isMoving = inputComponent.direction.sqrMagnitude > 0;
            }
        }
    }
}