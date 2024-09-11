using ECS.Components.Input;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class PlayerRigidbodyMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<RigidbodyMovableComponent, MoveInputEventComponent> _playerMoveFilter;

        public void Run()
        {
            foreach (var entity in _playerMoveFilter)
            {
                ref var movableComponent = ref _playerMoveFilter.Get1(entity);
                ref var inputComponent = ref _playerMoveFilter.Get2(entity);

                var movingData = movableComponent.movingData;
                var rigidbody = movableComponent.rigidbody;
                Vector2 localVelocity = movableComponent.transform.InverseTransformDirection(rigidbody.velocity);
                var currentVelocityX = localVelocity.x;
                var currentVelocityY = localVelocity.y;

                var velocityX = inputComponent.direction.y < 0
                    ? -inputComponent.direction.y * movingData.accelerationBackward
                    : -inputComponent.direction.y * movingData.accelerationForward;

                var velocityY = inputComponent.direction.x * movingData.accelerationSide;

                velocityY = Mathf.Clamp(currentVelocityY + velocityY, movingData.maxSpeedBackward,
                    movingData.maxSpeedForward) - currentVelocityY;
                velocityX =
                    Mathf.Clamp(currentVelocityX + velocityX, movingData.maxSpeedLeft, movingData.maxSpeedRight) -
                    currentVelocityX;

                var localForce = movableComponent.transform.TransformDirection(new Vector2(velocityX, velocityY));

                movableComponent.rigidbody.AddForce(localForce, ForceMode2D.Impulse);
                movableComponent.isMoving = inputComponent.direction.sqrMagnitude > 0;
            }
        }
    }
}