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

                var direction = (followComponent.target.position - movableComponent.transform.position).normalized;

                Vector2 localVelocity = movableComponent.transform.InverseTransformDirection(rigidbody.velocity);
                var currentVelocityX = localVelocity.x;
                var currentVelocityY = localVelocity.y;

                var velocityY = direction.y < 0
                    ? direction.y * movingData.accelerationBackward
                    : direction.y * movingData.accelerationForward;

                var velocityX = direction.x * movingData.accelerationSide;

                velocityY = Mathf.Clamp(currentVelocityY + velocityY, movingData.maxSpeedBackward,
                    movingData.maxSpeedForward) - currentVelocityY;
                velocityX =
                    Mathf.Clamp(currentVelocityX + velocityX, movingData.maxSpeedLeft, movingData.maxSpeedRight) -
                    currentVelocityX;

                var localForce = movableComponent.transform.TransformDirection(new Vector2(velocityX, velocityY));

                movableComponent.rigidbody.AddForce(localForce, ForceMode2D.Impulse);
                movableComponent.isMoving = direction.sqrMagnitude > 0;
            }
        }
    }
}