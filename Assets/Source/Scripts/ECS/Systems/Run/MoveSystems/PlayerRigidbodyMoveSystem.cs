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
                var velocityY = inputComponent.direction.y < 0
                    ? inputComponent.direction.y * movingData.accelerationBackward
                    : inputComponent.direction.y * movingData.accelerationForward;

                var velocityX = inputComponent.direction.x * movingData.accelerationSide;

                velocityY = Mathf.Clamp(currentVelocityY + velocityY, movingData.maxSpeedBackward,
                    movingData.maxSpeedForward) - currentVelocityY;
                velocityX =
                    Mathf.Clamp(currentVelocityX + velocityX, movingData.maxSpeedLeft, movingData.maxSpeedRight) -
                    currentVelocityX;
                var localForce = new Vector2(velocityX, velocityY);
                var worldForce = movableComponent.transform.TransformDirection(localForce);
                Debug.DrawRay(movableComponent.transform.position, localForce, Color.yellow);
                Debug.DrawRay(movableComponent.transform.position, worldForce, Color.blue);
                movableComponent.rigidbody.AddForce(worldForce, ForceMode2D.Impulse);
                movableComponent.isMoving = inputComponent.direction.sqrMagnitude > 0;
            }
        }
    }
}