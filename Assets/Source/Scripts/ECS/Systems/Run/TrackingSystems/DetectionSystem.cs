using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class DetectionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TrackerComponent, DetectionComponent> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var trackerComponent = ref _filter.Get1(index);
                ref var detectionComponent = ref _filter.Get2(index);

                if (trackerComponent.targetTransform == null)
                {
                    detectionComponent.isInRange = false;
                    continue;
                }

                var targetPosition = trackerComponent.targetTransform.position;
                var selfPosition = trackerComponent.selfTransform.position;
                var selfForward = trackerComponent.selfTransform.up;
                var distance = Vector2.Distance(targetPosition, selfPosition);
                var direction = targetPosition - selfPosition;
                var angle = Vector2.Angle(selfForward, direction);

                bool isWithingDistance = detectionComponent.radius >= distance;
                bool isWithingAngle = Mathf.Abs(angle) <= detectionComponent.angle;
                detectionComponent.isInRange = isWithingDistance && isWithingAngle;
            }
        }
    }
}