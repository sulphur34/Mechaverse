using ECS.Components;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class FollowMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MovableComponent, FollowComponent> _enemyFollowSystem;
        private readonly float _stopDistance = 2f;

        public void Run()
        {
            foreach (var entity in _enemyFollowSystem)
            {
                ref var followComponent = ref _enemyFollowSystem.Get2(entity);
                ref var movableComponent = ref _enemyFollowSystem.Get1(entity);

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