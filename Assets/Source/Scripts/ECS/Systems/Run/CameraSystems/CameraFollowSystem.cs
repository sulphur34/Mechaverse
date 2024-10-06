using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class CameraFollowSystem : IEcsRunSystem
    {

        private EcsFilter<CameraComponent, TargetableComponent, PlayerComponent> _filter = null;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var camera = ref _filter.Get1(index);
                ref var target = ref _filter.Get2(index);

                var targetPosition = target.transform.position;
                var cameraPositionZ = camera.camera.transform.position.z;
                camera.camera.transform.position = new Vector3(targetPosition.x, targetPosition.y, cameraPositionZ);
            }
        }
    }
}