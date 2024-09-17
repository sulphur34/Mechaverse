using ECS.Components;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class FollowMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MovableComponent, FollowComponent> _filter;
        private readonly float _stopDistance = 2f;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var followComponent = ref _filter.Get2(index);
                ref var movableComponent = ref _filter.Get1(index);

                if (followComponent.target == null)
                {
                    continue;
                }

                var direction = (followComponent.target.position - movableComponent.transform.position).normalized;
                var distance = Vector3.Distance(followComponent.target.position, movableComponent.transform.position);
                var isMoving = distance > _stopDistance;

                if (isMoving)
                {
                    movableComponent.transform.position += direction * (Time.deltaTime * movableComponent.moveSpeed);
                }
            }
        }
    }
}