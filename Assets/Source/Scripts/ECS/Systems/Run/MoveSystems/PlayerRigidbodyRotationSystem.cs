using ECS.Components.Input;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class PlayerRigidbodyRotationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<RigidbodyRotatableComponent, RotationInputEventComponent> _playerMoveFilter;

        public void Run()
        {
            foreach (var entity in _playerMoveFilter)
            {
                ref var rotatableComponent = ref _playerMoveFilter.Get1(entity);
                ref var inputComponent = ref _playerMoveFilter.Get2(entity);
                Vector2 targetDirection = new Vector2(inputComponent.direction.y, -inputComponent.direction.x);

                if (targetDirection == Vector2.zero)
                    continue;

                var rotationData = rotatableComponent.rotationData;
                var rigidbody = rotatableComponent.rigidbody;

                float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                float currentAngle = rotatableComponent.rigidbody.rotation;
                float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
                float dampingTorque = -rigidbody.angularVelocity * rotationData.dumpingFactor;
                float torque = angleDifference * rotationData.acceleration + dampingTorque;
                rigidbody.AddTorque(torque);
                float clampedAngularVelocity = Mathf.Clamp(
                    rigidbody.angularVelocity,
                    -rotationData.maxSpeed,
                    rotationData.maxSpeed);
                rigidbody.angularVelocity = clampedAngularVelocity;
            }
        }
    }
}