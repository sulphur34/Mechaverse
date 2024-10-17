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

                if (_followRotateFilter.GetEntity(entity).Has<ProjectileComponent>())
                {
                    Debug.Log(rotatableComponent.rigidbody.angularVelocity);
                }

                Vector2 targetDirection =
                    followComponent.target.position - rotatableComponent.rigidbody.transform.position;

                if (targetDirection == Vector2.zero)
                    continue;

                // Debug.DrawRay(movableComponent.transform.position, localForce, Color.green);
                // Debug.DrawRay(movableComponent.transform.position, worldForce, Color.red);
                Debug.DrawLine(rotatableComponent.rigidbody.position, followComponent.target.position, Color.cyan);
                var currentVelocity = rotatableComponent.rigidbody.angularVelocity;
                float targetAngle = Mathf.Atan2(-targetDirection.x, targetDirection.y) * Mathf.Rad2Deg;
                float currentAngle = rotatableComponent.rigidbody.rotation;
                float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
                float torque = angleDifference * rotatableComponent.rotationData.acceleration;
                var maxTorque = rotatableComponent.rotationData.maxSpeed;
                torque = Mathf.Clamp(currentVelocity + torque, -maxTorque, maxTorque) - currentVelocity;
                rotatableComponent.rigidbody.AddTorque(torque);
            }
        }
    }
}