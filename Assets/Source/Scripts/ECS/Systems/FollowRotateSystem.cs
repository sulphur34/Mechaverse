using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class FollowRotateSystem: IEcsRunSystem
    {
        private readonly EcsFilter<RotatableComponent, FollowComponent> _enemyFollowSystem;

        public void Run()
        {
            foreach (var entity in _enemyFollowSystem)
            {
                ref var followComponent = ref _enemyFollowSystem.Get2(entity);
                ref var rotatableComponent = ref _enemyFollowSystem.Get1(entity);

                if (followComponent.target == null)
                {
                    continue;
                }

                var direction = (followComponent.target.position - rotatableComponent.transform.position).normalized;
                rotatableComponent.transform.right = direction;
                direction.z = 0;
            }
        }
    }
}