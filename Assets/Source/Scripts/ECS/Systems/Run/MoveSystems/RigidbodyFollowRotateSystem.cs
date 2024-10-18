using ECS.Components;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class RigidbodyFollowRotateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<FollowComponent, RigidbodyRotatableComponent> _followRotateFilter;

        public void Run()
        {
            foreach (var entity in _followRotateFilter)
            {
                ref var followComponent = ref _followRotateFilter.Get1(entity);
                ref var rotatableComponent = ref _followRotateFilter.Get2(entity);

                if (followComponent.target == null)
                    continue;

                var rotationData = rotatableComponent.rotationData;
                var rigidbody = rotatableComponent.rigidbody;
                Vector2 targetDirection =
                    (followComponent.target.position - rigidbody.transform.position).normalized;
                targetDirection = new Vector2(targetDirection.y, -targetDirection.x);

                if (targetDirection == Vector2.zero)
                    continue;

                Debug.DrawLine(rigidbody.position, followComponent.target.position, Color.cyan);
                float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                float currentAngle = rigidbody.rotation;
                float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
                float dampingTorque = -rigidbody.angularVelocity * 0.5f;
                float torque = angleDifference * rotationData.acceleration + dampingTorque;
                rotatableComponent.rigidbody.AddTorque(torque);
                float clampedAngularVelocity = Mathf.Clamp(rigidbody.angularVelocity, -rotationData.maxSpeed,
                    rotationData.maxSpeed);
                rigidbody.angularVelocity = clampedAngularVelocity;
            }
        }
    }
}