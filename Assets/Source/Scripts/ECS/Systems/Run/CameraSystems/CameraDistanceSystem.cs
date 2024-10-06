using ECS.Components;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class CameraDistanceSystem : IEcsRunSystem
    {
        private EcsFilter<CameraComponent, RigidbodyMovableComponent, PlayerComponent> _filter = null;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var cameraComponent = ref _filter.Get1(index);
                ref var movableComponent = ref _filter.Get2(index);

                var speedRate = movableComponent.rigidbody.velocity.magnitude / movableComponent.movingData.maxSpeedForward;
                Vector3 newPosition = cameraComponent.defaultPosition;
                newPosition.z += newPosition.z * speedRate * cameraComponent.distanceRate;
                cameraComponent.camera.transform.position = newPosition;
            }
        }
    }
}