using System;
using ECS.Components;
using Leopotam.Ecs;

namespace Systems
{
    public class TrackingFollowSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TrackerComponent, FollowComponent> _trackerFilter;

        public void Run()
        {
            foreach (var entity in _trackerFilter)
            {
                ref var tracker = ref _trackerFilter.Get1(entity);
                ref var follow = ref _trackerFilter.Get2(entity);

                follow.target = tracker.targetTransform;
            }
        }
    }
}