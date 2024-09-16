using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class DetectionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TrackerComponent, DetectionComponent> _trackerDetectionFilter;

        public void Run()
        {
            foreach (var entity in _trackerDetectionFilter)
            {
                ref var trackerComponent = ref _trackerDetectionFilter.Get1(entity);
                ref var detectionComponent = ref _trackerDetectionFilter.Get2(entity);

                if(trackerComponent.targetTransform == null)
                    continue;

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