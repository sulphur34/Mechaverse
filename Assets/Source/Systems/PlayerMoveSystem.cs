using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class PlayerMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MovableComponent, InputEventComponent> _playerMoveFilter;

        public void Run()
        {
            foreach (var entity in _playerMoveFilter)
            {
                ref var movableComponent = ref _playerMoveFilter.Get1(entity);
                ref var inputComponent = ref _playerMoveFilter.Get2(entity);

                movableComponent.transform.position +=
                    inputComponent.direction * (Time.deltaTime * movableComponent.moveSpeed);
                movableComponent.isMoving = inputComponent.direction.sqrMagnitude > 0;
            }
        }
    }
}