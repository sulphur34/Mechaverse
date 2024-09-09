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
                if (targetDirection == Vector2.zero) continue;
                float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                float currentAngle = rotatableComponent.rigidbody.rotation;
                float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
                float torque = angleDifference * rotatableComponent.rotationData.acceleration;
                rotatableComponent.rigidbody.AddTorque(torque);
                float clampedAngularVelocity = Mathf.Clamp(rotatableComponent.rigidbody.angularVelocity, -rotatableComponent.rotationData.maxSpeed, rotatableComponent.rotationData.maxSpeed);
                rotatableComponent.rigidbody.angularVelocity = clampedAngularVelocity;
            }
        }
    }
}