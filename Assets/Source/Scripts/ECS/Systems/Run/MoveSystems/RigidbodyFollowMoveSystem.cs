using ECS.Components;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class RigidbodyFollowMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<FollowComponent, RigidbodyMovableComponent> _followRotateFilter;

        public void Run()
        {
            foreach (var entity in _followRotateFilter)
            {
                ref var followComponent = ref _followRotateFilter.Get1(entity);
                ref var movableComponent = ref _followRotateFilter.Get2(entity);

                if (followComponent.target == null)
                {
                    continue;
                }

                var movingData = movableComponent.movingData;
                var rigidbody = movableComponent.rigidbody;
                var transform = movableComponent.transform;

                var worldDirection = (followComponent.target.position - movableComponent.transform.position).normalized;
                var localDirection = transform.InverseTransformDirection(worldDirection);

                if (_followRotateFilter.GetEntity(entity).Has<ProjectileComponent>())
                {
                    Debug.Log(worldDirection);
                }

                var currentVelocityX = rigidbody.velocity.x;
                var currentVelocityY = rigidbody.velocity.y;

                var velocityY = localDirection.y < 0
                    ? localDirection.y * movingData.accelerationBackward
                    : localDirection.y * movingData.accelerationForward;

                var velocityX = localDirection.x * movingData.accelerationSide;

                if(velocityY == 0 && velocityX == 0)
                    continue;

                velocityY = Mathf.Clamp(currentVelocityY + velocityY, movingData.maxSpeedBackward,
                    movingData.maxSpeedForward) - currentVelocityY;
                velocityX =
                    Mathf.Clamp(currentVelocityX + velocityX, movingData.maxSpeedLeft, movingData.maxSpeedRight) -
                    currentVelocityX;

                var localForce = new Vector2(velocityX, velocityY);
                var worldForce = transform.TransformDirection(localForce);

                Debug.DrawRay(movableComponent.transform.position, localForce, Color.green);
                Debug.DrawRay(movableComponent.transform.position, worldForce, Color.red);
                Debug.DrawLine(movableComponent.transform.position, followComponent.target.position, Color.cyan);

                rigidbody.AddForce(worldForce, ForceMode2D.Force);
                movableComponent.isMoving = rigidbody.velocity.sqrMagnitude > 0;
            }
        }
    }
}